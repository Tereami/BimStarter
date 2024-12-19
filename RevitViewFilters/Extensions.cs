using Autodesk.Revit.DB;

namespace RevitViewFilters
{
    public static class Extensions
    {
        public static long GetValue(this ElementId elementId)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            return elementId.IntegerValue;
#else
            return elementId.Value;
#endif
        }
    }
}
