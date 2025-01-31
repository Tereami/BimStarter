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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public static List<ViewSheet> GetAllSheetsFromDocument(Document doc)
        {
            List<ViewSheet> sheets = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSheet))
                .WhereElementIsNotElementType()
                .Cast<ViewSheet>()
                .Where(i => !i.IsPlaceholder)
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
