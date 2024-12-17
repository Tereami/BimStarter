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
#region usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace Tools.LinksManager
{
    /// <summary>
    /// Класс-"оболочка", хранящий лист Revit, сведения формате листа и параметры печати листа
    /// </summary>
    public class MySheet : IComparable
    {
        public ViewSheet sheet;
        public PaperSize revitPaperSize;
        public System.Drawing.Printing.PaperSize windowsPaperSize;
        public IPrintSetting pSetting;
        public bool IsVertical;
        public long SheetId;
        public bool IsPrintable;

        //параметры для печати нескольких листов на одном
        public List<FamilyInstance> titleBlocks;
        public double widthMm;
        public double heigthMm;

        public bool ForceColored;
        public string PdfFileName;

        public int SheetNumberInt = -1;
        public int SheetSubNumber = 0;


        /// <summary>
        /// Инициализация класса, без объявления формата листа и параметров печати
        /// </summary>
        /// <param name="Sheet"></param>
        public MySheet(ViewSheet Sheet)
        {
            sheet = Sheet;
            SheetId = Sheet.GetElementId();
            SheetNumberInt = GetSheetNumberAsInt();

            ForceColored = false;
        }

        public MySheet(MySheet oldSheet)
        {
            sheet = oldSheet.sheet;
            revitPaperSize = oldSheet.revitPaperSize;
            windowsPaperSize = oldSheet.windowsPaperSize;
            pSetting = oldSheet.pSetting;
            IsVertical = oldSheet.IsVertical;
            SheetId = oldSheet.sheet.GetElementId();
            IsPrintable = oldSheet.IsPrintable;
            titleBlocks = oldSheet.titleBlocks;
            widthMm = oldSheet.widthMm;
            heigthMm = oldSheet.heigthMm;
            ForceColored = oldSheet.ForceColored;
            PdfFileName = oldSheet.PdfFileName;
            SheetNumberInt = oldSheet.SheetNumberInt;
        }

        public override string ToString()
        {
            string name = sheet.SheetNumber + " - " + sheet.Name;
            return name;
        }


        public bool FindTitleblocks(List<FamilyInstance> allTitleblocks, out string msg)
        {
            msg = string.Empty;
            titleBlocks = allTitleblocks
                    .Where(i => i.get_Parameter(BuiltInParameter.SHEET_NUMBER).AsString() == this.sheet.SheetNumber)
                    .ToList();

            Trace.WriteLine($" Titleblocks found: {titleBlocks.Count}");
            if (titleBlocks.Count == 0)
            {
                msg = "No titleblocks on sheet / Нет основных надписей на листе: " + sheet.Name;
                return false;
            }

            return true;
        }


        /// <summary>
        /// Формирует имя листа на базе строки-"конструктора", содержащего имена параметров,
        /// которые будут заменены на значения параметров из данного листа
        /// </summary>
        /// <param name="constructor">Строка конструктора. Имена параметров должны быть включены в треугольные скобки.</param>
        /// <returns>Сформированное имя листа</returns>
        public string NameByConstructor(string constructor)
        {
            string name = "";

            string prefix = constructor.Split('<').First();
            name = name + prefix;

            string[] sa = constructor.Split('<');
            for (int i = 0; i < sa.Length; i++)
            {
                string s = sa[i];
                if (!s.Contains(">")) continue;

                string paramName = s.Split('>').First();
                string separator = s.Split('>').Last();

                string val1 = this.GetParameterValueBySheetOrProject(sheet, paramName);
                string val = Tools.Extensions.Paths.ClearIllegalCharacters(val1);

                name = name + val;
                name = name + separator;
            }


            char[] arr = name.Where(c => (char.IsLetterOrDigit(c) ||
                             char.IsWhiteSpace(c) ||
                             c == '-' ||
                             c == '_' ||
                             c == '.')).ToArray();

            name = new string(arr);

            return name;
        }

        /// <summary>
        /// Получает значение параметра из листа и из "информации о проекте", по аналогии с "меткой" в семействе основной надписи.
        /// </summary>
        /// <param name="sheet">Элемент модели</param>
        /// <param name="paramName">Имя параметра</param>
        /// <returns></returns>
        private string GetParameterValueBySheetOrProject(Element sheet, string paramName)
        {
            string value = "";

            Parameter param = sheet.LookupParameter(paramName);
            if (param == null)
            {
                param = sheet.Document.ProjectInformation.LookupParameter(paramName);
            }
            if (param != null)
            {
                value = this.GetParameterValueAsString(param);
            }
            return value;
        }

        /// <summary>
        /// Получает значение параметра с любым типом данных, преобразованное в тип string
        /// </summary>
        /// <param name="param">Имя параметра</param>
        /// <returns>Значение параметра как string</returns>
        private string GetParameterValueAsString(Parameter param)
        {
            string val = "";
            switch (param.StorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Integer:
                    val = param.AsInteger().ToString();
                    break;
                case StorageType.Double:
                    double d = param.AsDouble();
#if R2017 || R2018 || R2019 || R2020
                    double d2 = UnitUtils.ConvertFromInternalUnits(d, DisplayUnitType.DUT_MILLIMETERS);
#else
                    double d2 = UnitUtils.ConvertFromInternalUnits(d, UnitTypeId.Millimeters);
#endif
                    val = Math.Round(d2).ToString("F3");
                    break;
                case StorageType.String:
                    val = param.AsString();
                    break;
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    val = param.AsElementId().IntegerValue.ToString();
#else
                    val = param.AsElementId().Value.ToString();
#endif
                    break;
                default:
                    break;
            }
            return val;
        }


        /// <summary>
        /// Попытаться преобразовать текстовый номер листа в число для правильной сортировки
        /// </summary>
        /// <returns></returns>
        public int GetSheetNumberAsInt()
        {
            string sheetNumberString = this.sheet.SheetNumber;

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
            catch
            {
            }
            return sheetNumber;
        }

        //для сортировки по номеру листа
        public int CompareTo(object obj)
        {
            MySheet ms = obj as MySheet;
            if (ms != null)
            {
                int thisSheetNumber = this.SheetNumberInt;
                int compareSheetNumber = ms.SheetNumberInt;
                int resuls = thisSheetNumber.CompareTo(compareSheetNumber);
                return resuls;
            }
            else
            {
                throw new Exception("Unable to equals the objects");
            }
        }

        public void CheckIsColored(string forceColoredParamName)
        {
            Trace.WriteLine($"Check parameter {forceColoredParamName} for sheet {this.sheet.Name}");
            Parameter isForceColoredParam = sheet.LookupParameter(forceColoredParamName);
            if (isForceColoredParam != null)
            {
                if (isForceColoredParam.HasValue)
                {
                    if (isForceColoredParam.AsInteger() == 1)
                    {
                        ForceColored = true;
                        Trace.WriteLine($"{forceColoredParamName} equals 1, force colored");
                    }
                    else
                    {
                        Trace.WriteLine($"{forceColoredParamName} equals 0, not force colored");
                    }
                }
                else
                {
                    Trace.WriteLine($"{forceColoredParamName} has no value, not force colored");
                }
            }
            else
            {
                Trace.WriteLine($"...{forceColoredParamName} is null, not force colored");
            }
        }
    }
}
