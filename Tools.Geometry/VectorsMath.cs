#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region usings
#endregion

using Autodesk.Revit.DB;
using System;

namespace Tools.Geometry
{
    public static class VectorsMath
    {
        /// <summary>
        /// Вычисляет векторное произведение. Если оно равно 0 - значит векторы параллельны.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static XYZ CrossProduct(XYZ v1, XYZ v2)
        {
            double X = v1.Y * v2.Z - v1.Z * v2.Y;
            double Y = v1.Z * v2.X - v1.X * v2.Z;
            double Z = v1.X * v2.Y - v1.Y * v2.X;

            XYZ crossProduct = new XYZ(X, Y, Z);
            return crossProduct;
        }

        /// <summary>
        /// Создает вектор из линии
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public static XYZ GetVectorFromLine(Line l)
        {
            XYZ p1 = l.GetEndPoint(0);
            XYZ p2 = l.GetEndPoint(1);
            XYZ v = getVectorFromTwoPoints(p1, p2);
            return v;
        }


        /// <summary>
        /// Возвращает вектор напрвления между точками
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static XYZ getVectorFromTwoPoints(XYZ p1, XYZ p2)
        {
            double X = p2.X - p1.X;
            double Y = p2.Y - p1.Y;
            double Z = p2.Z - p1.Z;

            XYZ v = new XYZ(X, Y, Z);
            return v;
        }


        /// <summary>
        /// Возвращает длину вектора v
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double GetVectorLength(XYZ v)
        {
            double d1 = Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2);
            double l = Math.Sqrt(d1);
            return l;
        }

        /// <summary>
        /// Создает вектор с длиной 1 и направлением как у заданного вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static XYZ normalizeVector(XYZ vector)
        {
            double l = GetVectorLength(vector);
            XYZ newVector = new XYZ(vector.X / l, vector.Y / l, vector.Z / l);
            return newVector;
        }
    }
}
