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
using System;
#endregion

namespace PilesCoords
{
    [Serializable]
    public class Settings
    {
        public bool numberingUpDown = true;
        public int firstNumber = 1;

        public string pileFamilyName = "201_Свая прямоугольная (Фунд_Ур)";
        public double pileDepth = 50;

        public string paramPilePosition = "О_Позиция";
        public static string staticParamPilePosition = "О_Позиция";

        public string paramPileLength = "Рзм.Длина";
        public string paramPileLengthAfterCut = "Рзм.ДлинаБалкиИстинная";

        public string paramRange = "Орг.ДиапазонПозиций";
        public string paramRangeWithElevation = "Орг.ДиапазонПозиций2";

        public string paramSlabBottomElev = "Рзм.ОтметкаНизаПлиты";
        public string paramPileCutHeigth = "Высота срубаемой части";
        public string paramPlacementElevation = "Рзм.ОтметкаРасположения";
        public string paramPileTypeNumber = "N типа рядовой (1-10)";

        public bool sortByPileType_Table1 = true;
        public bool sortByPileUses_Table1 = true;
        public bool sortByBottomElev_Table1 = false;
        public bool sortByTopElev_Table1 = false;
        public bool sortByCutLength_Table1 = false;
        public bool sortBySlabElev_Table1 = false;

        public bool sortByPileType_Table2 = true;
        public bool sortByPileUses_Table2 = true;
        public bool sortByBottomElev_Table2 = true;
        public bool sortByTopElev_Table2 = true;
        public bool sortByCutLength_Table2 = true;
        public bool sortBySlabElev_Table2 = false;

        //private static string xmlPath = "";


    }
}
