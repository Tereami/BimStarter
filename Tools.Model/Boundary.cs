using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Tools.Model
{
    public static class Boundary
    {
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
    }
}