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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace AutoJoin
{
    public static class Intersection
    {
        ///Проверить, пересекаются ли эти два элемента
        public static bool CheckElementsIsIntersect(Document doc, Element elem1, Element elem2)
        {
            GeometryElement gelem1 = elem1.get_Geometry(new Options());
            GeometryElement gelem2 = elem2.get_Geometry(new Options());

            bool check = false;

            if (gelem1 == null || gelem2 == null)
                return false;


            List<Solid> solids1 = GetSolidsOfElement(gelem1);
            List<Solid> solids2 = GetSolidsOfElement(gelem2);

            foreach (Solid solid1 in solids1)
            {

                foreach (Solid solid2 in solids2)
                {
                    List<Curve> curves2 = GetAllCurves(solid2);

                    foreach (Face face1 in solid1.Faces)
                    {
                        foreach (Curve curve2 in curves2)
                        {
                            //Solid intSolid = null;
                            //intSolid = BooleanOperationsUtils.ExecuteBooleanOperation(solid1, solid2, BooleanOperationsType.Intersect);

                            SetComparisonResult result = face1.Intersect(curve2);
                            if (result == SetComparisonResult.Overlap)
                            {
                                check = true;
                                return check;
                            }
                        }
                    }
                }
            }
            return check;
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

                bool check = CheckElementsIsIntersect(doc, curElem, elem);
                if (check) elems2.Add(curElem);
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


        /// <summary>
        /// Вырезает экземпляр семейства c пустотным элементов из другого элемента в модели. Требуется открытая транзакция
        /// </summary>
        public static bool CutElement(Document doc, Element elemForCut, Element elemWithVoid)
        {
            Debug.WriteLine("Try cut elem " + elemForCut.Id.GetElementIdValue().ToString()
                    + " by elem " + elemWithVoid.Id.GetElementIdValue().ToString());

            //Проверяю, можно ли вырезать геометрию из данного элемента
            bool check1 = InstanceVoidCutUtils.CanBeCutWithVoid(elemForCut);

            //проверяю, есть ли в семействе полый элемент и разрешено ли вырезание
            bool check2 = InstanceVoidCutUtils.IsVoidInstanceCuttingElement(elemWithVoid);

            //проверяю, существует ли уже вырезание
            bool check3 = InstanceVoidCutUtils.InstanceVoidCutExists(elemForCut, elemWithVoid);

            //Если одно из условий не выполняется - возвращаю false
            if(!check1 || !check2 || check3)
            {
                Debug.WriteLine("Unable to cut");
                return false;
            }

            try
            {
                InstanceVoidCutUtils.AddInstanceVoidCut(doc, elemForCut, elemWithVoid);
                Debug.WriteLine("Cut success");
                return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Cut exception " + ex.Message);
                return false;
            }
        }

    }
}
