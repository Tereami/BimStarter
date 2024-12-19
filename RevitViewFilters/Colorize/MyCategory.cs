using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitViewFilters
{
    public class MyCategory
    {
        public BuiltInCategory BuiltinCat;
        public Category Cat;

        public MyCategory(Document doc, ElementId categoryId)
        {
            BuiltinCat = (BuiltInCategory)categoryId.GetValue();

            Cat = Category.GetCategory(doc, BuiltinCat);
        }

        public override string ToString()
        {
            return Cat.Name;
        }
    }
}
