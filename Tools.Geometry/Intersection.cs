#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace Tools.Geometry
{
    public static class Intersection
    {
        public enum MyIntersectionResult { NoIntersection, Touching, Intersection, Incorrect }

        /// <summary>
        /// Проверить, пересекаются ли эти два элемента. Решение через объем пересекаемых солидов
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="elem1"></param>
        /// <param name="elem2"></param>
        /// <param name="transformSecondElementToFirst"></param>
        /// <returns></returns>
        public static MyIntersectionResult CheckElementsIntersection(Document doc, Element elem1, Element elem2, Transform transformSecondElementToFirst = null)
        {
            GeometryElement gelem1 = elem1.get_Geometry(new Options());
            GeometryElement gelem2 = elem2.get_Geometry(new Options());

            List<Solid> solids1 = GetSolidsOfElement(gelem1);
            List<Solid> solids2 = GetSolidsOfElement(gelem2);

            for (int i = 0; i < solids1.Count; i++)
            {
                Solid solid1 = solids1[i];
                for (int j = 0; j < solids2.Count; j++)
                {
                    Solid solid2 = solids2[j];
                    if (transformSecondElementToFirst != null && transformSecondElementToFirst.IsIdentity == false)
                    {
                        solid2 = SolidUtils.CreateTransformed(solid2, transformSecondElementToFirst);
                    }

                    bool check = false;
                    try
                    {
                        check = CheckSolidsIntersection(solid1, solid2);
                    }
                    catch
                    {
                        string msg = $"Incorrect elements: {elem1.Name} id: {elem1.Id}, {elem2.Name} id: {elem2.Id}";
                        Debug.WriteLine(msg);
                        Autodesk.Revit.UI.TaskDialog.Show("Warning", msg);
                        return MyIntersectionResult.Incorrect;
                    }
                    if (check) return MyIntersectionResult.Intersection;
                }
            }
            return MyIntersectionResult.NoIntersection;
        }


        public static bool CheckSolidsIntersection(Solid solid1, Solid solid2)
        {
            Solid interSolid = BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Intersect);
            double volume = Math.Abs(interSolid.Volume);
            if (volume > 0.000001)
            {
                return true;
            }
            return false;
        }

        /////Проверить, пересекаются ли эти два элемента. Решение через пересечение линий и граней - не очень надежное
        //public static bool CheckElementsIsIntersect(Document doc, Element elem1, Element elem2)
        //{
        //    GeometryElement gelem1 = elem1.get_Geometry(new Options());
        //    GeometryElement gelem2 = elem2.get_Geometry(new Options());

        //    bool check = false;

        //    if (gelem1 == null || gelem2 == null)
        //        return false;


        //    List<Solid> solids1 = GetSolidsOfElement(gelem1);
        //    List<Solid> solids2 = GetSolidsOfElement(gelem2);

        //    foreach (Solid solid1 in solids1)
        //    {

        //        foreach (Solid solid2 in solids2)
        //        {
        //            List<Curve> curves2 = GetAllCurves(solid2);

        //            foreach (Face face1 in solid1.Faces)
        //            {
        //                foreach (Curve curve2 in curves2)
        //                {
        //                    //Solid intSolid = null;
        //                    //intSolid = BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Intersect);

        //                    SetComparisonResult result = face1.Intersect(curve2);
        //                    if (result == SetComparisonResult.Overlap)
        //                    {
        //                        check = true;
        //                        return check;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return check;
        //}

        /// <summary>
        /// Проверить пересечение линии и элемента
        /// </summary>
        public static List<XYZ> CheckIntersectCurveAndElement(Curve curve, Element elem)
        {
            GeometryElement geoElem = elem.get_Geometry(new Options());
            List<Solid> solids = GetSolidsOfElement(geoElem);
            List<XYZ> intersectPoints = new List<XYZ>();
            foreach (Solid sol in solids)
            {
                foreach (Face face in sol.Faces)
                {
                    IntersectionResultArray resultArray;
                    SetComparisonResult intersectCheck = face.Intersect(curve, out resultArray);
                    if (intersectCheck == SetComparisonResult.Overlap)
                    {
                        foreach (IntersectionResult ir in resultArray)
                        {
                            XYZ point = ir.XYZPoint;
                            intersectPoints.Add(point);
                        }
                    }

                }
            }
            return intersectPoints;
        }


        /// <summary>
        /// Получить список солидов из данной геометрии
        /// </summary>
        private static List<Solid> GetSolidsOfElement(GeometryElement geoElem)
        {
            List<Solid> solids = new List<Solid>();

            foreach (GeometryObject geoObj in geoElem)
            {
                if (geoObj is Solid)
                {
                    Solid solid = geoObj as Solid;
                    if (solid == null) continue;
                    if (solid.Volume == 0) continue;
                    solids.Add(solid);
                    continue;
                }
                if (geoObj is GeometryInstance)
                {
                    GeometryInstance geomIns = geoObj as GeometryInstance;
                    GeometryElement instGeoElement = geomIns.GetInstanceGeometry();
                    List<Solid> solids2 = GetSolidsOfElement(instGeoElement);
                    solids.AddRange(solids2);
                }
            }
            return solids;
        }

        /// <summary>
        /// Проверить, содержит ли данный элемент объемную 3D-геометрию
        /// </summary>
        public static bool ContainsSolids(Element elem)
        {
            GeometryElement geoElem = elem.get_Geometry(new Options());
            if (geoElem == null) return false;

            bool check = ContainsSolids(geoElem);
            return check;
        }

        /// <summary>
        /// Проверить, содержит ли данный элемент объемную 3D-геометрию
        /// </summary>
        public static bool ContainsSolids(GeometryElement geoElem)
        {
            if (geoElem == null) return false;

            foreach (GeometryObject geoObj in geoElem)
            {
                if (geoObj is Solid)
                {
                    return true;
                }
                if (geoObj is GeometryInstance)
                {
                    GeometryInstance geomIns = geoObj as GeometryInstance;
                    GeometryElement instGeoElement = geomIns.GetInstanceGeometry();
                    return ContainsSolids(instGeoElement);
                }
            }
            return false;
        }


        /// <summary>
        /// Получить все ребра из солида
        /// </summary>
        private static List<Curve> GetAllCurves(Solid solid)
        {
            List<Curve> curves = new List<Curve>();
            foreach (Face face in solid.Faces)
                curves.AddRange(GetAllCurves(face));

            return curves;
        }

        /// <summary>
        /// Получить все ребра из грани
        /// </summary>
        private static List<Curve> GetAllCurves(Face face)
        {
            List<Curve> curves = new List<Curve>();

            foreach (EdgeArray loop in face.EdgeLoops)
            {
                foreach (Edge edge in loop)
                {
                    Curve c2 = edge.AsCurve();
                    curves.Add(c2);
                    //List<XYZ> points = edge.Tessellate() as List<XYZ>;
                    //for (int ii = 0; ii + 1 < points.Count; ii++)
                    //{
                    //    try
                    //    {
                    //        Line line = Line.CreateBound(points[ii], points[ii + 1]);
                    //        curves.Add(line);
                    //    }
                    //    catch { }
                    //}
                }
            }
            return curves;
        }

        /// <summary>
        /// Получает список всех элементов, которые пересекает данный элемент
        /// </summary>
        public static List<Element> GetAllIntersectionElements(Document doc, Element elem)
        {
            List<Element> elems = new List<Element>();

            elems = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .Where(x => ContainsSolids(x))
                .ToList();


            //.Select(x => x.get_Geometry(new Options()).ToList());

            List<Element> elems2 = new List<Element>();

            foreach (Element curElem in elems)
            {
                if (curElem.Id == elem.Id) continue; //один и тот же элемент

                MyIntersectionResult check = CheckElementsIntersection(doc, curElem, elem);
                if (check == MyIntersectionResult.Intersection) elems2.Add(curElem);
            }

            if (elems2.Count == 0)
            {
                return null;
            }

            return elems2;
        }

        /*
        public static List<Element> GetAllIntersectionElements2(Document doc, Element voidElem)
        {
            Options op = new Options();
            op.ComputeReferences = true;
            GeometryElement ge = voidElem.get_Geometry(op);
            Solid sol = null;
            foreach (GeometryObject geo in ge)
            {
                if (geo == null) continue;
                if (geo is Solid)
                {
                    sol = geo as Solid;
                    break;
                }
            }
            ElementIntersectsSolidFilter solins = new ElementIntersectsSolidFilter(sol);

            FilteredElementCollector filcol = new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(solins);

            foreach (Element e in filcol)
            {
                InstanceVoidCutUtils.AddInstanceVoidCut(doc, e, voidElem);
            }


        }
        */


        public static bool CheckBoundingBoxesIntersect(BoundingBoxXYZ box1, BoundingBoxXYZ box2)
        {
            XYZ center1 = new XYZ((box1.Min.X + box1.Max.X) / 2, (box1.Min.Y + box1.Max.Y) / 2, (box1.Min.Z + box1.Max.Z) / 2);
            XYZ halfwidth1 = new XYZ((box1.Max.X - box1.Min.X) / 2, (box1.Max.Y - box1.Min.Y) / 2, (box1.Max.Z - box1.Min.Z) / 2);

            XYZ center2 = new XYZ((box2.Min.X + box2.Max.X) / 2, (box2.Min.Y + box2.Max.Y) / 2, (box2.Min.Z + box2.Max.Z) / 2);
            XYZ halfwidth2 = new XYZ((box2.Max.X - box2.Min.X) / 2, (box2.Max.Y - box2.Min.Y) / 2, (box2.Max.Z - box2.Min.Z) / 2);


            if (Math.Abs(center1.X - center2.X) > (halfwidth1.X + halfwidth2.X)) return false;
            if (Math.Abs(center1.Y - center2.Y) > (halfwidth1.Y + halfwidth2.Y)) return false;
            if (Math.Abs(center1.Z - center2.Z) > (halfwidth1.Z + halfwidth2.Z)) return false;

            return true;
        }

        public static List<Element> GetAllIntersectionElements(Document doc, View view, Element elem, List<Element> elems, Transform transformToFirstElem)
        {
            List<Element> elems2 = new List<Element>();

            foreach (Element curElem in elems)
            {
                if (curElem.Id == elem.Id) continue; //один и тот же элемент

                MyIntersectionResult check = CheckElementsIntersection(doc, curElem, elem, transformToFirstElem);

                if (check == MyIntersectionResult.Intersection)
                    elems2.Add(curElem);
            }

            if (elems2.Count == 0)
            {
                return null;
            }

            return elems2;
        }
    }
}
