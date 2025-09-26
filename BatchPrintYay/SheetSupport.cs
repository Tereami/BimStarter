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
using System.Diagnostics;
using System.Text.RegularExpressions;
#endregion

namespace BatchPrintYay
{
    public static class SheetSupport
    {
        public static string ClearIllegalCharacters(string line)
        {
            if (string.IsNullOrEmpty(line)) return string.Empty;
            string regexSearch = new string(System.IO.Path.GetInvalidFileNameChars())
                + new string(System.IO.Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string line2 = r.Replace(line, "");
            return line2;
        }


        public static string CheckTitleblocSizeCorrects(ViewSheet sheet, FamilyInstance titleBlock)
        {
            string message = "";

            Trace.WriteLine($"   Check Titleblock ID {titleBlock.Id}");

            double widthFeets = titleBlock.get_Parameter(BuiltInParameter.SHEET_WIDTH).AsDouble();
            double widthMm = Tools.Model.Units.ConvertFromInternalToMillimeters(widthFeets);
            widthMm = Math.Round(widthMm);
            double heightFeets = titleBlock.get_Parameter(BuiltInParameter.SHEET_HEIGHT).AsDouble();
            double heightMm = Tools.Model.Units.ConvertFromInternalToMillimeters(heightFeets);
            heightMm = Math.Round(heightMm);


            double widthMmCheck = -1, heigthMmCheck = -1;

            Parameter checkWidthParam = titleBlock.LookupParameter("Ширина");
            if (checkWidthParam == null)
                checkWidthParam = titleBlock.LookupParameter("Width");

            if (checkWidthParam != null)
            {
                double feetsWidthCheck = checkWidthParam.AsDouble();
                widthMmCheck = Tools.Model.Units.ConvertFromInternalToMillimeters(feetsWidthCheck);
                widthMmCheck = Math.Round(widthMmCheck);
                Trace.WriteLine(MyStrings.MessageParameterExists + " Ширина = " + widthMmCheck.ToString("F3"));
            }

            Parameter checkHeightParam = titleBlock.LookupParameter("Высота");
            if (checkHeightParam == null)
                checkHeightParam = titleBlock.LookupParameter("Height");

            if (checkHeightParam != null)
            {
                double feetsHeightCheck = checkHeightParam.AsDouble();
                heigthMmCheck = Tools.Model.Units.ConvertFromInternalToMillimeters(feetsHeightCheck);
                heigthMmCheck = Math.Round(heigthMmCheck);
                Trace.WriteLine(MyStrings.MessageParameterExists + " Высота = " + heigthMmCheck.ToString("F3"));
            }

            if (widthMmCheck == -1 || heigthMmCheck == -1)
            {
                Trace.WriteLine("    Titleblock is not from Weandrevit template, unable to check size");
                return string.Empty;
            }

            double epsilon = 2.0;
            bool widthEquals = DoubleEquals(widthMm, widthMmCheck, epsilon);
            bool heightEquals = DoubleEquals(heightMm, heigthMmCheck, epsilon);

            if (!widthEquals || !heightEquals)
            {
                Trace.WriteLine(MyStrings.MessageSheetSizeProblem + epsilon.ToString("F3") + " mm");
                message += "Sheet '" + sheet.SheetNumber + " : " + sheet.Name;
                message += "'. " + MyStrings.MessageUnableToGetTitleblockSize;
                if (widthMm != widthMmCheck)
                {
                    message += "'Ширина': " + widthMmCheck.ToString("F0") + "мм, '" +
                        MyStrings.ParameterBultinSheetWidth + "': " + widthMm.ToString("F0") + "мм.\n";
                }
                if (heightMm != heigthMmCheck)
                {
                    message += "'Высота': " + heigthMmCheck.ToString("F0") + "мм, '" +
                        MyStrings.ParameterBultinSheetHeight + "': " + heightMm.ToString("F0") + "мм.\n";
                }

                message += MyStrings.MessageCheckTitleblockSize;
            }
            else
            {
                Trace.WriteLine("    Titleblock size is correct");
            }

            return message;
        }

        private static bool DoubleEquals(double d1, double d2, double epsilon)
        {
            double c = Math.Abs(d1 - d2);
            if (c <= epsilon) return true;
            return false;
        }
    }
}
