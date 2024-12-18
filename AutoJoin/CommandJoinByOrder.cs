#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion


namespace AutoJoin
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandJoinByOrder : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("JoinByOrder"));
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            ICollection<ElementId> ids = sel.GetElementIds();

            if (ids.Count == 0)
            {
                message = MyStrings.ErrorNoSelectedElements;
                return Result.Failed;
            }

            if (ids.Count > 100)
            {
                TaskDialog dialog = new TaskDialog(MyStrings.Warning);
                dialog.MainInstruction = $"{MyStrings.WarningTooManyElements1} ({ids.Count}), {MyStrings.WarningTooManyElements2}?";
                dialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                if (dialog.Show() != TaskDialogResult.Ok)
                {
                    return Result.Cancelled;
                }
            }

            List<Element> elems = new List<Element>();
            foreach (ElementId id in ids)
            {
                Element elem = doc.GetElement(id);
                elems.Add(elem);
            }



            List<MyCategory> uniqCats = elems
                .Select(i => (BuiltInCategory)i.Category.Id.GetElementIdValue())
                .Distinct()
                .Select(i => new MyCategory(doc, i))
                .ToList();

            FormSetJoinOrder form = new FormSetJoinOrder(uniqCats);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return Result.Cancelled;

            Dictionary<BuiltInCategory, int> categoriesPriority =
                form.Cats.ToDictionary(i => i._category, j => j.priority);

            int counter = 0;

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionJoin);

                foreach (Element elem1 in elems)
                {
                    foreach (Element elem2 in elems)
                    {
                        if (elem2.Equals(elem1)) continue;

                        bool alreadyJoined = JoinGeometryUtils.AreElementsJoined(doc, elem1, elem2);
                        bool isIntersects = Tools.Model.Intersection.CheckElementsIsIntersect(doc, elem1, elem2);
                        if (!isIntersects && !alreadyJoined) continue;

                        try
                        {
                            JoinGeometryUtils.JoinGeometry(doc, elem1, elem2);
                        }
                        catch { }

                        bool isElemJoined = JoinGeometryUtils.AreElementsJoined(doc, elem1, elem2);
                        if (!isElemJoined) continue;

                        bool isFirstElemMain = JoinGeometryUtils.IsCuttingElementInJoin(doc, elem1, elem2);
                        int firstElemPriority = categoriesPriority[(BuiltInCategory)elem1.Category.Id.GetElementIdValue()];
                        int secondElemPriority = categoriesPriority[(BuiltInCategory)elem2.Category.Id.GetElementIdValue()];

                        if (isFirstElemMain && firstElemPriority > secondElemPriority)
                        {
                            JoinGeometryUtils.SwitchJoinOrder(doc, elem1, elem2);
                            counter++;
                        }
                    }
                }

                doc.Regenerate();



                t.Commit();
            }

            if (counter == 0)
            {
                message = MyStrings.MessageJoinPriorityNoJoin;
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }
    }
}
