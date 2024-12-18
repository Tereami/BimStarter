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
using System;
using System.Collections.Generic;
using System.Diagnostics;
#endregion

namespace AutoJoin
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    class CommandAutoJoin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("AutoJoin"));
            UIApplication uiApp = commandData.Application;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;

            //Выбрать элементы для соединения
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            ICollection<ElementId> ids = sel.GetElementIds();
            Debug.WriteLine("Selected elems: " + ids.Count.ToString());

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

            bool isExecute = false;

            //каждый элемент в списке соединить с каждым элементом из списка
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionJoin);

                foreach (Element elem1 in elems)
                {
                    Debug.WriteLine("Join elem id " + elem1.Id.GetElementIdValue().ToString() + " with... ");
                    foreach (Element elem2 in elems)
                    {
                        //если этот тот же элемент - пропустить
                        if (elem2.Equals(elem1)) continue;

                        Debug.WriteLine(" id " + elem2.Id.GetElementIdValue().ToString());
                        //если элементы уже ранее соединены - пропустить
                        bool alreadyJoined = JoinGeometryUtils.AreElementsJoined(doc, elem1, elem2);
                        if (alreadyJoined)
                        {
                            Debug.WriteLine("Elements are already joined");
                            continue;
                        }

                        //проверить, пересекаются ли элементы, если не пересекаются - пропустить
                        bool isIntersects = Tools.Model.Intersection.CheckElementsIsIntersect(doc, elem1, elem2);
                        if (!isIntersects)
                        {
                            Debug.WriteLine("Elements dont have intersection");
                            continue;
                        }

                        //соединить элементы
                        try
                        {
                            JoinGeometryUtils.JoinGeometry(doc, elem1, elem2);
                            isExecute = true;
                            Debug.WriteLine("Joined succesfully!");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception: " + ex.Message);
                        }
                    }
                }

                t.Commit();
            }

            if (!isExecute)
            {
                message = MyStrings.MessageNoIntersectionNoJoin;
                return Result.Cancelled;
            }

            Debug.WriteLine("AutoJoin success");
            return Result.Succeeded;
        }
    }
}
