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
using System;
using System.Linq;
#endregion

namespace RevitAreaReinforcement
{
    [Serializable]
    public class RebarInfoFloor
    {
        public string rebarTypeName;

        public double interval = 0.65616797900262469;
        public double topCover = 0.098425196850393692;
        public double bottomCover = 0.13123359580052493;
        public bool useDirection = false;
        public bool turnTopBars = false;
        public bool turnBottomBars = true;

        public bool SkipAlreadyReinforcedFloors = true;
        public double MinimumReinforcementCoeff = 150;

        public RebarInfoFloor()
        {

        }


        public static RebarInfoFloor GetDefault(Document doc)
        {
            RebarBarType bartype = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .First();
            string bartypename = bartype.Name;

            RebarInfoFloor info = new RebarInfoFloor();
            info.rebarTypeName = bartypename;


            return info;
        }
    }
}
