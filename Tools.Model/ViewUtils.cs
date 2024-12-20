#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Tools.Model
{
    public static class ViewUtils
    {
        public static View GetDefaultView(Document doc)
        {
            View curView = doc.ActiveView;
            if (curView is View3D)
            {
                return curView;
            }
            else
            {
                List<View3D> views = new FilteredElementCollector(doc)
                    .OfClass(typeof(View3D))
                    .Cast<View3D>()
                    .Where(v => v.IsTemplate == false)
                    .Where(v => v.Name.Contains("3D"))
                    .ToList();
                if (views.Count == 0) throw new Exception($"Please create a 3D view in the file {doc.Title}");

                View3D view = views.First();

                if (view.DetailLevel != ViewDetailLevel.Fine)
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("View level of detailing");

                        try
                        {
                            view.DetailLevel = ViewDetailLevel.Fine;
                        }
                        catch
                        {
                            throw new Exception($"Failed to set HIGH leveh of detailing for a view {view.Name}, view id: {view.Id}");
                        }

                        t.Commit();
                    }
                }

                return view;
            }
        }


        public static List<Element> GetAllElementsAtView(View v)
        {
            Document doc = v.Document;
            FilteredElementCollector col = new FilteredElementCollector(doc, v.Id)
                .WhereElementIsNotElementType();

            List<Element> elems = new List<Element>();
            foreach (Element elem in col)
            {
                if (elem.Category == null)
                    continue;
                ElementId camerasCategoryId = new ElementId(BuiltInCategory.OST_Cameras);
                if (elem.Category.Id == camerasCategoryId)
                    continue;

                elems.Add(elem);
            }
            return elems;
        }
    }
}