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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace PilesCoords
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class PilesElevationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("PilesElevation"));

            Tools.SettingsSaver.Saver<Settings> saver = new Tools.SettingsSaver.Saver<Settings>();
            Settings sets = saver.Activate(nameof(PilesCoords));

            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Element> selems = sel.GetElementIds().Select(i => doc.GetElement(i)).ToList();

            List<FamilyInstance> piles = SupportGetter.GetPiles(selems, sets);
            List<Element> slabs = selems.Except(piles).ToList();
            Trace.WriteLine($"Piles count: {piles.Count} slabs count: {slabs.Count}");
            if (piles.Count == 0)
            {
                message = MyStrings.MessageSelectPiles;
                return Result.Failed;
            }
            if (slabs.Count == 0)
            {
                message = MyStrings.MessageSelectGrilliage;
                return Result.Failed;
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Отметки свай");
                foreach (FamilyInstance pile in piles)
                {
                    Trace.WriteLine($"Current pile id: {pile.Id}");
                    XYZ pileTopPointBeforeCut = MyPile.GetPileTopPointBeforeCut(pile, sets);

                    XYZ p1 = new XYZ(pileTopPointBeforeCut.X, pileTopPointBeforeCut.Y, pileTopPointBeforeCut.Z - 3000 / 304.8);
                    XYZ p2 = new XYZ(pileTopPointBeforeCut.X, pileTopPointBeforeCut.Y, pileTopPointBeforeCut.Z + 3000 / 304.8);
                    Line slabLine = Line.CreateBound(p1, p2);

                    List<Element> slabsIntersectWithPile = new List<Element>();
                    List<XYZ> intersectPointsWithAllSlabs = new List<XYZ>();

                    foreach (Element slab in slabs)
                    {
                        List<XYZ> intersectPoints = Tools.Geometry.Intersection.CheckIntersectCurveAndElement(slabLine, slab);

                        if (intersectPoints.Count > 0)
                        {
                            intersectPointsWithAllSlabs.AddRange(intersectPoints);
                        }
                    }

                    XYZ slabBottomPoint = Tools.Geometry.Height.GetBottomPoint(intersectPointsWithAllSlabs);
                    Trace.WriteLine("SlabBottomPoint Z = " + (slabBottomPoint.Z * 304.8).ToString("F1"));
                    Parameter elevParam = pile.LookupParameter(sets.paramSlabBottomElev);
                    if (elevParam == null)
                    {
                        TaskDialog.Show(MyStrings.Error, $"{MyStrings.ErrorNoParameter} {sets.paramSlabBottomElev}");
                        message = $"No parameter {sets.paramSlabBottomElev}";
                        return Result.Failed;
                    }
                    elevParam.Set(slabBottomPoint.Z);

                    try
                    {
                        XYZ pileBottomPoint = MyPile.GetPileBottomPoint(pile);
                        Trace.WriteLine("Pile bottom elevation: " + (pileBottomPoint.Z * 304.8).ToString("F2"));
                        Parameter pileElevParam = pile.LookupParameter(sets.paramPlacementElevation);
                        if (pileElevParam == null)
                        {
                            Trace.WriteLine("No parameter: " + sets.paramPlacementElevation);
                        }
                        else
                        {
                            pileElevParam.Set(pileBottomPoint.Z);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                }
                t.Commit();
            }
            Trace.WriteLine("Piles elevation succeded");
            return Result.Succeeded;
        }
    }
}