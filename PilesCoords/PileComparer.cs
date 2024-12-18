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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace PilesCoords
{
    class PileComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            Element elem1 = x as Element;
            string markString1 = elem1.LookupParameter(Settings.staticParamPilePosition).AsString();
            int mark1 = int.Parse(markString1);

            Element elem2 = y as Element;
            string markString2 = elem2.LookupParameter(Settings.staticParamPilePosition).AsString();
            int mark2 = int.Parse(markString2);


            if (mark1 > mark2)
                return 1;
            else if (mark1 < mark2)
                return -1;
            else
                return 0;
        }
    }
}
