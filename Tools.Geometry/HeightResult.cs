using Autodesk.Revit.DB;

namespace Tools.Geometry
{
    public class HeightResult
    {
        public double TopElevation { get; }
        public double TopElevationMm { get; }
        public XYZ TopPoint { get; }

        public double BottomElevation { get; }
        public double BottomElevationMm { get; }

        public XYZ BottomPoint { get; }

        public double Height { get; }
        public double HeightMm { get; }

        public HeightResult(XYZ topPoint, XYZ bottomPoint)
        {
            TopPoint = topPoint;
            BottomPoint = bottomPoint;
            TopElevation = TopPoint.Z;
            TopElevationMm = TopElevation * 304.8;
            BottomElevation = BottomPoint.Z;
            BottomElevationMm = BottomElevation * 304.8;
            Height = TopElevation - BottomElevation;
            HeightMm = Height * 304.8;
        }
    }
}
