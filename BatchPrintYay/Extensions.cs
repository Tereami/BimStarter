using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchPrintYay
{
    public static class Extensions
    {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023

        public static int GetElementId(this Element elem)
        {
            return elem.Id.IntegerValue;
        }
#else
        public static long GetElementId(this Element elem)
        {
            return elem.Id.Value;
        }
#endif
    }
}
