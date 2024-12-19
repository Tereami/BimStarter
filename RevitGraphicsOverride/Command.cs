#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion


namespace RevitGraphicsOverride
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("GraphicsOverride"));
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Dictionary<string, List<OverridenResult>> resultsOnSheets = new Dictionary<string, List<OverridenResult>>();

            Form1 form1 = new Form1();
            form1.ShowDialog();
            if (form1.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            if (form1.searchAllProject == false)
            {
                View curView = uidoc.ActiveView;
                string title = curView.Name;
                Trace.WriteLine("Search only current view: " + title);

                string sheetNumber = curView.get_Parameter(BuiltInParameter.VIEWER_SHEET_NUMBER).AsString();
                if (sheetNumber != "---")
                {
                    string sheetName = curView.get_Parameter(BuiltInParameter.VIEWPORT_SHEET_NAME).AsString();
                    title = MyStrings.Sheet + ": " + sheetNumber + " - " + sheetName + "; " + curView.Name;
                }
                Trace.WriteLine("Sheet number: " + sheetNumber);
                List<OverridenResult> overridenElems = SupportGraphics.getOverridenElemsOnView(doc, curView);
                Trace.WriteLine("Overriden elems found: " + overridenElems.Count);
                resultsOnSheets.Add(title, overridenElems);
            }
            else
            {
                Trace.WriteLine("Search entire project");
                List<ViewSheet> sheets = new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewSheet))
                    .Cast<ViewSheet>()
                    .OrderBy(i => i.SheetNumber)
                    .ToList();
                Trace.WriteLine("Sheets: " + sheets.Count);

                foreach (ViewSheet sheet in sheets)
                {
                    Trace.WriteLine("Current sheet: " + sheet.Name);
                    List<View> viewsOnSheet = sheet.GetAllPlacedViews().Select(i => doc.GetElement(i) as View).ToList();
                    Trace.WriteLine("Views on sheet: " + viewsOnSheet.Count);

                    foreach (View curView in viewsOnSheet)
                    {
                        Trace.WriteLine("View: " + curView.Name);
                        List<OverridenResult> overridenElemsCurSheet = SupportGraphics.getOverridenElemsOnView(doc, curView);
                        if (overridenElemsCurSheet.Count == 0) continue;
                        string title = sheet.Title + "; " + curView.Name;
                        resultsOnSheets.Add(title, overridenElemsCurSheet);
                    }
                }
            }
            Trace.WriteLine("Views with overrides: " + resultsOnSheets.Count);
            if (resultsOnSheets.Count == 0)
            {
                TaskDialog.Show(MyStrings.Report, MyStrings.ErrorNoElements);
                return Result.Cancelled;
            }

            if (form1.selectElems)
            {
                Trace.WriteLine("Select elements");
                List<ElementId> ids = new List<ElementId>();
                foreach (var kvp in resultsOnSheets)
                {
                    List<OverridenResult> ress = kvp.Value;
                    List<ElementId> curIds = ress.Select(i => i.Elem.Id).ToList();
                    ids.AddRange(curIds);
                }

                uidoc.Selection.SetElementIds(ids);
            }

            if (form1.showResults)
            {
                Trace.WriteLine("Show window");
                Form2 form2 = new Form2(resultsOnSheets);
                form2.ShowDialog();

                if (form2.DialogResult == System.Windows.Forms.DialogResult.Yes)
                {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    ElementId idToSelect = new ElementId((int)form2.idToSelect); //в версиях до 2024 можно приводить long к int
                    ElementId idToOpen = new ElementId((int)form2.idViewToOpen);
#else
                    ElementId idToSelect = new ElementId(form2.idToSelect);
                    ElementId idToOpen = new ElementId(form2.idViewToOpen);
#endif
                    View overrideView = doc.GetElement(idToOpen) as View;
                    Trace.WriteLine($"Select by double click, id {idToSelect}, view: {overrideView.Name}");
                    uidoc.ActiveView = overrideView;
                    uidoc.Selection.SetElementIds(new List<ElementId> { idToSelect });
                }
            }
            Trace.WriteLine("Finished");
            return Result.Succeeded;
        }
    }
}
