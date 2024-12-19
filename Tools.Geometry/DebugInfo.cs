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
#endregion




namespace Tools.Geometry
{
    public class DebugInfo
    {
        public static string ProfileDebugInfo(List<Curve> profile)
        {
            string msg = "Curves count: " + profile.Count.ToString();

            for (int i = 0; i < profile.Count; i++)
            {
                Curve c = profile[i];
                msg += CurveDebugInfo(c);
            }

            return msg;
        }

        public static string CurveDebugInfo(Curve c)
        {
            List<string> info = new List<string>()
            {
                $"Curve length: {InchesToStringMillimeters(c.Length)}",
                 GetPointDebugInfo(c.GetEndPoint(0)),
                 GetPointDebugInfo(c.GetEndPoint(1)),
            };
            string msg = string.Join(Environment.NewLine, info);
            return msg;
        }

        public static string GetPointDebugInfo(XYZ point)
        {
            string msg = $"X: {InchesToStringMillimeters(point.X)}\tY: {InchesToStringMillimeters(point.Y)}\tZ: {InchesToStringMillimeters(point.Y)}";
            return msg;
        }

        static public string PointString(XYZ p)
        {
            string result = $"x={p.X.ToString("0.##")},y={p.Y.ToString("0.##")},z={p.Z.ToString("0.##")}";
            return result;
        }

        public static string InchesToStringMillimeters(double inches)
        {
            double mm = UnitUtils.ConvertFromInternalUnits(inches, DisplayUnitType.DUT_MILLIMETERS);
            string text = mm.ToString("0.#");
            return text;
        }
    }
}
