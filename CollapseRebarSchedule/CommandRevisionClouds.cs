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
using System.Linq;
#endregion


namespace SchedulesTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandRevisionClouds : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            FormRevisionSettings form = new FormRevisionSettings();
            form.ShowDialog();

            if (form.DialogResult != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;


            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<ViewSheet> sheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(i => i.IsPlaceholder == false)
                .Where(i => !i.Name.StartsWith("("))
                .ToList();

            //заполняю ячейки "Кол.уч."
            if (Settings.UseCloudsCount || Settings.UseRevisionsOnThisSheet)
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Clouds");

                    foreach (ViewSheet sheet in sheets)
                    {
                        if (Settings.UseCloudsCount)
                        {
                            string[] cloudsCounts = GetCloudsCountOnSheet(sheet);

                            sheet.LookupParameter("Ш.КолвоУч1Текст").Set("");
                            sheet.LookupParameter("Ш.КолвоУч2Текст").Set("");
                            sheet.LookupParameter("Ш.КолвоУч3Текст").Set("");
                            sheet.LookupParameter("Ш.КолвоУч4Текст").Set("");

                            for (int i = 0; i < cloudsCounts.Length; i++)
                            {
                                string val = cloudsCounts[i];
                                string paramName = "Ш.КолвоУч" + (i + 1).ToString() + "Текст";
                                sheet.LookupParameter(paramName).Set(val);
                            }
                        }

                        if (Settings.UseRevisionsOnThisSheet)
                        {
                            List<ElementId> revisionsIds = sheet.GetAllRevisionIds().ToList();
                            if (revisionsIds.Count > 0)
                            {
                                string revisionsText = "Изм.";
                                for (int i = 0; i < revisionsIds.Count; i++)
                                {
                                    Revision curRev = doc.GetElement(revisionsIds[i]) as Revision;
                                    string revNumber = Settings.revisionParam.GetValueFromRevision(curRev);
                                    revisionsText = revisionsText + revNumber;
                                    if (i != revisionsIds.Count - 1) revisionsText = revisionsText + ",";
                                }
                                sheet.LookupParameter(Settings.SheetNoteParam).Set(revisionsText);
                            }
                        }
                    }
                    t.Commit();
                }
            }


            if (Settings.UseGroupingRevisions)
            {
                Dictionary<string, List<ViewSheet>> sheetsBase = new Dictionary<string, List<ViewSheet>>();


                foreach (ViewSheet sheet in sheets)
                {
                    string revisionDescription = "";
                    if (Settings.UseStandartRevisionDescription)
                    {
                        Revision curRev = doc.GetElement(sheet.GetCurrentRevision()) as Revision;
                        if (curRev == null) continue;
                        revisionDescription = curRev.Description;
                    }
                    else
                    {
                        revisionDescription = sheet.LookupParameter(Settings.SheetRevisionDescriptionParameter).AsString();
                    }
                    if (sheetsBase.ContainsKey(revisionDescription))
                    {
                        List<ViewSheet> curSheets = sheetsBase[revisionDescription];
                        curSheets.Add(sheet);
                        sheetsBase[revisionDescription] = curSheets;
                    }
                    else
                    {
                        List<ViewSheet> newSheets = new List<ViewSheet> { sheet };
                        sheetsBase.Add(revisionDescription, newSheets);
                    }
                }

                using (Transaction t2 = new Transaction(doc))
                {
                    t2.Start("Диапазоны");
                    foreach (var kvp in sheetsBase)
                    {
                        List<ViewSheet> curSheets = kvp.Value;
                        List<string> marks = curSheets.Select(i => i.LookupParameter(Settings.SheetNumberParameter).AsString()).ToList();
                        string range = Tools.Extensions.Strings.GetMarksRange(marks);

                        foreach (ViewSheet sheet in curSheets)
                        {
                            Parameter p = sheet.LookupParameter(Settings.SheetsForLastRevision);
                            if (p != null)
                            {
                                p.Set(range);
                            }
                            else
                            {
                                message += "Нет параметра" + Settings.SheetsForLastRevision;
                                return Result.Failed;
                            }
                        }
                    }
                    t2.Commit();
                }
            }


            return Result.Succeeded;
        }

        private string[] GetCloudsCountOnSheet(ViewSheet sheet)
        {
            Document doc = sheet.Document;
            List<RevisionCloud> clouds = new FilteredElementCollector(doc)
                .OfClass(typeof(RevisionCloud))
                .Cast<RevisionCloud>()
                .Where(i => i.OwnerViewId == sheet.Id)
                .ToList();

            List<Revision> allRevisionsOnSheet = sheet.GetAllRevisionIds()
                .Select(i => doc.GetElement(i))
                .Cast<Revision>()
                .OrderBy(i => Settings.revisionParam.GetValueFromRevision(i))
                .ToList();

            var allRevisionsGrouped = allRevisionsOnSheet.GroupBy(i => Settings.revisionParam.GetValueFromRevision(i)).ToDictionary(j => j.Key, j => j.ToList());

            Dictionary<string, List<Revision>> revsBase = new Dictionary<string, List<Revision>>();
            foreach (Revision rev in allRevisionsOnSheet)
            {
                string utvDlya = Settings.revisionParam.GetValueFromRevision(rev);
            }

            int revisionsCount = allRevisionsOnSheet.Count;
            List<Revision> lastRevisionsOnSheet = new List<Revision>();

            if (revisionsCount > 4)
            {
                lastRevisionsOnSheet = allRevisionsOnSheet.GetRange(revisionsCount - 4, 4);
            }
            else
            {
                lastRevisionsOnSheet = allRevisionsOnSheet;
            }

            int revsCount = lastRevisionsOnSheet.Count;
            string[] cloudsCounts = new string[revsCount];

            for (int i = 0; i < revsCount; i++)
            {
                Revision rev = lastRevisionsOnSheet[i];
                int curRevCloudsCount = clouds.Where(c => c.RevisionId == rev.Id).ToList().Count;
                string val = "";
                if (curRevCloudsCount == 0) val = "-";
                else val = curRevCloudsCount.ToString();

                cloudsCounts[i] = val;
            }
            return cloudsCounts;
        }
    }
}
