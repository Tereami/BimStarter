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
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace AutoJoinCut
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    class CommandCreateCope : IExternalCommand
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



            using (Transaction t = new Transaction(doc))
            {

                //проверяю пересечение каждой балки с каждой балкой
                foreach (FamilyInstance f1 in beams)
                {
                    t.Start(MyStrings.TransactionCope);
                    foreach (FamilyInstance f2 in beams)
                    {
                        if (f1.Id == f2.Id) continue;

                        try
                        {
                            var check = Tools.Geometry.Intersection.CheckElementsIntersection(doc, f1, f2);
                            if (check != Tools.Geometry.Intersection.MyIntersectionResult.Intersection) continue;

                            StructuralInstanceUsage su1 = f1.StructuralUsage;

                            int p1 = f1.LookupParameter("Приоритет врезки").AsInteger();
                            int p2 = f2.LookupParameter("Приоритет врезки").AsInteger();

                            if (p1 < p2)
                            {
                                f2.AddCoping(f1);
                            }
                            else
                            {
                                f1.AddCoping(f2);
                            }
                        }
                        catch { }
                    }
                    t.Commit();
                }
            }



            return Result.Succeeded;
        }
    }
}
