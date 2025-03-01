using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinkWriter
{
    public class CustomParameter
    {
        [DisplayName("Workset name")]
        public string Name { get; set; }

        public string Value { get; set; }

        public List<BuiltInCategory> revitCategories { get; set; }

        [DisplayName("Categories")]
        public string CategoriesText
        {
            get
            {
                if (revitCategories == null || revitCategories.Count == 0) return "None";
#if R2017 || R2018 || R2019
                string cats = revitCategories.Count + " категорий";
#else
                string cats = string.Join(" ", revitCategories.Select(i => LabelUtils.GetLabelFor(i)));
#endif
                return cats;
            }
        }

        public static CustomParameter GetDefault()
        {
            CustomParameter cp = new CustomParameter()
            {
                Name = "PARAMETER NAME",
                Value = "VALUE",
                revitCategories = new List<BuiltInCategory> { BuiltInCategory.OST_Walls }
            };
            return cp;
        }
    }
}
