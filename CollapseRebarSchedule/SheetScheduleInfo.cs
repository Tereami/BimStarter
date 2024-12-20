using Autodesk.Revit.DB;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SchedulesTools
{
    public class SheetScheduleInfo
    {
        public string SheetNumberString;
        public int SheetNumberInt;
        public string ScheduleName;
        public int RowsCount = 1;

        public SheetScheduleInfo(ScheduleSheetInstance ssi, ViewSheet sheet, TableSettings sets)
        {
            Trace.WriteLine($"Start creating new info, schedule {ssi.Name} from sheet id {sheet.Id}");
            string sheetNumberStringRaw = "";
            if (sets.useStandardSheetNumber)
            {
                sheetNumberStringRaw = sheet.get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString();
            }
            else
            {
                Parameter sheetNumberParam = sheet.LookupParameter(sets.altSheetNumberParam);
                if (sheetNumberParam == null || !sheetNumberParam.HasValue)
                {
                    string msg = $"Failed to get {sets.altSheetNumberParam} from sheet id {sheet.Id}";
                    Trace.WriteLine(msg);
                    throw new Exception(msg);
                }
                else
                {
                    sheetNumberStringRaw = sheetNumberParam.AsString();
                }
            }
            if (sheetNumberStringRaw == "")
            {
                throw new Exception($"Failed to get sheet number for sheet: {sheet.Name}");
            }
            SheetNumberString = Regex.Replace(sheetNumberStringRaw, @"[^\d]+", "");
            SheetNumberInt = Convert.ToInt32(SheetNumberString);

            Regex regex = new Regex(@"\*(?<name>.+)\*");
            Match match = regex.Match(ssi.Name);
            string scheduleNameRaw = match.Groups["name"].Value;

            if (scheduleNameRaw.Contains(sets.newLineSymbol))
            {
                RowsCount = scheduleNameRaw.Split(sets.newLineSymbol[0]).Length;
                scheduleNameRaw = scheduleNameRaw.Replace(sets.newLineSymbol, Environment.NewLine);
            }
            else if (scheduleNameRaw.Length > sets.maxCharsInOneLine)
            {
                double rowsCountRaw = scheduleNameRaw.Length / (double)sets.maxCharsInOneLine;
                RowsCount = (int)Math.Ceiling(rowsCountRaw);
            }

            ScheduleName = scheduleNameRaw;

            Trace.WriteLine($"Completed, sheet number: {SheetNumberString} from sheet id {sheet.Id}");
        }
    }
}