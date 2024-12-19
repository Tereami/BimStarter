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
    public class Orientation
    {
        const double _eps = 1.0e-9;
        static public bool IsZero(double a)
        {
            return _eps > Math.Abs(a);
        }

        static public bool IsHorizontal(XYZ v)
        {
            return IsZero(v.Z);
        }

        static public bool IsVertical(XYZ v)
        {
            return IsZero(v.X) && IsZero(v.Y);
        }

        static public bool IsHorizontal(Edge e)
        {
            XYZ p = e.Evaluate(0);
            XYZ q = e.Evaluate(1);
            return IsHorizontal(q - p);
        }

        static public bool IsHorizontal(PlanarFace f)
        {
            return IsVertical(f.FaceNormal);
        }

        static public bool IsVertical(PlanarFace f)
        {
            return IsHorizontal(f.FaceNormal);
        }

        static public bool IsVertical(CylindricalFace f)
        {
            return IsVertical(f.Axis);
        }

        public static double FindTheLongestCurveLength(View v, Element elem)
        {
            Options opt = new Options { View = v };
            GeometryElement geoElem = elem.get_Geometry(opt);
            List<Solid> solids = Tools.Geometry.Solids.GetSolidsFromElement(geoElem);

            double length = 0;

            foreach (Solid sol in solids)
            {
                List<Curve> curves = Tools.Geometry.Solids.GetAllCurves(sol);
                foreach (Curve c in curves)
                {
                    if (c.Length > length)
                        length = c.Length;
                }
            }
            return length;
        }
    }
}