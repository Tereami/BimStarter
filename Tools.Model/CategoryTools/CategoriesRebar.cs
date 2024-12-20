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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using System.Collections.Generic;
#endregion


namespace Tools.Model.CategoryTools
{
    public class CategoriesRebar
    {
        Application app { get; }
        Document doc { get; }

        public CategoriesRebar(Application App, Document Doc)
        {
            app = App;
            doc = Doc;
        }

        public CategorySet GetRebarCategory()
        {
            CategorySet catSet = app.Create.NewCategorySet();
            catSet.Insert(doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rebar));
            return catSet;
        }


        public CategorySet GetAllAvailableCategories()
        {
            CategorySet catSet = app.Create.NewCategorySet();
            Categories cats = doc.Settings.Categories;

            foreach (Category cat in cats)
            {
                bool check = cat.AllowsBoundParameters;
                if (check)
                {
                    catSet.Insert(cat);
                }
            }
            return catSet;
        }


        public List<ElementId> GetConcreteCategodyIds()
        {
            List<ElementId> ids = new List<ElementId>
            {
                new ElementId(BuiltInCategory.OST_Walls),
                new ElementId(BuiltInCategory.OST_StructuralColumns),
                new ElementId(BuiltInCategory.OST_StructuralFraming),
                new ElementId(BuiltInCategory.OST_StructuralFoundation),
                new ElementId(BuiltInCategory.OST_Floors),
                new ElementId(BuiltInCategory.OST_GenericModel),
            };
            return ids;
        }
    }
}