using Autodesk.Revit.DB;

namespace RebarTools
{
    public static class Extensions
    {
        public static int GetElementId(this ElementId id)
        {
            int result = 0;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022  || R2023
            result = id.IntegerValue;
#else
            result = (int)id.Value;
#endif
            return result;
        }
    }
}
