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
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace SchedulesTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class CommandCreateTable : IExternalCommand
    {
        List<string> requiredNames = new List<string>()
        {
            "пецификаций",
            "chedule"
        };

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("SchedulesTable"));


            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            //получаю ведомость спецификаций
            ViewSchedule templateVs = null;
            ViewSheet firstSheet = null;
            View curView = doc.ActiveView;
            if (curView is ViewSchedule)
            {
                Trace.WriteLine("Current view is viewschedule");
                templateVs = curView as ViewSchedule;
                if (!IsScheduleNameCorrect(templateVs.Name))
                {
                    message = MyStrings.ErrorIncorrectSchedule;
                    Trace.WriteLine(message);
                    return Result.Failed;
                }

                List<ViewSheet> scheduleSheets = TableSupport.GetSheetsContainsScheduleInstances(doc, templateVs);
                Trace.WriteLine("Schedule instances count: " + scheduleSheets.Count);
                if (scheduleSheets.Count > 1)
                {
                    message = MyStrings.ErrorTwoSheets;
                    return Result.Failed;
                }
                firstSheet = scheduleSheets[0];
                if (firstSheet == null)
                {
                    message = MyStrings.ErrorNoScheduleOnFirstSheet;
                    return Result.Failed;
                }
            }
            else if (curView is ViewSheet)
            {
                Trace.WriteLine("Current view is Sheet");
                ElementId ssiId = null;
                Selection sel = uiDoc.Selection;
                List<ElementId> selIds = sel.GetElementIds().ToList();
                if (selIds.Count == 0)
                {
                    try
                    {
                        Trace.WriteLine("No selected elems, PickPoint");
                        Reference refer = sel.PickObject(ObjectType.Element,
                            new ScheduleSelectionFilter(requiredNames),
                            MyStrings.MsgSelectSchedule);
                        ssiId = refer.ElementId;
                    }
                    catch
                    {
                        Trace.WriteLine("PickPoint cancelled");
                        return Result.Cancelled;
                    }
                }
                else
                {
                    ssiId = selIds.First();
                }
                Trace.WriteLine($"Schedule instance id: {ssiId}");

                ScheduleSheetInstance selSse = doc.GetElement(ssiId) as ScheduleSheetInstance;
                if (selSse == null || !IsScheduleNameCorrect(selSse.Name))
                {
                    message = MyStrings.ErrorIncorrectSchedule;
                    Trace.WriteLine(message);
                    return Result.Failed;
                }
                firstSheet = curView as ViewSheet;
                templateVs = doc.GetElement(selSse.ScheduleId) as ViewSchedule;
            }
            else
            {
                message = MyStrings.MsgSelectOrOpen;
                Trace.WriteLine(message);
                return Result.Failed;
            }


            Tools.SettingsSaver.Saver<TableSettings> saver = new Tools.SettingsSaver.Saver<TableSettings>();
            TableSettings sets = saver.Activate("SchedulesTable");
            if (sets == null)
            {
                Trace.WriteLine("Failed to read config xml file");
                return Result.Cancelled;
            }
            sets.getLinkFiles = templateVs.Definition.IncludeLinkedFiles;
            FormTableSettings form = new FormTableSettings(sets);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled by user");
                return Result.Cancelled;
            }
            sets = form.sets;
            string sheetComplect = "";
            if (sets.useComplects)
            {
                Parameter sheetComplectParam = firstSheet.LookupParameter(sets.sheetComplectParamName);
                if (sheetComplectParam == null || !sheetComplectParam.HasValue)
                {
                    message = $"{MyStrings.ErrorNoSheetComplect} {sets.sheetComplectParamName} {firstSheet.Name}";
                    return Result.Failed;
                }
                sheetComplect = sheetComplectParam.AsString();
            }

            List<Document> docs = new List<Document> { doc };
            if (sets.getLinkFiles)
            {
                docs.AddRange(TableSupport.GetAllLinkedDocs(doc));
            }
            Trace.WriteLine("Documents: " + docs.Count);

            List<SheetScheduleInfo> infos = new List<SheetScheduleInfo>();
            foreach (Document curDoc in docs)
            {
                List<SheetScheduleInfo> curInfos = TableSupport.GetSchedulesInfo(curDoc, sets, sheetComplect);
                Trace.WriteLine("Schedules found in file " + curDoc.Title + ": " + curInfos.Count);
                infos.AddRange(curInfos);
            }
            //infos = infos.OrderBy(i => i.SheetNumberInt).ToList();
            infos = infos.OrderBy(i => i.SheetNumberString, new Tools.StringsNaturalSort.NumericComparer()).ToList();

            Trace.WriteLine("Schedules found: " + infos.Count);
            if (infos.Count == 0)
            {
                message = MyStrings.ErrorNoSchedulesFound;
                return Result.Failed;
            }

            TableData tData = templateVs.GetTableData();
            TableSectionData tsd = tData.GetSectionData(SectionType.Header);

            int lastRowNumber = tsd.LastRowNumber;
            if (lastRowNumber < 4)
            {
                message = MyStrings.Error3Row;
                return Result.Failed;
            }

            TableCellStyle cellStyle1 = tsd.GetTableCellStyle(lastRowNumber, 0);
            TableCellStyle cellStyle2 = tsd.GetTableCellStyle(lastRowNumber, 1);
            TableCellStyle cellStyle3 = tsd.GetTableCellStyle(lastRowNumber, 2);

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Schedules table");

                //удаляю лишние промежуточные строки
                while (tsd.LastRowNumber > 4)
                {
                    tsd.RemoveRow(3);
                }
                Trace.WriteLine("Rows deleted");

                //очищаю ячейки на всякий случай
                tsd.ClearCell(2, 0);
                tsd.ClearCell(2, 1);
                tsd.ClearCell(3, 0);
                tsd.ClearCell(3, 1);
                tsd.ClearCell(4, 0);
                tsd.ClearCell(4, 1);
                Trace.WriteLine("Cells cleaned");

                //добавляю пустые строки
                for (int i = 0; i < infos.Count - 3; i++)
                {
                    tsd.InsertRow(tsd.LastRowNumber);

                    tsd.SetRowHeight(tsd.LastRowNumber - 1, sets.rowHeight);
                    tsd.SetCellStyle(tsd.LastRowNumber - 1, 0, cellStyle1);
                    tsd.SetCellStyle(tsd.LastRowNumber - 1, 1, cellStyle2);
                    tsd.SetCellStyle(tsd.LastRowNumber - 1, 2, cellStyle3);
                    Trace.WriteLine("Create row number: " + i);
                }

                for (int i = 0; i < infos.Count; i++)
                {
                    int curRowNumber = i + 2;
                    SheetScheduleInfo info = infos[i];
                    tsd.SetCellText(curRowNumber, 0, info.SheetNumberString.ToString());
                    tsd.SetCellText(curRowNumber, 1, info.ScheduleName);
                    Trace.WriteLine("Write: " + info.SheetNumberString + " : " + info.ScheduleName + ", row №: " + curRowNumber);

                    //Увеличу высоту строки, если текст слишком длинный
                    double increaseRowHeight = 1.0;
                    for (int c = 1; c < info.RowsCount; c++)
                    {
                        increaseRowHeight *= sets.rowHeightCoeff;
                    }
                    double rowHeight = sets.rowHeight * increaseRowHeight;
                    Trace.WriteLine($"Row height coeff: {increaseRowHeight}, height: {rowHeight}");
                    tsd.SetRowHeight(curRowNumber, rowHeight);
                }
                t.Commit();
            }
            saver.Save(sets);
            Trace.WriteLine("Succeded");
            return Result.Succeeded;
        }

        private bool IsScheduleNameCorrect(string name)
        {
            foreach (string curname in requiredNames)
            {
                if (name.Contains(curname)) return true;
            }
            return false;
        }
    }
}