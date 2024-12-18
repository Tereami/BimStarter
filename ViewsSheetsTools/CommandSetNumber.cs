#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace ViewsSheetsTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandSetNumber : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            char uchar = (char)8234;

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger(nameof(CommandSetNumber)));


            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;

            List<ElementId> selids = sel.GetElementIds().ToList();
            if (selids.Count == 0)
            {
                if (doc.ActiveView is ViewSheet)
                {
                    Trace.WriteLine($"Set number for the opened sheet {doc.ActiveView.Name}");
                    selids.Add(doc.ActiveView.Id);
                }
                else
                {
                    message = MyStrings.ErrorNoSelectedElements;
                    Trace.WriteLine(message);
                    return Result.Failed;
                }
            }
            if (selids.Count > 1)
            {
                message = MyStrings.ErrorSelectOnlyOneElement;
                Trace.WriteLine(message);
                return Result.Failed;
            }

            Element selem = doc.GetElement(selids[0]);
            Trace.WriteLine($"Selected element: {selem.Name} id {selem.Id}");

            BuiltInParameter numberParam = BuiltInParameter.INVALID;
            string labelText = "";

            if (selem is Autodesk.Revit.DB.Architecture.Room)
            {
                Trace.WriteLine("Set Room number");
                numberParam = BuiltInParameter.ROOM_NUMBER;
                labelText = MyStrings.LabelRoomNumber;
            }
            else if (selem is Grid)
            {
                Trace.WriteLine("Set Grid number");
                numberParam = BuiltInParameter.DATUM_TEXT;
                labelText = MyStrings.LabelGridNumber;
            }
            else if (selem is Viewport)
            {
                Trace.WriteLine("Set Viewport number");
                numberParam = BuiltInParameter.VIEWPORT_DETAIL_NUMBER;
                labelText = MyStrings.LabelViewportNumber;
            }
            else if (selem is ViewSheet)
            {
                Trace.WriteLine("Set Viewsheet number");
                numberParam = BuiltInParameter.SHEET_NUMBER;
                labelText = MyStrings.LabelSheetNumber;
            }

            if (numberParam == BuiltInParameter.INVALID)
            {
                message = MyStrings.ErrorSupportedElements;
                Trace.WriteLine(message);
                return Result.Failed;
            }

            FormSetNumber form = new FormSetNumber(labelText);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }
            string number = form.numbervalue;
            Trace.WriteLine($"Value: {number}");


            Type selemType = selem.GetType();
            if (selem is Autodesk.Revit.DB.Architecture.Room)
            {
                selemType = typeof(SpatialElement);
            }
            List<Element> col = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(selemType)
                .ToElements()
                .ToList();
            HashSet<string> values = new HashSet<string>();
            foreach (Element e in col)
            {
                Parameter p = e.get_Parameter(numberParam);
                if (p is null) continue;
                if (!p.HasValue) continue;
                if (p.StorageType != StorageType.String) continue;
                string val = p.AsString();
                values.Add(val);
                Trace.WriteLine($"Already used value {val} in element {e.Name} : {e.Id}");
            }

            Trace.WriteLine($"Try to add invisible chars...");
            int counter = 0;
            while (values.Contains(number))
            {
                Trace.WriteLine($"Number {number} is already used! Add symbol. Iteration {counter}");
                number += uchar;
                counter++;
            }
            Trace.WriteLine($"Number {number} is available!");

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionName);

                Parameter finalNumberParam = selem.get_Parameter(numberParam);
                if (finalNumberParam is null)
                {
                    message = MyStrings.ErrorFailedToGetNumberParam;
                    Trace.WriteLine(message);
                    return Result.Failed;
                }
                if (finalNumberParam.IsReadOnly)
                {
                    message = MyStrings.ErrorParamReadonly;
                    Trace.WriteLine(message);
                    return Result.Failed;
                }
                Trace.WriteLine($"Try to set number...");
                finalNumberParam.Set(number);
                t.Commit();
            }
            Trace.WriteLine($"Success!");
            return Result.Succeeded;
        }
    }
}