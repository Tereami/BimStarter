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
#endregion

namespace RevitViewFilters
{
    public class FilterSourceInfo
    {

        private string name = "";
        public string FilterName
        {
            get { return name; }
        }

        private string[] cats;
        public string[] Categories
        {
            get { return cats; }
        }

        public int CountFilterRules
        {
            get { return sourceRules.Count; }
        }

        private List<string[]> sourceRules = new List<string[]>();
        public List<string[]> SourceRules
        {
            get { return sourceRules; }
        }


        public FilterSourceInfo(string[] Line)
        {
            int length = Line.Length;
            if (length != 11 && length != 8 && length != 5)
                return;

            name = Line[0];
            cats = Line[1].Split(',');

            for (int i = 2; i < Line.Length; i++)
            {
                string[] rule = new string[3];
                for (int j = 0; j < 3; j++)
                {
                    string check = Line[i];
                    if (string.IsNullOrEmpty(check)) return;
                    rule[j] = Line[i];
                    i++;
                }
                sourceRules.Add(rule);
                i--;
            }
        }

    }
}
