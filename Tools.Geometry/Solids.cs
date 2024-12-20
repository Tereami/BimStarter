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
#endregion

namespace Tools.Geometry
{
    public static class Solids
    {
        public static List<Solid> GetSolidsFromElement(Element elem)
        {
            Options opt = new Options();
            opt.ComputeReferences = true;
            opt.DetailLevel = ViewDetailLevel.Fine;
            GeometryElement geoElem = elem.get_Geometry(opt);

            List<Solid> solids = GetSolidsFromElement(geoElem);
            return solids;
        }

        public static List<Solid> GetSolidsFromElement(GeometryElement geoElem)
        {
            Trace.WriteLine("Get solids from geoelem");
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
                    List<Solid> solids2 = GetSolidsFromElement(instGeoElement);
                    solids.AddRange(solids2);
                }
            }
            Trace.WriteLine("Solids found: " + solids.Count.ToString());
            return solids;
        }

        /// <summary>
        /// Получает первый солид из элемента
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public static Solid GetSolidFromElement(Element elem)
        {
            List<Solid> solids = GetSolidsFromElement(elem);
            if (solids == null || solids.Count == 0)
            {
                throw new System.Exception($"Element id {elem.Id} has no 3D geometry!");
            }

            Solid mainSolid = solids[0];
            foreach (Solid solid in solids)
            {
                if (solid.Volume > mainSolid.Volume)
                    mainSolid = solid;
            }
            return mainSolid;
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
        private static bool ContainsSolids(GeometryElement geoElem)
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
        public static List<Curve> GetAllCurves(Solid solid)
        {
            List<Curve> curves = new List<Curve>();
            foreach (Face face in solid.Faces)
            {
                curves.AddRange(Tools.Geometry.FaceUtils.GetFaceOuterBoundary(face));
            }

            return curves;
        }


    }
}