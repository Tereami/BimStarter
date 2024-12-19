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
Zuev Aleksandr, 2020, all rights reserved.*/
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
    class CommandCreateAreaRebar : IExternalCommand
    {
        public static List<AreaReinforcement> allAreaReinforcements { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("WallAreaRebar"));
            Trace.WriteLine("Wall reinforcement start");

            App.ActivateConfigFolder();

            Document doc = commandData.Application.ActiveUIDocument.Document;

            allAreaReinforcements = SupportDocumentGetter.GetRebarAreas(doc);

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Wall> walls = new List<Wall>();


            foreach (ElementId id in sel.GetElementIds())
            {
                Element elem = doc.GetElement(id);

                if (elem is Wall) walls.Add(elem as Wall);
            }
            if (walls.Count == 0)
            {
                message = MyStrings.MessageSelectWalls;
                return Result.Failed;
            }

            Trace.WriteLine("Selected walls count: " + walls.Count.ToString());
            foreach (Wall w in walls)
            {
                Parameter isStructural = w.get_Parameter(BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT);
                if (isStructural == null) continue;
                if (isStructural.AsInteger() != 1)
                {
                    elements.Insert(w);
                }
            }
            if (elements.Size > 0)
            {
                message = MyStrings.MessageNoStructuralWalls;
                Trace.WriteLine("Non-structural walls were found");
                return Result.Failed;
            }

            ElementId areaTypeId = SupportDocumentGetter.GetDefaultArea(doc).Id;
            RebarCoverType zeroCover = SupportDocumentGetter.GetRebarCoverType(doc, 0);
            List<string> rebarTypes = SupportDocumentGetter.GetRebarTypes(doc);
            List<string> rebarTypes2 = rebarTypes.ToList();
            bool wallsHaveRebarInfo = SupportDocumentGetter.CheckWallsHaveRebarInfo(walls);


            RebarInfoWall riw = new RebarInfoWall(); //RebarInfoWall.GetDefault(doc);
            riw.SetDefaultUnificateLengths();
            string wallPath = System.IO.Path.Combine(App.localFolder, "wall.xml");
            Trace.WriteLine("Try to deserialize xml: " + wallPath);
            XmlSerializer serializer = new XmlSerializer(typeof(RebarInfoWall));

            if (System.IO.File.Exists(wallPath))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(wallPath))
                {
                    try
                    {
                        riw.lengthsUnification.Clear();
                        riw = (RebarInfoWall)serializer.Deserialize(reader);
                    }
                    catch
                    {
                        Trace.WriteLine("Deserialize fauled!");
                    }
                }
            }

            if (wallsHaveRebarInfo)
            {
                Trace.WriteLine("DialogWindow for auto-reinforcement");
                DialogWindowWallAuto dialogWallAuto = new DialogWindowWallAuto(riw);
                dialogWallAuto.ShowDialog();
                if (dialogWallAuto.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
                riw = dialogWallAuto.rebarInfo;
                Trace.WriteLine("RebarInfo created");
            }
            else
            {
                Trace.WriteLine("Dialog window for manual-reinforcement");
                DialogWindowWall form = new DialogWindowWall(riw, rebarTypes, rebarTypes2);
                form.ShowDialog();
                if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
            }

            List<string> errorMessages = new List<string>();
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionWallReinforcement);
                Trace.WriteLine("Start transaction");

                foreach (Wall wall in walls)
                {
                    if (wallsHaveRebarInfo)
                    {
                        Trace.WriteLine("Get rebar info from wall");
                        RebarInfoWall infoFromWall = new RebarInfoWall(doc, wall);
                        riw.rebarCover = infoFromWall.rebarCover;
                        riw.verticalFreeLength = infoFromWall.verticalFreeLength;
                        riw.verticalRebarTypeName = infoFromWall.verticalRebarTypeName;
                        riw.verticalRebarInterval = infoFromWall.verticalRebarInterval;
                        riw.horizontalRebarTypeName = infoFromWall.horizontalRebarTypeName;
                        riw.horizontalRebarInterval = infoFromWall.horizontalRebarInterval;
                        riw.horizontalHeightIncreaseIntervalBottom = infoFromWall.horizontalHeightIncreaseIntervalBottom;
                        riw.horizontalHeightIncreaseIntervalTop = infoFromWall.horizontalHeightIncreaseIntervalTop;
                        riw.horizontalAddInterval = true;
                    }
                    Trace.WriteLine("Start wall reinforcement");
                    List<string> curErrorMessages = RebarWorkerWall.GenerateRebar(doc, wall, riw, zeroCover, areaTypeId);
                    errorMessages.AddRange(curErrorMessages);
                }
                t.Commit();
                Trace.WriteLine("Finish transaction");
            }

            if (errorMessages.Count > 0)
            {
                string errorMsg = string.Join(Environment.NewLine, errorMessages);
                Trace.WriteLine(errorMsg);
                TaskDialog.Show("Error", errorMsg);
                return Result.Failed;
            }

            Trace.Write("Delete xml settings file and rewrite: " + wallPath);
            if (File.Exists(wallPath))
            {
                try
                {
                    File.Delete(wallPath);
                }
                catch
                {
                    TaskDialog.Show("Warning", "Settings are not saved! Failed to delete file: " + wallPath);
                }
            }
            if (!File.Exists(wallPath))
            {
                using (FileStream writer = new FileStream(wallPath, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(writer, riw);
                }
                Trace.WriteLine("... xml success!");
            }
            return Result.Succeeded;
        }
    }
}
