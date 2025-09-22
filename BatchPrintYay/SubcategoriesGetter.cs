#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2025, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2025, all rigths reserved.*/
#endregion
#region usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace BatchPrintYay
{
    public class SubcategoriesGetter
    {
        private string _prefix;
        private Document _doc;
        public SubcategoriesGetter(string prefix, Document doc)
        {
            _prefix = prefix;
            _doc = doc;
        }

        public void Hide()
        {
            ApplySubcats(true);
        }

        public void Show()
        {
            ApplySubcats(false);
        }

        private void ApplySubcats(bool hide)
        {
            List<Category> noprintsubcats = GetSubcategories(_doc, _prefix);
            Debug.WriteLine("Hidden Subcategories lines found: " + noprintsubcats.Count.ToString() + " prefix " + _prefix);
            if (noprintsubcats.Count == 0)
            {
                return;
            }
            string transname = hide ? "Hide subcategory" : "Show subcategory";
            List<View> views = GetAllViews(_doc);

            using (Transaction t = new Transaction(_doc))
            {
                t.Start(transname);

                foreach (View v in views)
                {
                    foreach (Category subcat in noprintsubcats)
                    {
                        try
                        {
                            v.SetCategoryHidden(subcat.Id, hide);
                        }
                        catch
                        {
                            string msg = "Failed to hide a subcategory: " + subcat.Name + " on a view: " + v.Name;
                            Autodesk.Revit.UI.TaskDialog.Show("Error", msg);
                            throw new Exception(msg);
                        }
                    }
                }

                t.Commit();
            }
            string resultname = hide ? "Hidden " : "Shown ";
            Debug.WriteLine(resultname + noprintsubcats.Count + " categories at " + views.Count + " views");
        }

        private List<View> GetAllViews(Document doc)
        {
            List<View> views = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(v => v.HasViewDiscipline())
                .Where(v => v.ViewTemplateId == ElementId.InvalidElementId)
                .ToList();
            Debug.WriteLine("Views found: " + views.Count);

            List<ViewSheet> sheets = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>()
                .Where(i => !i.IsPlaceholder)
                .ToList();
            Debug.WriteLine("Viewsheets found: " + sheets.Count);

            views.AddRange(sheets);
            return views;
        }

        private List<Category> GetSubcategories(Document doc, string pref)
        {
            List<Category> noprintsubcats = new List<Category>();
            Category cat = Category.GetCategory(doc, BuiltInCategory.OST_Lines);
            foreach (Category subcat in cat.SubCategories)
            {
                string subcatname = subcat.Name;
                if (subcatname.StartsWith(pref))
                {
                    noprintsubcats.Add(subcat);
                }
            }
            Debug.WriteLine("Subcategories found: " + noprintsubcats.Count);
            return noprintsubcats;
        }
    }
}
