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
        public static Guid trueLengthGuid = new Guid("b62d0a35-0f0f-432d-9d3d-e821093a7d02"); //Рзм.ДлинаБалкаИстинная
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
        public static Level GetLevelOfElement(Element elem)
        {
            Trace.WriteLine($"Try to get level of elem: {elem.Id}");
            Document doc = elem.Document;
            Level baseLevel = null;

            try
            {
                //Это для обычных семейств на основе стены или уровня, у которых есть параметр "Уровень"
                baseLevel = doc.GetElement(elem.LevelId) as Level;
                if (baseLevel != null)
                {
                    Trace.WriteLine($"Level is found as LevelId, id: {baseLevel.Id}");
                    return baseLevel;
                }
            }
            catch { }

            //от определения базового уровня через элемент-основу решил пока отказаться т.к. сложно определить смещение
            /*if (baseLevel == null)
            {
                FamilyInstance fi = elem as FamilyInstance;
                if (fi != null)
                {
                    Element hostElem = fi.Host;
                    if (hostElem != null)
                    {
                        if (hostElem is Level)
                        {
                            baseLevel = hostElem as Level;
                        }
                        else
                        {
                            Trace.WriteLine("Recursively try tp find a level by host elem, id: " + hostElem.Id.GetValue());
                            baseLevel = GetBaseLevel(hostElem);
                        }
                        if (baseLevel != null)
                        {
                            Trace.WriteLine("Level is found as a host level, id: " + baseLevel.Id.GetValue());
                            return baseLevel;
                        }
                    }
                }
            }*/

            Trace.WriteLine("Try to find base level as a parameter");

            List<BuiltInParameter> baseLevelParams = new List<BuiltInParameter> {
                BuiltInParameter.LEVEL_PARAM,
                BuiltInParameter.FAMILY_LEVEL_PARAM,
                BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
                BuiltInParameter.SCHEDULE_BASE_LEVEL_PARAM,
                BuiltInParameter.SCHEDULE_LEVEL_PARAM,
                BuiltInParameter.WALL_BASE_CONSTRAINT
            };

            baseLevel = GetLevelUsingParameters(elem, baseLevelParams);

            if (baseLevel == null)
            {
                Trace.WriteLine($"Failed to find base level for element id {elem.Id}");
                throw new Exception($"Не удалось получить уровень у элемента {elem.Id}");
            }
            return baseLevel;


            //ElementId levId = elem.LevelId;
            //if (levId == ElementId.InvalidElementId)
            //{
            //    Parameter levParam = elem.get_Parameter(BuiltInParameter.SCHEDULE_LEVEL_PARAM);
            //    if (levParam != null)
            //        levId = levParam.AsElementId();
            //}

            //if (levId == ElementId.InvalidElementId)
            //{
            //    Parameter levParam = elem.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM);
            //    if (levParam != null)
            //        levId = levParam.AsElementId();
            //}

            //if (levId == ElementId.InvalidElementId)
            //{
            //    Parameter levParam = elem.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);
            //    if (levParam != null)
            //        levId = levParam.AsElementId();
            //}

            //if (levId == ElementId.InvalidElementId)
            //{
            //    Trace.WriteLine("Try to get level by geometry");
            //    List<Solid> solids = Tools.Geometry.Solids.GetSolidsFromElement(elem);
            //    if (solids.Count == 0) return null;
            //    XYZ[] maxmin = Tools.Geometry.Height.GetMaxMinHeightPoints(solids);
            //    XYZ minPoint = maxmin[1];
            //    levId = GetNearestLevel(minPoint, doc);
            //}

            //if (levId == ElementId.InvalidElementId)
            //{
            //    Trace.WriteLine("Unable to get level");
            //    throw new Exception($"Не удалось получить уровень у элемента {elem.Id}");
            //}
            //Trace.WriteLine($"Level id: {levId}");
            //Level lev = doc.GetElement(levId) as Level;
            //return lev;
        }

        public static Level GetTopLevel(Element elem)
        {
            Document doc = elem.Document;

            List<BuiltInParameter> topLevParams = new List<BuiltInParameter> {
                BuiltInParameter.WALL_HEIGHT_TYPE,
                BuiltInParameter.SCHEDULE_TOP_LEVEL_PARAM,
                BuiltInParameter.FAMILY_TOP_LEVEL_PARAM
            };

            Level topLevel = GetLevelUsingParameters(elem, topLevParams);
            return topLevel;
        }

        private static Level GetLevelUsingParameters(Element elem, List<BuiltInParameter> builtInParameters)
        {
            Document doc = elem.Document;
            Level lev = null;
            foreach (BuiltInParameter bip in builtInParameters)
            {
                Parameter levelParam = elem.get_Parameter(bip);
                if (levelParam != null && levelParam.HasValue)
                {
                    ElementId levId = levelParam.AsElementId();
                    if (levId != ElementId.InvalidElementId)
                    {
                        lev = doc.GetElement(levId) as Level;
                        if (lev != null)
                        {
                            string paramName = Enum.GetName(typeof(BuiltInParameter), bip);
                            Trace.WriteLine($"Level is found as {paramName} level id {lev.Id}");
                            return lev;
                        }
                    }
                }
            }
            Trace.WriteLine("Failed to find level");
            return lev;
        }

        /// <summary>
        /// Получает смещение от уровня через параметр семейства.
        /// 
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static double GetOffsetFromBaseLevel(Element elem)
        {
            List<BuiltInParameter> offsetParams = new List<BuiltInParameter> {
                BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM,
                BuiltInParameter.INSTANCE_ELEVATION_PARAM,
                BuiltInParameter.SCHEDULE_BASE_LEVEL_OFFSET_PARAM,
                BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM,
                BuiltInParameter.INSTANCE_SILL_HEIGHT_PARAM,
                BuiltInParameter.WALL_BASE_OFFSET,
                BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM
            };

            double baseLevelOffset = GetDoubleUsingParameters(elem, offsetParams);
            return baseLevelOffset;
        }

        public static double GetOffsetFromTopLevel(Element elem)
        {
            List<BuiltInParameter> offsetParams = new List<BuiltInParameter> {
                BuiltInParameter.SCHEDULE_TOP_LEVEL_OFFSET_PARAM,
                BuiltInParameter.WALL_TOP_OFFSET,
                BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM
            };

            double topLevelOffset = GetDoubleUsingParameters(elem, offsetParams);
            return topLevelOffset;
        }

        private static double GetDoubleUsingParameters(Element elem, List<BuiltInParameter> builtInParameters)
        {
            Parameter param = null;
            double tolerance = 1e-6;

            foreach (BuiltInParameter bip in builtInParameters)
            {
                param = elem.get_Parameter(bip);
                if (param != null && param.HasValue)
                {
                    double elev = param.AsDouble();
                    if (Math.Abs(elev) > tolerance)
                        return elev;
                }
            }
            return 0;
        }

        public static double GetElementHeight(Element elem)
        {
            List<BuiltInParameter> heightParams = new List<BuiltInParameter> {
                BuiltInParameter.WALL_USER_HEIGHT_PARAM,
                BuiltInParameter.INSTANCE_LENGTH_PARAM
            };

            double height = GetDoubleUsingParameters(elem, heightParams);

            if (height == 0)
            {
                Parameter heightParam = elem.get_Parameter(trueLengthGuid);
                if (heightParam != null && heightParam.HasValue)
                {
                    Trace.WriteLine("Get Height as a truelength parameter");
                    height = heightParam.AsDouble();
                }
            }

            return height;
        }

        public static Level GetNearestLevel(XYZ point, Document doc)
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

            return finalLevel;
        }
    }
}