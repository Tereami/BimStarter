#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Forms
{
    public class RevitCategory
    {
        public BuiltInCategory InternalCategory { get; set; }

        public string DisplayName { get; set; }

        public RevitCategory()
        {
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static List<RevitCategory> LoadAllCategories(Document doc, Predicate<Category> filter)
        {
            List<RevitCategory> cats = new List<RevitCategory>();
            foreach (Category cat in doc.Settings.Categories)
            {
                //if (cat.CategoryType == CategoryType.Model || (cat.CategoryType == CategoryType.Annotation && cat.AllowsBoundParameters))

                if (filter(cat))
                {
                    string name = cat.Name;
                    if (string.IsNullOrEmpty(name)) continue;

                    BuiltInCategory bic = (BuiltInCategory)cat.Id.GetValue();



                    RevitCategory rc = new RevitCategory { DisplayName = name, InternalCategory = bic };
                    cats.Add(rc);
                }
            }

            cats = cats.OrderBy(i => i.DisplayName).ToList();
            return cats;
        }
    }
}
