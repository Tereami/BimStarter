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
#endregion

namespace Tools.Model
{
    public static class Units
    {
        static public string RealString(double a)
        {
            return a.ToString("0.##");
        }


        public static string InchesToStringMillimeters(this double inches)
        {
            double mm = inches.InchesToMillimeters();
            string text = mm.ToString("0.#");
            return text;
        }

        public static double ParseToInches(this string millimeters)
        {
            if (double.TryParse(millimeters, out double result))
                return result / 304.8;
            else
                return 0;
        }

        public static double InchesToMillimeters(this double inches)
        {
            double mm = Math.Round(inches * 304.8, 3);
            return mm;
        }

        public static double MillimetersToInches(this double millimeters)
        {
            double inches = millimeters / 304.8;
            return inches;
        }

        public static double ConvertFromInternalToMillimeters(double d)
        {
#if R2017 || R2018 || R2019 || R2020
            double d2 = UnitUtils.ConvertFromInternalUnits(d, DisplayUnitType.DUT_MILLIMETERS);
#else
            double d2 = UnitUtils.ConvertFromInternalUnits(d, UnitTypeId.Millimeters);
#endif
            return d2;
        }

        public static string GetElementIdAsString(ElementId elementId)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            string val = elementId.IntegerValue.ToString();
#else
            string val = elementId.Value.ToString();
#endif
            return val;
        }
    }
}
