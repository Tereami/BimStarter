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

using Autodesk.Revit.DB;

namespace Tools.Geometry
{
    public static class Points
    {
        /// <summary>
        /// Проверяет, совпадают ли точки
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool CheckPointsIsOverlap(XYZ p1, XYZ p2)
        {
            double delta = GetLengthBetweenPoints(p1, p2);
            if (delta < 0.000001) return true;
            return false;
        }

        /// <summary>
        /// Возвращает расстояние между точками
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetLengthBetweenPoints(XYZ p1, XYZ p2)
        {
            XYZ v = VectorsMath.getVectorFromTwoPoints(p1, p2);
            double length = VectorsMath.GetVectorLength(v);
            return length;
        }
    }
}
