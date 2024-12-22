#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
#endregion

namespace RevitAreaReinforcement
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandCreateFloorRebar : IExternalCommand
    {
        public static List<AreaReinforcement> allAreaReinforcements { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("FloorAreaRebar"));
            Trace.WriteLine("Floor reinforcement start");
            App.ActivateConfigFolder();
            Document doc = commandData.Application.ActiveUIDocument.Document;

            allAreaReinforcements = SupportDocumentGetter.GetRebarAreas(doc);

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Floor> floors = new List<Floor>();
            foreach (ElementId id in sel.GetElementIds())
            {
                Element elem = doc.GetElement(id);
                if (elem is Floor) floors.Add(elem as Floor);
            }
            Trace.WriteLine("Selected floors: " + floors.Count);
            if (floors.Count == 0)
            {
                message = MyStrings.MessageSelectFloors;
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            Trace.WriteLine($"AreaTypeId: {areaTypeId}");
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);
            Trace.WriteLine($"Zero cover type id: {zeroCover.Id}");

            List<string> rebarTypes = SupportDocumentGetter.GetRebarTypes(doc);
            Trace.WriteLine("Rebar types: " + rebarTypes.Count);

            if (floors.Count == 0)
            {
                message = MyStrings.MessageNoCorrectFloors;
                return Result.Failed;
            }

            foreach (Floor fl in floors)
            {
                Parameter floorIsStructuralParam = fl.get_Parameter(BuiltInParameter.FLOOR_PARAM_IS_STRUCTURAL);
                if (floorIsStructuralParam != null)
                {
                    if (floorIsStructuralParam.AsInteger() != 1)
                    {
                        elements.Insert(fl);
                    }
                }
            }
            Trace.WriteLine("Structural floors: " + (floors.Count - elements.Size));
            if (elements.Size > 0)
            {
                message = MyStrings.MessageNoStructuralFloors;
                return Result.Failed;
            }

            string floorPath = System.IO.Path.Combine(App.localFolder, "floor.xml");
            RebarInfoFloor rif = null;
            XmlSerializer serializer = new XmlSerializer(typeof(RebarInfoFloor));

            if (System.IO.File.Exists(floorPath))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(floorPath))
                {
                    try
                    {
                        rif = (RebarInfoFloor)serializer.Deserialize(reader);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Failed deserialize, create new one: " + ex.Message);
                        rif = RebarInfoFloor.GetDefault(doc);
                    }
                    if (rif == null)
                    {
                        Trace.WriteLine("Deserialize error: " + floorPath);
                        throw new Exception("Serialize failed: " + floorPath);
                    }
                }
            }
            else
            {
                Trace.WriteLine("No xml file, create new one");
                rif = RebarInfoFloor.GetDefault(doc);
            }

            DialogWindowFloor form = new DialogWindowFloor(rif, rebarTypes);
            form.ShowDialog();
            if (form.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            List<string> rebarMessages = new List<string>();

            List<AreaReinforcement> createdAreas = new List<AreaReinforcement>();
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionFloorReinforcement);

                foreach (Floor floor in floors)
                {
                    Trace.WriteLine($"Current reinforcement floor: {floor.Id}");
                    List<AreaReinforcement> curCreatedAreas = RebarWorkerFloor.Generate(doc, floor, rif, areaTypeId, out List<string> curRebarMessages);
                    rebarMessages.AddRange(curRebarMessages);
                    createdAreas.AddRange(curCreatedAreas);
                }
                t.Commit();
            }

            if (rebarMessages.Count > 0)
            {
                message += string.Join(System.Environment.NewLine, rebarMessages);

                Trace.WriteLine("Errors: " + message);
                return Result.Failed;
            }

            Trace.WriteLine("Save settings to xml file: " + floorPath);
            if (File.Exists(floorPath))
            {
                try
                {
                    File.Delete(floorPath);
                }
                catch
                {
                    TaskDialog.Show("Warning", "Settings are not saved! Failed to delete file: " + floorPath);
                }
                Trace.WriteLine("File is deleted: " + floorPath);
            }
            if (!File.Exists(floorPath))
            {
                using (FileStream writer = new FileStream(floorPath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, form.rif);
                    Trace.WriteLine("New settings file is created: " + floorPath);
                }
            }

            sel.SetElementIds(createdAreas.Select(i => i.Id).ToList());

            string resultMessage = $"{MyStrings.ResultMessage1}: {createdAreas.Count}\n{MyStrings.ResultMessage2}!";
            Tools.Forms.BalloonTip.Show(MyStrings.Success, resultMessage);

            Trace.WriteLine("All done");
            return Result.Succeeded;
        }
    }
}
