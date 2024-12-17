using Autodesk.Revit.DB;

namespace FixSlowFile
{
    public static class Extensions
    {
        public static int GetElementId(this ElementId elemId)
        {
            int id = 0;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            id = elemId.IntegerValue;
#else
            id = (int)elemId.Value;
#endif
            return id;
        }
    }
}
