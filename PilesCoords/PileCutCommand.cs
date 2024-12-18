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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace PilesCoords
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class PileCutCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("PileCut"));
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Tools.SettingsSaver.Saver<Settings> saver = new Tools.SettingsSaver.Saver<Settings>();
            Settings sets = saver.Activate(nameof(PilesCoords));

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Element> selems = sel.GetElementIds().Select(i => doc.GetElement(i)).ToList();

            List<FamilyInstance> piles = SupportGetter.GetPiles(selems, sets);
            List<Element> slabs = selems.Except(piles).ToList();

            if (piles.Count == 0)
            {
                message = "Выберите сваи.";
                Trace.WriteLine("Piles arent selected");
                return Result.Failed;
            }
            if (slabs.Count == 0)
            {
                message = "Выберите ростверк";
                Trace.WriteLine("Foundations are not selected");
                return Result.Failed;
            }


            using (var t = new Transaction(doc))
            {
                t.Start("Срубка свай под ростверк");
                foreach (Element pileElement in piles)
                {
                    FamilyInstance pile = pileElement as FamilyInstance;
                    if (pile == null) continue;
                    Trace.WriteLine($"Current pile id: {pileElement.Id}");

                    XYZ pileBottomPoint = MyPile.GetPileBottomPoint(pile);
                    XYZ pileTopPointBeforeCut = MyPile.GetPileTopPointBeforeCut(pile, sets);

                    //строю фиктивную линию для определения пересечения с плитой
                    XYZ p1 = new XYZ(pileTopPointBeforeCut.X, pileTopPointBeforeCut.Y, pileTopPointBeforeCut.Z - 3000 / 304.8);
                    XYZ p2 = new XYZ(pileTopPointBeforeCut.X, pileTopPointBeforeCut.Y, pileTopPointBeforeCut.Z + 3000 / 304.8);
                    Line slabLine = Line.CreateBound(p1, p2);

                    List<Element> slabsIntersectWithPile = new List<Element>();
                    List<XYZ> intersectPointsWithAllSlabs = new List<XYZ>();

                    foreach (Element slab in slabs)
                    {
                        List<XYZ> intersectPoints = Tools.Model.Intersection.CheckIntersectCurveAndElement(slabLine, slab);

                        if (intersectPoints.Count > 0)
                        {
                            intersectPointsWithAllSlabs.AddRange(intersectPoints);
                        }
                    }

                    if (intersectPointsWithAllSlabs.Count == 0)
                    {
                        Trace.WriteLine("No intersects with foundation");
                        continue;
                    }

                    XYZ slabBottomPoint = Tools.Model.Boundary.GetBottomPoint(intersectPointsWithAllSlabs);

                    double cutLength = pileTopPointBeforeCut.Z - slabBottomPoint.Z - (sets.pileDepth / 304.8);
                    Trace.WriteLine("Cut length: " + (cutLength * 304.8).ToString("F2"));
                    SupportGetter.GetParameter(pile, sets.paramPileCutHeigth, true).Set(cutLength);

                }
                t.Commit();
            }
            Trace.WriteLine("Success");
            return Result.Succeeded;
        }
    }
}
