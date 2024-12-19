using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitViewFilters
{
    public interface IFilterData
    {
        int ValuesCount { get; }
        string FilterNamePrefix { get; }
        bool ApplyFilters(Document doc, View v, ElementId fillPatternId, bool colorLines, bool colorFill);
        MyDialogResult CollectValues(Document doc, View v);
    }
}
