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

    class CommandBeamDisallowJoin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            if (sel.GetElementIds().Count == 0)
            {
                TaskDialog.Show(MyStrings.Error, MyStrings.ErrorSelectBeams);
                return Result.Cancelled;
            }

            List<FamilyInstance> beams = sel.GetElementIds()
                .Select(i => doc.GetElement(i))
                .Cast<FamilyInstance>()
                .ToList();

            int count = 0;
            int errCount = 0;

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionBeamUnjoin);
                foreach (FamilyInstance beam in beams)
                {
                    try
                    {
                        Autodesk.Revit.DB.Structure
                            .StructuralFramingUtils.DisallowJoinAtEnd(beam, 0);
                        Autodesk.Revit.DB.Structure
                            .StructuralFramingUtils.DisallowJoinAtEnd(beam, 1);

                        count++;
                    }
                    catch { errCount++; }
                }

                t.Commit();
            }

            TaskDialog.Show(MyStrings.TransactionBeamUnjoin, $"{MyStrings.ResultBeams1}: {count}; {MyStrings.ResultBeams2}: {errCount}");

            return Result.Succeeded;
        }
    }
}
