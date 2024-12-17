using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonBimStarter
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
