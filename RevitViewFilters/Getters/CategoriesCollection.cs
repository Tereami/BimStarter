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
#region Usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
#endregion

namespace RevitViewFilters
{
    public class CategoriesCollection
    {
        public Dictionary<string, BuiltInCategory> categories;

        public CategoriesCollection(Document doc)
        {
            categories = new Dictionary<string, BuiltInCategory>();

            Categories cats = doc.Settings.Categories;

            var iterator = cats.GetEnumerator();
            iterator.Reset();
            while (iterator.MoveNext())
            {
                Category cat = iterator.Current as Category;
                if (cat == null) continue;
                BuiltInCategory bic = (BuiltInCategory)cat.Id.GetValue();
                string catname = cat.Name;
                categories.Add(catname, bic);
            }
        }

        public BuiltInCategory GetCategoryByName(string name)
        {
            BuiltInCategory bic = BuiltInCategory.INVALID;
            if (categories.ContainsKey(name))
            {
                bic = categories[name];
            }
            else
            {
                throw new Exception("Incorrect category: " + name);
            }
            return bic;
        }
    }
}
