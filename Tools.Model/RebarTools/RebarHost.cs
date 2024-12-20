using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
using Tools.Geometry;

namespace Tools.Model.RebarTools
{
    public static class RebarHost
    {
        /// <summary>
        /// Получает конструкцию, которой принадлежит IFC-стержень
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="curView"></param>
        /// <param name="rebar"></param>
        /// <param name="concreteElements"></param>
        /// <returns></returns>
        public static Element GetHostElementForIfcRebar(Document doc, View view, Element rebar, List<Element> concreteElements, Transform transform2)
        {
            Element hostElem = null;
            List<Element> intersectElems = Intersection.GetAllIntersectionElements(doc, view, rebar, concreteElements, transform2);
            if (intersectElems == null || intersectElems.Count == 0)
            {
                //эта ifc-арматура висит в воздухе, пропускаем
                return null;
            }

            if (intersectElems.Count == 1) hostElem = intersectElems.First(); //ifc-арматура пересекается только с одной конструкцией, это и есть её основа

            //если пересекает несколько конструкций - берем нижнюю
            if (intersectElems.Count > 1)
            {
                View defaultView = Tools.Model.ViewUtils.GetDefaultView(doc);
                hostElem = Tools.Geometry.Height.GetBottomElement(intersectElems, defaultView);
            }

            return hostElem;
        }

        public static bool CheckIntersectionRebarAndElement(Document doc, Autodesk.Revit.DB.Structure.Rebar rebar, Element elem, View view, Transform linkTransform)
        {
            GeometryElement geoElem = elem.get_Geometry(new Options());
            List<Solid> solids = Tools.Geometry.Solids.GetSolidsFromElement(geoElem);

#if R2017
            List<Curve> rebarCurves = rebar.ComputeDrivingCurves().ToList();
#else
            List<Curve> rebarCurves = rebar.GetShapeDrivenAccessor().ComputeDrivingCurves().ToList();
#endif
            for (int i = 0; i < solids.Count; i++)
            {
                Solid s = solids[i];
                if (!linkTransform.IsIdentity)
                {
                    s = SolidUtils.CreateTransformed(s, linkTransform.Inverse);
                }
                foreach (Face face in s.Faces)
                {
                    foreach (Curve rebarCurve in rebarCurves)
                    {
                        SetComparisonResult result = face.Intersect(rebarCurve);
                        if (result == SetComparisonResult.Overlap) return true;
                    }
                }
            }
            return false;
        }
    }
}
