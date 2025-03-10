using System;
using System.Collections.Generic;

namespace Tools.StringsNaturalSort
{
    public static class Extensions
    {
        public static void SortNatural(this List<string> list)
        {
            NumericComparer nc = new NumericComparer();
            list.Sort(nc);
        }

        public static void SortNatural(this string[] array)
        {
            NumericComparer nc = new NumericComparer();
            Array.Sort(array, nc);
        }
    }
}
