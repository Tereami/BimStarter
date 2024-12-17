#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace ArchParametrisation
{
    public static class Extensions
    {
        public static int GetElementIdValue(this ElementId id)
        {
            int result = 0;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022  || R2023
            result = id.IntegerValue;
#else
            result = (int)id.Value;
#endif
            return result;
        }

        public static List<FamilyInstance> GetMirroredElements(this Document doc)
        {
            List<FamilyInstance> col = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(i => i.Mirrored)
                .ToList();

            return col;
        }

        public static Dictionary<int, List<FamilyInstance>> GetOpenings(this Document doc, Settings sets)
        {
            List<ElementId> openingsCategories = new List<ElementId>
                {
                    new ElementId(BuiltInCategory.OST_Windows),
                    new ElementId(BuiltInCategory.OST_Doors),
                };
            ElementMulticategoryFilter openingsFilter = new ElementMulticategoryFilter(openingsCategories);
            List<FamilyInstance> openings = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WherePasses(openingsFilter)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .ToList();

            Dictionary<int, List<FamilyInstance>> roomIdsAndOpenings =
                new Dictionary<int, List<FamilyInstance>>();

            foreach (FamilyInstance fi in openings)
            {
                List<Room> curRooms = new List<Room>();
                Room room1 = fi.ToRoom;
                Room room2 = fi.FromRoom;

                if (room1 == null && room2 == null)
                {
                    Debug.WriteLine($"Opening id {fi.Id} is not in a room. Maybe model error");
                    continue;
                }
                else if (room1 != null && room2 == null)
                {
                    curRooms.Add(room1);
                }
                else if (room1 == null && room2 != null)
                {
                    curRooms.Add(room2);
                }
                else
                {
                    if (room1.Id == room2.Id)
                    {
                        Debug.WriteLine($"Opening id {fi.Id} is entirely in the one room id {room1.Id}. Maybe model error");
                        if (sets.openingsCalculateInOneRoom)
                        {
                            curRooms.Add(room1);
                        }
                    }
                    else
                    {
                        curRooms.Add(room1);
                        curRooms.Add(room2);
                    }
                }



                foreach (Room r in curRooms)
                {
                    if (r == null) continue;
                    int roomId = r.Id.GetElementIdValue();
                    if (roomIdsAndOpenings.ContainsKey(roomId))
                        roomIdsAndOpenings[roomId].Add(fi);
                    else
                        roomIdsAndOpenings.Add(roomId, new List<FamilyInstance> { fi });
                }
            }

            return roomIdsAndOpenings;
        }

        public static double GetOpeningArea(this FamilyInstance fi, Settings sets)
        {
            Parameter widthParam = SuperGetParameter(fi, sets.openingsWidthParamName);
            Parameter heightParam = SuperGetParameter(fi, sets.openingsHeightParamName);
            if (widthParam == null || heightParam == null)
                return 0;
            if (!widthParam.HasValue || !heightParam.HasValue)
                return 0;

            double width = widthParam.AsDouble();
            double height = heightParam.AsDouble();
            double openingArea = width * height;
            return openingArea;
        }

        public static Parameter SuperGetParameter(this Element Elem, string ParamName)
        {
            Parameter param = Elem.LookupParameter(ParamName);
            if (param == null)
            {
                Element eltype = Elem.Document.GetElement(Elem.GetTypeId());
                param = eltype.LookupParameter(ParamName);
            }
            return param;
        }

        public static void SetValue<T>(this Element elem, string paramName, T value, bool ShowErrors)
        {
            Parameter p = elem.LookupParameter(paramName);
            if (p == null)
            {
                string msg = $"{MyStrings.ErrorNoParameter} {paramName} {MyStrings.ErrorInElement} {elem.Id.GetElementIdValue()}";
                Debug.WriteLine(msg);
                if (ShowErrors)
                    throw new Exception(msg);
                else
                    return;
            }
            if (p.IsReadOnly)
            {
                string msg = $"{MyStrings.Parameter} {paramName} {MyStrings.ErrorParamDisabled} {elem.Id.GetElementIdValue()}";
                Debug.WriteLine(msg);
                if (ShowErrors)
                    throw new Exception(msg);
                else
                    return;
            }

            if (value is string stringvalue)
                p.Set(stringvalue);
            else if (value is double doubleVal)
                p.Set(doubleVal);
            else if (value is int intvalue)
                p.Set(intvalue);
            else if (value is ElementId idValue)
                p.Set(idValue);
            else
                throw new Exception($"{MyStrings.ErrorUnknownParamType} {paramName}");
        }

        public static T GetValue<T>(this Element elem, string paramName)
        {
            Parameter p = elem.LookupParameter(paramName);
            if (p == null)
                throw new Exception($"{MyStrings.ErrorNoParameter} {paramName} {MyStrings.ErrorInElement} {elem.Id}");
            if (!p.HasValue)
                return default(T);

            switch (p.StorageType)
            {
                case StorageType.Integer:
                    return (T)(object)p.AsInteger();
                case StorageType.Double:
                    return (T)(object)p.AsDouble();
                case StorageType.String:
                    return (T)(object)p.AsString();
                case StorageType.ElementId:
                    return (T)(object)p.AsElementId();
            }
            throw new Exception($"{MyStrings.ErrorUnknownParamType} {paramName}");
        }



        public static List<Element> GetBoundaryRoomsElements(this Room r)
        {
            List<Element> elems = new List<Element>();

            SpatialElementBoundaryOptions opts = new SpatialElementBoundaryOptions();
            opts.StoreFreeBoundaryFaces = false;

            foreach (IList<BoundarySegment> boundary in r.GetBoundarySegments(opts))
            {
                foreach (BoundarySegment segment in boundary)
                {
                    ElementId elemId = segment.ElementId;
                    if (elemId == ElementId.InvalidElementId)
                        continue;
                    Element e = r.Document.GetElement(elemId);
                    elems.Add(e);
                }
            }

            return elems;
        }
    }
}
