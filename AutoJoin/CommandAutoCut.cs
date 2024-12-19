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

    public class CommandAutoCut : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("AutoCut"));
            Debug.WriteLine("Start AutoCut");

            UIApplication uiApp = commandData.Application;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;

            //Выбрать пустотный элемент для вырезания
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            ICollection<ElementId> ids = sel.GetElementIds();
            Debug.WriteLine("Selected elements: " + ids.Count.ToString());

            if (ids.Count == 0)
            {
                message = MyStrings.ErrorNoSelectedElements;
                return Result.Failed;
            }
            if (ids.Count > 1)
            {
                message = MyStrings.ErrorSelectSingleElement;
                return Result.Failed;
            }

            Element voidElem = doc.GetElement(ids.First());

            //получаю список элементов, которые пересекает данный элемент
            List<Element> elems = Tools.Geometry.Intersection.GetAllIntersectionElements(doc, voidElem);
            Debug.WriteLine("Intersected elements: " + elems.Count.ToString());

            if (elems == null)
            {
                message = MyStrings.MessageNoIntersectionsNoCut;
            }

            //вырезаю элемент из данных элементов
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionCut);
                foreach (Element curElem in elems)
                {
                    Tools.Geometry.Intersection.CutElement(doc, curElem, voidElem);
                }
                t.Commit();
            }
            Debug.WriteLine("AutoCut completed");
            return Result.Succeeded;
        }
    }
}