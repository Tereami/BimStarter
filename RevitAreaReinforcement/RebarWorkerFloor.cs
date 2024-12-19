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
using Autodesk.Revit.DB.Structure;
using System.Collections.Generic;
using System.Diagnostics;
using Tools.Geometry;
#endregion

namespace RevitAreaReinforcement
{
    public static class RebarWorkerFloor
    {

        public static List<string> Generate(Document doc, Floor floor, RebarInfoFloor rif, ElementId areaTypeId)
        {
            Trace.WriteLine("RebarWorkerFloor is started");
            List<string> messages = new List<string>();


            if (rif.SkipAlreadyReinforcedFloors)
            {
                bool checkFloorAlreadyHasReinforced = SupportDocumentGetter
                    .CheckElementHasReinforcement(floor, CommandCreateFloorRebar.allAreaReinforcements, rif.MinimumReinforcementCoeff);

                if (checkFloorAlreadyHasReinforced)
                {
                    string msg = $"Floor id {floor.Id} is already reinforced";
                    Trace.WriteLine(msg);
                    return messages;
                }
            }


            MyRebarType mrt = new MyRebarType(doc, rif.rebarTypeName);
            if (mrt.isValid == false)
            {
                messages.Add(MyStrings.ErrorFailedToGetRebarType + rif.rebarTypeName);
            }
            double interval = rif.interval;
            double topCoverUser = rif.topCover;
            double bottomCoverUser = rif.bottomCover;

            RebarCoverType coverTop = doc.GetElement(floor.get_Parameter(BuiltInParameter.CLEAR_COVER_TOP).AsElementId()) as RebarCoverType;
            RebarCoverType coverBottom = doc.GetElement(floor.get_Parameter(BuiltInParameter.CLEAR_COVER_BOTTOM).AsElementId()) as RebarCoverType;

            if (coverTop == null)
            {
                Trace.WriteLine("Top cover is null");
                coverTop = coverBottom;
            }
            if (coverBottom == null)
            {
                Trace.WriteLine("Bottom cover is null");
                coverBottom = coverTop;
            }

            Trace.WriteLine($"Rebar cover types id, top: {coverTop.Id}, bottom: {coverBottom.Id}");

#if R2017 || R2018 || R2019 || R2020 || R2021
            double diam = mrt.bartype.BarDiameter;
#else
            double diam = mrt.bartype.BarNominalDiameter;
#endif

            double topCoverDir1 = topCoverUser - coverTop.CoverDistance;
            double topCoverDir2 = topCoverDir1 + diam;
            if (rif.turnTopBars)
            {
                topCoverDir1 += diam;
                topCoverDir2 -= diam;
            }

            double bottomCoverDir1 = bottomCoverUser - coverBottom.CoverDistance;
            double bottomCoverDir2 = bottomCoverDir1 + diam;
            if (rif.turnBottomBars)
            {
                bottomCoverDir1 += diam;
                bottomCoverDir2 -= diam;
            }


            List<Curve> curves = Contour.GetFloorOuterBoundary(floor);
            Trace.WriteLine("Boundary curves count: " + curves.Count);

            XYZ direction = new XYZ(1, 0, 0);

            if (rif.useDirection)
            {
                double angle = floor.SpanDirectionAngle;
                Transform rotateTransform = Transform.CreateRotationAtPoint(new XYZ(0, 0, 1), angle, new XYZ(0, 0, 0));
                Line horizontal = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(1, 0, 0));
                Curve rotatedCurve = horizontal.CreateTransformed(rotateTransform);
                direction = rotatedCurve.GetEndPoint(1);
            }
            Trace.WriteLine("Direction: " + direction.ToString());

            AreaReinforcement arTopX = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(1);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_1_GENERIC).Set(interval);
            arTopX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_TOP_OFFSET).Set(topCoverDir1);
            arTopX.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set(MyStrings.TextTopXrebar);
            Trace.WriteLine("Top X is created");

            AreaReinforcement arTopY = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(1);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_TOP_DIR_2_GENERIC).Set(interval);
            arTopY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_TOP_OFFSET).Set(topCoverDir2);
            arTopY.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set(MyStrings.TextTopYrebar);
            Trace.WriteLine("Top Y is created");

            AreaReinforcement arBottomX = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(1);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(0);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_1_GENERIC).Set(interval);
            arBottomX.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_BOTTOM_OFFSET).Set(bottomCoverDir1);
            arBottomX.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set(MyStrings.TextBottomXrebar);
            Trace.WriteLine("Bottom X is created");

            AreaReinforcement arBottomY = AreaReinforcement
                .Create(doc, floor, curves, direction, areaTypeId, mrt.bartype.Id, ElementId.InvalidElementId);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_1_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_TOP_DIR_2_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_1_GENERIC).Set(0);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ACTIVE_BOTTOM_DIR_2_GENERIC).Set(1);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_SPACING_BOTTOM_DIR_2_GENERIC).Set(interval);
            arBottomY.get_Parameter(BuiltInParameter.REBAR_SYSTEM_ADDL_BOTTOM_OFFSET).Set(bottomCoverDir2);
            arBottomY.get_Parameter(BuiltInParameter.NUMBER_PARTITION_PARAM).Set(MyStrings.TextBottomYrebar);
            Trace.WriteLine("Bottom Y is created");

            return messages;
        }
    }
}
