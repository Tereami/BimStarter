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
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
#endregion


namespace SchedulesTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandCollapseRebarSchedule : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("CollapseRebarSchedule"));
            Debug.WriteLine($"{nameof(CommandCollapseRebarSchedule)} start");

            Tools.SettingsSaver.Saver<CollapseScheduleSettings> saver = new Tools.SettingsSaver.Saver<CollapseScheduleSettings>();
            CollapseScheduleSettings sets = saver.Activate("CollapseRebarSchedule");
            if (sets == null)
            {
                Trace.WriteLine("Failed to read config xml file");
                return Result.Cancelled;
            }
            FormCollapseSettings form = new FormCollapseSettings(sets);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return Result.Cancelled;
            sets = form.NewSets;
            Debug.WriteLine($"Settings: {sets.ToString()}");

            Document doc = commandData.Application.ActiveUIDocument.Document;
            ScheduleDefinition sdef = null;
            ViewSchedule vs = commandData.Application.ActiveUIDocument.ActiveView as ViewSchedule;
            if (vs == null)
            {
                Debug.WriteLine($"Active view is not ViewSchedule");
                Selection sel = commandData.Application.ActiveUIDocument.Selection;
                if (sel.GetElementIds().Count == 0)
                {
                    message = MyStrings.ErrorNoSelectedSchedule;
                    Debug.WriteLine(message);
                    return Result.Failed;
                }
                ScheduleSheetInstance ssi = doc.GetElement(sel.GetElementIds().First()) as ScheduleSheetInstance;
                if (ssi == null)
                {
                    message = MyStrings.ErrorNoSelectedSchedule;
                    Debug.WriteLine(message);
                    return Result.Failed;
                }
                vs = doc.GetElement(ssi.ScheduleId) as ViewSchedule;
                Debug.WriteLine($"Selected schedule {vs.Name}, id: {vs.Id}");
            }
            Debug.WriteLine($"Schedule ID: {vs.Id}");
            sdef = vs.Definition;

            int firstWeightCell = 0;
            int startHiddenFields = 0;
            int borderCell = 9999;

            //определяю первую и последнюю ячейку с массой
            Debug.WriteLine($"Try to define first and last processed columns");
            bool flagEndCellFound = false;
            for (int i = 0; i < sdef.GetFieldCount(); i++)
            {
                ScheduleField sfield = sdef.GetField(i);
                string cellName = sfield.GetName();
                Debug.WriteLine($"i = {i}, field name: {cellName}");
                if (firstWeightCell == 0)
                {
                    if (char.IsNumber(cellName[0]))
                    {
                        firstWeightCell = i;
                        Debug.WriteLine($"First char is a number. Start field is found, i={i}");
                    }
                    else
                    {
                        if (sfield.IsHidden)
                        {
                            startHiddenFields++;
                            Debug.WriteLine($"Field if hidden");
                        }
                    }
                }
                if (cellName.StartsWith(sets.LastColumnSign))
                {
                    Debug.WriteLine($"Field name starts with {sets.LastColumnSign}. i={i}");
                    borderCell = i;
                    flagEndCellFound = true;
                    break;
                }
            }

            if (!flagEndCellFound)
            {
                message = MyStrings.ErrorNoEndColumn;
                Debug.WriteLine($"Incorrect schedule! Failed");
                return Result.Failed;
            }

            Debug.WriteLine($"Transaction start");
            int allFields = 0, hiddenFields = 0, openedFields = 0;
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionName);

                Dictionary<int, bool> fieldsState = new Dictionary<int, bool>();
                Debug.WriteLine($"Show all fields...");
                for (int i = firstWeightCell; i < borderCell; i++)
                {
                    if (!sdef.IsValidFieldIndex(i))
                    {
                        throw new System.Exception($"Field index {i} is not valid.");
                    }
                    ScheduleField sfield = sdef.GetField(i);

                    fieldsState.Add(i, sfield.IsHidden);
                    sfield.IsHidden = false;
                    allFields++;
                }
                Debug.WriteLine($"Show all fields completed");
                doc.Regenerate();

                Debug.WriteLine($"Start Table data...");
                TableData tdata = vs.GetTableData();

                TableSectionData tsd = tdata.GetSectionData(SectionType.Body);
                int firstRownumber = tsd.FirstRowNumber + sets.HeaderRowsCount;
                int lastRowNumber = tsd.LastRowNumber;
                int rowsCount = lastRowNumber - firstRownumber + 1;
                Debug.WriteLine($"First row: {firstRownumber}, last row {lastRowNumber}, count {rowsCount}");

                for (int i = firstWeightCell; i < borderCell; i++)
                {
                    ScheduleField sfield = sdef.GetField(i);
                    Debug.WriteLine($"...");
                    Debug.WriteLine($"Field index i = {i}, field name:\t{sfield.GetName()}");

                    List<string> values = new List<string>();
                    for (int j = firstRownumber; j <= lastRowNumber; j++)
                    {
                        string cellText = tsd.GetCellText(j, i - startHiddenFields);
                        Debug.WriteLine($"Row number j = {j}, cell text: {cellText}");
                        values.Add(cellText);
                    }

                    bool checkOnlyTextAndZeros = OnlyTextAndZeros(values);
                    if (checkOnlyTextAndZeros)
                    {
                        if (fieldsState[i] == false)
                            hiddenFields++;

                        sfield.IsHidden = true;
                        Debug.WriteLine($"Field made hidden");
                    }
                    else
                    {
                        if (fieldsState[i] == true)
                            openedFields++;
                        Debug.WriteLine($"Field remains shown");
                    }
                }
                t.Commit();
            }

            string msg = "";
            if (hiddenFields == 0 && openedFields == 0)
            {
                msg = MyStrings.ResultNoFields;
                Debug.WriteLine(MyStrings.ResultNoFields);
            }
            else
            {
                List<string> messages = new List<string>
                {
                    MyStrings.ResultMessage,
                    $"{MyStrings.ResultMessageHidden}: {hiddenFields}",
                    $"{MyStrings.ResultMessageOpened}: {openedFields}",
                };
                msg = string.Join(System.Environment.NewLine, messages);
            }
            Tools.Forms.BalloonTip.Show(MyStrings.Result, msg);
            Debug.WriteLine(msg);
            saver.Save(sets);
            return Result.Succeeded;
        }

        private bool OnlyTextAndZeros(List<string> values)
        {
            bool haveZeros = false;
            bool haveNumber = false;
            double val = -1;
            foreach (string s in values)
            {
                val = -1;
                if (string.IsNullOrEmpty(s))
                {
                    Debug.WriteLine($"Is null or empty");
                    continue;
                }
                string noComma = s.Replace(',', '.');
                bool isNumber = double.TryParse(noComma, NumberStyles.Any, CultureInfo.InvariantCulture, out val);
                if (!isNumber)
                {
                    Debug.WriteLine($"{noComma} - text, not a number");
                    continue;
                }
                if (val > 0)
                {
                    Debug.WriteLine($"{noComma} - number greater than zero");
                    haveNumber = true;
                    continue;
                }
                else
                {
                    Debug.WriteLine($"{noComma} - zero");
                    haveZeros = true;
                }
            }
            if (haveZeros && !haveNumber)
            {
                Debug.WriteLine($"Have zeros = true, haveNumber = false. checkOnlyTextAndZeros = true");
                return true;
            }
            Debug.WriteLine($"checkOnlyTextAndZeros = false");
            return false;
        }
    }
}
