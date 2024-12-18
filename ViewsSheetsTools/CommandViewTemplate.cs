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
using Tools.LinksManager;

#endregion


namespace ViewsSheetsTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandViewTemplate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger(nameof(CommandViewTemplate)));

            var app = commandData.Application.Application;
            Document mainDoc = commandData.Application.ActiveUIDocument.Document;

            DocumentSet docSet = app.Documents;
            List<MyRevitLinkDocument> allDocs = new List<MyRevitLinkDocument>();
            foreach (Document doc in app.Documents)
            {
                if (doc.Title == mainDoc.Title) continue;
                if (doc.IsValidObject)
                {
                    MyRevitLinkDocument myDoc = new MyRevitLinkDocument(doc, false);
                    allDocs.Add(myDoc);
                }
            }
            Trace.Write("Docs count: " + allDocs.Count);
            if (allDocs.Count == 0)
            {
                message = "Нет открытых документов для копирования!";
                return Result.Failed;
            }

            FormSelectLinks formLinks = new FormSelectLinks(allDocs, false);
            if (formLinks.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            MyRevitLinkDocument selectedMyDoc = formLinks.selectedLinks[0];
            Trace.WriteLine($"Selected doc: {selectedMyDoc.Name}");

            Document selectedDoc = selectedMyDoc.Doc;
            List<View> templates = new FilteredElementCollector(selectedDoc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.IsTemplate == true)
                .ToList();
            Trace.WriteLine("Templates found: " + templates.Count);
            List<MyView> myViews = templates
                .OrderBy(i => i.Name)
                .Select(i => new MyView(i))
                .ToList();

            FormSelectTemplates form2 = new FormSelectTemplates(myViews);
            form2.ShowDialog();
            if (form2.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }

            List<ElementId> templateIds = form2.selectedTemplates.Select(i => i.view.Id).ToList();
            Trace.WriteLine("Selected templates: " + templateIds.Count);
            CopyPasteOptions cpo = new CopyPasteOptions();
            cpo.SetDuplicateTypeNamesHandler(new DuplicateNamesHandler());

            using (Transaction t = new Transaction(mainDoc))
            {
                t.Start("Копирование шаблонов видов");

                ElementTransformUtils.CopyElements(selectedDoc, templateIds, mainDoc, Transform.Identity, cpo);

                t.Commit();
            }

            string msg = "Успешно скопировано шаблонов: " + templateIds.Count.ToString();
            if (DuplicateTypes.types.Count > 0) msg += "\nПродублированы: " + DuplicateTypes.ReturnAsString();

            Trace.WriteLine(msg);
            TaskDialog.Show("Отчет", msg);

            return Result.Succeeded;
        }
    }
}
