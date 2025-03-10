using System.Collections;
using System.Collections.Generic;

namespace Tools.StringsNaturalSort
{
    public class NumericComparer : IComparer, IComparer<string>
    {
        public NumericComparer()
        { }

        public int Compare(object x, object y)
        {
            if ((x is string) && (y is string))
            {
                return StringLogicalComparer.Compare((string)x, (string)y);
            }
            return -1;
        }

        public int Compare(string x, string y)
        {
            return StringLogicalComparer.Compare(x, y);
        }
    }
}
