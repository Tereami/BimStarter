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
using System.Linq;
#endregion

namespace AutoJoinCut
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class CommandBatchUnjoin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selids = sel.GetElementIds().ToList();

            if (selids.Count == 0)
            {
                message = MyStrings.ErrorNoSelectedElements;
                return Result.Failed;
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionUnjoin);
                foreach (ElementId selId in selids)
                {
                    Element elem = doc.GetElement(selId);
                    List<ElementId> ids = JoinGeometryUtils.GetJoinedElements(doc, elem).ToList();

                    foreach (ElementId id in ids)
                    {
                        Element elem2 = doc.GetElement(id);
                        JoinGeometryUtils.UnjoinGeometry(doc, elem, elem2);
                    }
                    doc.Regenerate();
                }
                t.Commit();
            }

            return Result.Succeeded;
        }
    }
}
