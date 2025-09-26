#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace Tools.Geometry
{
    public static class Height
    {
        public static double GetZoneHeigth(List<Curve> lines)
        {
            List<XYZ> points = new List<XYZ>();
            XYZ bottomPoint = lines.First().GetEndPoint(0);
            XYZ topPoint = lines.First().GetEndPoint(1);

            foreach (Curve curve in lines)
            {
                points.Add(curve.GetEndPoint(0));
                points.Add(curve.GetEndPoint(1));
            }

            foreach (XYZ point in points)
            {
                if (point.Z > topPoint.Z)
                {
                    topPoint = point;
                }
                if (point.Z < bottomPoint.Z)
                {
                    bottomPoint = point;
                }
            }

            double heigth = topPoint.Z - bottomPoint.Z;
            Trace.WriteLine($"Zone height: {heigth * 304.8}");
            return heigth;
        }

        public static HeightResult GetMaxMinHeightPoints(Element elem)
        {
            List<Solid> solids = Tools.Geometry.Solids.GetSolidsFromElement(elem);
            HeightResult result = Tools.Geometry.Height.GetMaxMinHeightPoints(solids);
            return result;
        }

        public static HeightResult GetMaxMinHeightPoints(List<Solid> solids)
        {
            XYZ maxZpoint = new XYZ(0, 0, -9999999);
            XYZ minZpoint = new XYZ(0, 0, 9999999);

            List<Edge> edges = new List<Edge>();
            foreach (Solid s in solids)
            {
                foreach (Edge e in s.Edges)
                {
                    edges.Add(e);
                }
            }

            foreach (Edge e in edges)
            {
                Curve c = e.AsCurve();
                XYZ p1 = c.GetEndPoint(0);
                if (p1.Z > maxZpoint.Z) maxZpoint = p1;
                if (p1.Z < minZpoint.Z) minZpoint = p1;

                XYZ p2 = c.GetEndPoint(1);
                if (p2.Z > maxZpoint.Z) maxZpoint = p2;
                if (p2.Z < minZpoint.Z) minZpoint = p2;
            }
            HeightResult result = new HeightResult(maxZpoint, minZpoint);
            return result;
        }


        public static XYZ GetTopPoint(List<XYZ> points)
        {
            XYZ topPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                XYZ curPoint = points[i];
                if (curPoint.Z > topPoint.Z)
                {
                    topPoint = curPoint;
                }
            }
            return topPoint;
        }
        public static XYZ GetBottomPoint(List<XYZ> points)
        {
            XYZ topPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                XYZ curPoint = points[i];
                if (curPoint.Z < topPoint.Z)
                {
                    topPoint = curPoint;
                }
            }
            return topPoint;
        }

        /// <summary>
        /// Получает нижнюю точку линии
        /// </summary>
        public static XYZ GetBottomPoint(Curve curve)
        {
            XYZ p1 = curve.GetEndPoint(0);
            XYZ p2 = curve.GetEndPoint(1);
            if (p1.Z < p2.Z) return p1;
            return p2;
        }

        /// <summary>
        /// Получает самый нижерасположенный элемент из списка
        /// </summary>
        /// <param name="elems"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static Element GetBottomElement(List<Element> elems, View view)
        {
            Element elem = null;
            double TopPointOfBottomElement = -999999;

            foreach (Element curElem in elems)
            {
                BoundingBoxXYZ box = curElem.get_BoundingBox(view);
                XYZ topPoint = box.Max;
                double curTopElev = topPoint.Z;

                if (elem == null)
                {
                    elem = curElem;
                    TopPointOfBottomElement = curTopElev;
                    continue;
                }


                if (curTopElev < TopPointOfBottomElement)
                {
                    TopPointOfBottomElement = curTopElev;
                    elem = curElem;
                }
            }

            return elem;
        }
    }
}