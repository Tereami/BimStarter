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
using System.ComponentModel;
using System.Linq;

namespace RevitWorksets.WorksetWorkers
{
    [Serializable]
    public class WorksetByCategory : WorksetBy
    {
        public List<BuiltInCategory> revitCategories;

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

        public WorksetByCategory()
        {

        }

        public static BindingList<WorksetByCategory> GetDefault()
        {
            BindingList<WorksetByCategory> worksets = new BindingList<WorksetByCategory>
            {
                new WorksetByCategory
                {
                    WorksetName = MyStrings.DefaultWorksetConcrete,
                    revitCategories = new List<BuiltInCategory> {
                    BuiltInCategory.OST_Walls,
                    BuiltInCategory.OST_Floors,
                    BuiltInCategory.OST_StructuralFoundation,
                }
                },
                new WorksetByCategory
                {
                    WorksetName = MyStrings.DefaultWorksetRebar,
                    revitCategories = new List<BuiltInCategory> {
                   BuiltInCategory.OST_Rebar,
                   BuiltInCategory.OST_AreaRein,
                   BuiltInCategory.OST_PathRein,
                }
                },
                new WorksetByCategory
                {
                    WorksetName = MyStrings.DefaultWorksetGrids,
                    revitCategories = new List<BuiltInCategory>  {
                    BuiltInCategory.OST_Grids,
                    BuiltInCategory.OST_Levels,
                }
                }
            };
            return worksets;
        }
    }
}
