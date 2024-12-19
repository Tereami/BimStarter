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
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace RevitViewFilters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    class CommandBatchDelete : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("BatchDeleteFilters"));
            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<ParameterFilterElement> filters = new FilteredElementCollector(doc)
                .OfClass(typeof(ParameterFilterElement))
                .Cast<ParameterFilterElement>()
                .ToList();
            Trace.WriteLine("Filters found: " + filters.Count);

            List<string> filterNames = filters.Select(x => x.Name).ToList();
            filterNames.Sort();

            FormBatchDelete form = new FormBatchDelete();
            form.Items = filterNames;

            form.ShowDialog();

            if (form.DialogResult != DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            List<string> deleteFilterNames = form.CheckedItems;
            Trace.WriteLine("Filters name for deleting: " + deleteFilterNames.Count);

            List<ParameterFilterElement> filtersToDelete = filters
                .Where(i => deleteFilterNames.Contains(i.Name))
                .ToList();
            Trace.WriteLine("filters found: " + filtersToDelete.Count);

            List<ElementId> ids = filtersToDelete.Select(i => i.Id).ToList();

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionDeleteFilters + ": " + deleteFilterNames.Count.ToString());
                doc.Delete(ids);
                t.Commit();
            }

            form.Dispose();

            Trace.WriteLine("Deleted filters: " + ids.Count);
            Autodesk.Revit.UI.TaskDialog.Show(MyStrings.TransactionDeleteFilters, MyStrings.MessageDeleteFiltersSuccess + ids.Count);

            return Result.Succeeded;
        }
    }
}
