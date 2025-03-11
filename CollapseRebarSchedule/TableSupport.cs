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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.Forms;
#endregion

namespace SchedulesTools
{
    public static class TableSupport
    {
        public static List<SheetScheduleInfo> GetSchedulesInfo(Document doc, TableSettings sets, string complectNumber = "")
        {
            Trace.WriteLine("Get schedules in document: " + doc.Title);
            List<SheetScheduleInfo> infos = new List<SheetScheduleInfo>();
            List<ScheduleSheetInstance> scheduleInstances = new FilteredElementCollector(doc)
                .OfClass(typeof(ScheduleSheetInstance))
                .Cast<ScheduleSheetInstance>()
                .Where(i => doc.GetElement(i.ScheduleId).Name.EndsWith("*"))
                .ToList();
            Trace.WriteLine("Schedule instances found: " + scheduleInstances.Count);

            Dictionary<ElementId, string> splitSuffixes = GetSplitSuffixes(doc, scheduleInstances);

            List<ViewSheet> sheets = GetAllSheetsFromDocument(doc);
            foreach (ViewSheet sheet in sheets)
            {
                Trace.WriteLine("Check sheet: " + sheet.Name);
                if (sets.useComplects)
                {
                    Parameter complectParam = sheet.LookupParameter(sets.sheetComplectParamName);
                    if (complectParam == null || !complectParam.HasValue)
                    {
                        Trace.WriteLine("No complect parameter");
                        continue;
                    }
                    string curComplectValue = complectParam.AsString();
                    if (curComplectValue != complectNumber)
                    {
                        Trace.WriteLine("Skip, sheet complect = " + curComplectValue + " is not " + complectNumber);
                        continue;
                    }
                }

                List<ScheduleSheetInstance> curSsis = scheduleInstances
                    .Where(i => i.OwnerViewId == sheet.Id)
                    .GroupBy(i => i.ScheduleId).Select(j => j.First())
                    .ToList();
                Trace.WriteLine("Schedule instances on sheet: " + curSsis.Count);
                foreach (ScheduleSheetInstance ssi in curSsis)
                {
                    Trace.WriteLine($"Schedule instance id: {ssi.Id}");
                    SheetScheduleInfo info = new SheetScheduleInfo(ssi, sheet, sets);
                    if (splitSuffixes.ContainsKey(ssi.Id))
                        info.ScheduleName += " " + splitSuffixes[ssi.Id];
                    infos.Add(info);
                }
            }

            return infos;
        }

        public static List<ViewSheet> GetSheetsContainsScheduleInstances(Document doc, ViewSchedule vs)
        {
            ElementId scheduleId = vs.Id;
            List<ScheduleSheetInstance> ssis = new FilteredElementCollector(doc)
                .OfClass(typeof(ScheduleSheetInstance))
                .Cast<ScheduleSheetInstance>()
                .Where(i => i.ScheduleId == scheduleId)
                .ToList();

            if (ssis.Count == 0) return null;

            List<ViewSheet> sheets = new List<ViewSheet>();

            foreach (ScheduleSheetInstance ssi in ssis)
            {
                ElementId sheetId = ssi.OwnerViewId;
                ViewSheet sheet = doc.GetElement(sheetId) as ViewSheet;
                Trace.WriteLine(vs.Name + " is at sheet " + sheet.Name);
                sheets.Add(sheet);
            }

            return sheets;
        }

        private static Dictionary<ElementId, string> GetSplitSuffixes(Document doc, List<ScheduleSheetInstance> scheduleInstances)
        {
            Dictionary<ElementId, string> splitSuffixes = new Dictionary<ElementId, string>();
            Dictionary<ElementId, List<ScheduleSheetInstance>> groupedByScheduleId =
                scheduleInstances.GroupBy(i => i.ScheduleId)
                .ToDictionary(i => i.Key, i => i.ToList());
            foreach (var group in groupedByScheduleId)
            {
                int splitCount = group.Value.Count;
                if (splitCount == 1) continue;


                List<List<ScheduleSheetInstance>> groupedBySheets = group.Value
                    .GroupBy(i => GetSheetNumberAsInt(i, doc))
                    .Select(i => Tuple.Create<int, List<ScheduleSheetInstance>>(i.Key, i.ToList()))
                    .OrderBy(i => i.Item1)
                    .Select(i => i.Item2)
                    .ToList();

                if (groupedBySheets.Count == 1) continue;

                foreach (var ssi in groupedBySheets.First())
                {
                    splitSuffixes.Add(ssi.Id, MyStrings.ScheduleSuffixStart);
                }

                foreach (ScheduleSheetInstance ssi in groupedBySheets.Last())
                {
                    splitSuffixes.Add(ssi.Id, MyStrings.ScheduleSuffixEnd);
                }

                if (groupedBySheets.Count == 2) continue;

                for (int i = 1; i < groupedBySheets.Count - 1; i++)
                {
                    foreach (ScheduleSheetInstance ssi in groupedBySheets[i])
                    {
                        splitSuffixes.Add(ssi.Id, MyStrings.ScheduleSuffixContinue);
                    }
                }
            }
            return splitSuffixes;
        }

        private static int GetSheetNumberAsInt(ScheduleSheetInstance ssi, Document doc)
        {
            ViewSheet sheet = doc.GetElement(ssi.OwnerViewId) as ViewSheet;

            string sheetNumberString = sheet.SheetNumber;

            if (sheetNumberString.Contains("-"))
            {
                sheetNumberString = sheetNumberString.Split('-').Last();
            }
            if (sheetNumberString.Contains("_"))
            {
                sheetNumberString = sheetNumberString.Split('_').Last();
            }
            int sheetNumber = 0;
            try
            {
                sheetNumber = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(sheetNumberString, @"[^\d]+", ""));
            }
            catch { }
            return sheet.Id.GetValue();
        }

        public static List<ViewSheet> GetAllSheetsFromDocument(Document doc)
        {
            List<ViewSheet> sheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .WhereElementIsNotElementType()
                .Cast<ViewSheet>()
                .Where(i => !i.IsPlaceholder)
                .OrderBy(i => i.SheetNumber)
                .ToList();
            Trace.WriteLine("Sheets found: " + sheets.Count + " in " + doc.Title);
            return sheets;
        }


        public static List<Document> GetAllLinkedDocs(Document doc)
        {
            List<Document> docs = new List<Document>();

            List<RevitLinkInstance> links = new FilteredElementCollector(doc)
                .OfClass(typeof(RevitLinkInstance))
                .Cast<RevitLinkInstance>()
                .ToList();

            foreach (RevitLinkInstance rli in links)
            {
                Trace.WriteLine("Check rvt link: " + rli.Name);
                Document linkDoc = rli.GetLinkDocument();
                if (linkDoc == null) continue;
                if (docs.Contains(linkDoc)) continue;

                docs.Add(linkDoc);
            }
            Trace.WriteLine("Link docs found: " + docs.Count);
            return docs;
        }
    }
}
