using Autodesk.Revit.DB;

namespace Tools.Forms
{
    public static class Extensions
    {
        public static int GetValue(this ElementId elemid)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            return elemid.IntegerValue;
#else
            return (int)elemid.Value;
#endif
        }
    }
}
