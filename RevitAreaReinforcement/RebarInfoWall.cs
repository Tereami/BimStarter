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
using System;
using System.Collections.Generic;
using System.Diagnostics;
#endregion

namespace RevitAreaReinforcement
{
    [Serializable]
    public class RebarInfoWall
    {
        public static Guid vertRebarDiameterParamGuid { get { return new Guid("ee3d34a4-e9e1-464a-9b9a-d034b0036a13"); } }
        public static Guid vertRebarIntervalParamGuid { get { return new Guid("a792ada0-6a07-48eb-86b6-8c0389a40419"); } }
        public static Guid vertRebarClassParamGuid { get { return new Guid("b2459150-dab4-4db4-97cb-592e92a15fe0"); } }
        public static Guid horizRebarDiameterParamGuid { get { return new Guid("1e273bec-8d8b-426c-bbd2-cea2ac71142b"); } }
        public static Guid horizRebarIntervalParamGuid { get { return new Guid("235e1a65-b813-4bc2-9325-d94476f793ef"); } }
        public static Guid horizRebarClassParamGuid { get { return new Guid("27814539-ea9c-450e-8fe5-961968ae65d1"); } }
        public static Guid rebarCoverDistanceParamGuid { get { return new Guid("16182862-7e1a-4253-a615-5a9bc4cbc268"); } }
        public static Guid rebarFreeLengthParamGuid { get { return new Guid("c353cc79-d7c0-47f4-94ab-79d08b734684"); } }

        public bool generateVertical = true;
        public string verticalSectionText = MyStrings.TextVerticalRebarPartition;
        public bool generateHorizontal = true;
        public string horizontalSectionText = MyStrings.TextHorizontalRebarPartition;

        public string verticalRebarTypeName = "12 A240";
        public double verticalRebarInterval = 0.65616797900262469;
        public string horizontalRebarTypeName = "12 A240";
        public double horizontalRebarInterval = 0.65616797900262469;
        public bool horizontalAddInterval = true;

        public bool horizontalIntervalIncreasedTopOrBottom = false;
        public double horizontalHeightIncreaseIntervalBottom = 0;
        public double horizontalHeightIncreaseIntervalTop = 0;

        public double verticalFreeLength = 2.1325459317585302;
        public bool autoVerticalFreeLength = false;
        public bool verticalAsymmOffset = false;
        public bool verticalAsymmOffsetTop = false;
        public double verticalAsymmManualLength = 0;
        public bool verticalRebarStretched = false;
        public double verticalFreeLengthRound = 0.16404199475065617;

        public double horizontalFreeLength = -0.0656167979; //0.590551181102362205;

        public double backOffset = 0.16404199475065617;
        public double bottomOffset = 0.16404199475065617;
        public double topOffset = 0.16404199475065617;


        public double rebarCover = 0.16404199475065617;

        public bool useUnification = false;
        public List<double> lengthsUnification = new List<double>();

        public bool SkipAlreadyReinforcedWalls = true;
        public double MinimumReinforcementCoeff = 150;

        /// <summary>
        /// Создание настроек армирования по-умолчанию
        /// </summary>
        public RebarInfoWall()
        {
        }

        /// <summary>
        /// Создание настроек армирования на основе информации из стены
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="wall"></param>
        public RebarInfoWall(Document doc, Wall wall)
        {
            Trace.WriteLine($"Start get info from wall id {wall.Id}");
            double verticalDiameter = GetParameter(vertRebarDiameterParamGuid, wall).AsDouble();
            double verticalClass = GetParameter(vertRebarClassParamGuid, wall).AsDouble();
            if (verticalDiameter == 0 || verticalClass == 0)
            {
                generateVertical = false;
                verticalRebarTypeName = null;
                Trace.WriteLine("Vertical rebar will not be created");
            }
            else
            {
                Trace.WriteLine("Vertical rebar will be created");
                generateVertical = true;
                MyRebarType newrebtype = new MyRebarType(doc, verticalDiameter, verticalClass, false);
                verticalRebarTypeName = newrebtype.bartype.Name;
                Trace.WriteLine("Vertical rebar typename: " + verticalRebarTypeName);
            }

            double horizontalDiameter = GetParameter(horizRebarDiameterParamGuid, wall).AsDouble();
            double horizontalClass = GetParameter(horizRebarClassParamGuid, wall).AsDouble();
            if (horizontalDiameter == 0 || horizontalClass == 0)
            {
                generateHorizontal = false;
                horizontalRebarTypeName = null;
                Trace.WriteLine("Horizontal rebar will not be created");
            }
            else
            {
                Trace.WriteLine("Horizontal rebar will be created");
                generateHorizontal = true;
                horizontalRebarTypeName = new MyRebarType(doc, horizontalDiameter, horizontalClass, true).bartype.Name;
                Trace.WriteLine("Horizontal rebar typename: " + horizontalRebarTypeName);
            }


            verticalRebarInterval = GetParameter(vertRebarIntervalParamGuid, wall).AsDouble();
            horizontalRebarInterval = GetParameter(horizRebarIntervalParamGuid, wall).AsDouble();
            Trace.WriteLine("Rebar interval: " + verticalRebarInterval.ToString("F3") + "x" + horizontalRebarInterval.ToString("F3"));

            try
            {
                verticalFreeLength = GetParameter(rebarFreeLengthParamGuid, wall).AsDouble();
                Trace.WriteLine("Vertical free length from parameter. L=" + verticalFreeLength.ToString("F3"));
            }
            catch
            {
                verticalFreeLength = 0;
                Trace.WriteLine("Vertical free length = 0");
            }

            rebarCover = GetParameter(rebarCoverDistanceParamGuid, wall).AsDouble();
            Trace.WriteLine("Rebar corver =" + rebarCover.ToString("F3"));

            //horizontalFreeLength = wall.Width - rebarCover;
            backOffset = bottomOffset;

            Parameter addStepHeightBottomParam = wall.LookupParameter("Арм.ВысотаУчащенияНиз");
            Parameter addStepHeightTopParam = wall.LookupParameter("Арм.ВысотаУчащенияВерх");
            if (addStepHeightBottomParam != null && addStepHeightTopParam != null)
            {
                if (addStepHeightBottomParam != null && addStepHeightBottomParam.HasValue)
                {
                    horizontalHeightIncreaseIntervalBottom = addStepHeightBottomParam.AsDouble();
                    horizontalIntervalIncreasedTopOrBottom = true;
                }
                else
                    horizontalHeightIncreaseIntervalBottom = 0;

                if (addStepHeightTopParam != null && addStepHeightTopParam.HasValue)
                {
                    horizontalHeightIncreaseIntervalTop = addStepHeightTopParam.AsDouble();
                    horizontalIntervalIncreasedTopOrBottom = true;
                }
                else
                    horizontalHeightIncreaseIntervalTop = 0;
            }
        }


        /*public static RebarInfoWall GetDefault(Document doc)
        {
            Trace.WriteLine("Create default rebar info");
            List<RebarBarType> bartypes = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .ToList();

            Trace.WriteLine("RebarBarTypes found: " + bartypes.Count.ToString());

            RebarBarType bartype = bartypes.First();
            string bartypename = bartype.Name;
            Trace.WriteLine("Selected bartype: " + bartypename);

            RebarInfoWall info = new RebarInfoWall();
            info.horizontalRebarTypeName = bartypename;
            info.verticalRebarTypeName = bartypename;
            info.lengthsUnification = new List<double> { 38.38582677165354, 25.59055118110236, 19.19291338582677, 12.79527559055118, 9.596456692913386, 7.677165354330709 };

            Trace.WriteLine("RebarInfo was created");
            return info;
        }*/


        private Parameter GetParameter(Guid guid, Element elem)
        {
            Parameter param = elem.get_Parameter(guid);
            if (param == null || !param.HasValue)
            {
                Trace.WriteLine($"Failed to get parameter {guid} from element id {elem.Id}");
                throw new Exception("Element " + elem.Id + " doesnt contain parameter " + guid.ToString());
            }
            return param;
        }

        public double getNearestLength(double l)
        {
            double distance = 99999;
            double result = 99999;
            foreach (double u in lengthsUnification)
            {
                double curDist = u - l;
                if (curDist >= 0 && curDist < distance)
                {
                    distance = curDist;
                    result = u;
                }
            }
            return result;
        }

        public void SetDefaultUnificateLengths()
        {
            lengthsUnification = new List<double>
            {
                38.385826771653541,
                25.590551181102363,
                19.19291338582677,
                12.795275590551181,
                9.5964566929133852,
                7.6771653543307083
            };
        }
    }
}
