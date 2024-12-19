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
#endregion


namespace Tools.Geometry
{
    public class FaceUtils
    {
        /// <summary>
        /// Фильтрует грани, оставляет только вертикальные
        /// </summary>
        /// <param name="sol"></param>
        /// <returns></returns>
        public static List<Face> GetVerticalFaces(Solid sol)
        {
            FaceArray faces = sol.Faces;
            List<Face> verticalFaces = new List<Face>();
            foreach (Face face in faces)
            {
                PlanarFace pface = face as PlanarFace;
                if (pface == null) continue;

                XYZ normal = pface.FaceNormal;
                if (normal.Z == 0)
                {
                    verticalFaces.Add(face);
                }
            }
            return verticalFaces;
        }

        /// <summary>
        /// Получает грань наибольшей площади
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static Face GetLargeFace(List<Face> faces)
        {
            double maxArea = 0;
            Face maxFace = null;
            foreach (Face face in faces)
            {
                if (face.Reference == null) continue;
                if (face.Area > maxArea)
                {
                    maxFace = face;
                    maxArea = face.Area;
                }
            }
            return maxFace;
        }




        public static List<Curve> GetFaceOuterBoundary(Face face)
        {
            EdgeArrayArray eaa = face.EdgeLoops;
            List<Curve> curves = new List<Curve>();

            double mainArrayLength = 0;

            foreach (EdgeArray ea in eaa)
            {
                List<Curve> curCurves = new List<Curve>();
                double curArrayLength = 0;
                foreach (Edge e in ea)
                {
                    Curve line = e.AsCurve();
                    //XYZ[] pts = e.Tessellate().ToArray<XYZ>();
                    //int m = pts.Length;
                    //XYZ p = pts[0];
                    //XYZ q = pts[m - 1];
                    //Line line = Line.CreateBound(p, q);
                    curCurves.Add(line);
                    curArrayLength += line.Length;
                }
                if (curArrayLength > mainArrayLength)
                {
                    curves = curCurves;
                    mainArrayLength = curArrayLength;
                }
            }

            return curves;
        }
    }
}
