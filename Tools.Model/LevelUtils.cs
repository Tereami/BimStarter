#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion


namespace Tools.Model
{
    public static class LevelUtils
    {
        public static string GetFloorNumberByLevel(Level lev, int floorTextPosition)
        {
            string levname = lev.Name;
            Trace.WriteLine($"Try to get floor number by level name: {levname}");
            string[] splitname = levname.Split(' ');
            if (splitname.Length < 2)
            {
                Trace.WriteLine($"Incorrect level name: {levname}");
                throw new Exception($"Некорректное имя уровня: {levname}");
            }
            string floorNumber = splitname[floorTextPosition];
            Trace.WriteLine($"Floor number: {floorNumber}");
            return floorNumber;
        }
        public static Level GetLevelOfElement(Element elem, Document doc)
        {
            Trace.WriteLine($"Try to get level of elem: {elem.Id}");
            ElementId levId = elem.LevelId;
            if (levId == ElementId.InvalidElementId)
            {
                Parameter levParam = elem.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM);
                if (levParam != null)
                    levId = levParam.AsElementId();
            }

            if (levId == ElementId.InvalidElementId)
            {
                Parameter levParam = elem.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM);
                if (levParam != null)
                    levId = levParam.AsElementId();
            }

            if (levId == ElementId.InvalidElementId)
            {
                Parameter levParam = elem.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);
                if (levParam != null)
                    levId = levParam.AsElementId();
            }

            if (levId == ElementId.InvalidElementId)
            {
                Trace.WriteLine("Try to get level by geometry");
                List<Solid> solids = GeometryUtils.GetSolidsFromElement(elem);
                if (solids.Count == 0) return null;
                XYZ[] maxmin = GeometryUtils.GetMaxMinHeightPoints(solids);
                XYZ minPoint = maxmin[1];
                levId = GetNearestLevel(minPoint, doc);
            }

            if (levId == ElementId.InvalidElementId)
            {
                Trace.WriteLine("Unable to get level");
                throw new Exception($"Не удалось получить уровень у элемента {elem.Id}");
            }
            Trace.WriteLine($"Level id: {levId}");
            Level lev = doc.GetElement(levId) as Level;
            return lev;
        }

        public static ElementId GetNearestLevel(XYZ point, Document doc)
        {
            BasePoint projectBasePoint = new FilteredElementCollector(doc)
                .OfClass(typeof(BasePoint))
                .WhereElementIsNotElementType()
                .Cast<BasePoint>()
                .Where(i => i.IsShared == false)
                .First();
            double projectPointElevation = projectBasePoint.get_BoundingBox(null).Min.Z;

            double pointZ = point.Z;
            List<Level> levels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .ToList();

            Level finalLevel = null;

            foreach (Level lev in levels)
            {
                if (finalLevel == null)
                {
                    finalLevel = lev;
                    continue;
                }
                if (lev.Elevation < finalLevel.Elevation)
                {
                    finalLevel = lev;
                    continue;
                }
            }

            double offset = 10000;
            foreach (Level lev in levels)
            {
                double levHeigth = lev.Elevation + projectPointElevation;
                double testElev = pointZ - levHeigth;
                if (testElev < 0) continue;

                if (testElev < offset)
                {
                    finalLevel = lev;
                    offset = testElev;
                }
            }

            return finalLevel.Id;
        }
    }
}