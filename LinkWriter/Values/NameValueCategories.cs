using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace LinkWriter.Values
{
    public class NameValueCategories
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public List<BuiltInCategory> Categories { get; set; }

        public NameValueCategories(string name, string value, IEnumerable<BuiltInCategory> cats)
        {
            Name = name;
            Value = value;
            Categories = cats.ToList();
        }
    }
}
