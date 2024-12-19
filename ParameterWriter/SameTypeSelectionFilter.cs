using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace ParameterWriter
{
    public class SameTypeSelectionFilter : ISelectionFilter
    {
        Element firstElem;

        public SameTypeSelectionFilter(Element elem)
        {
            firstElem = elem;
        }

        bool ISelectionFilter.AllowElement(Element elem)
        {
            Type firstType = firstElem.GetType();
            Type curType = elem.GetType();
            bool check = firstType.Equals(curType);
            return check;
        }

        bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
