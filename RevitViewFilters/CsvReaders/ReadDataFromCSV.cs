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
#endregion

namespace RevitViewFilters
{
    public static class ReadDataFromCSV
    {
        public static List<string[]> Read(string filePath)
        {
            //int rowsCount = 0;
            int colsCount = 0;

            List<string[]> linesList = new List<string[]>();
            string[] lines = System.IO.File.ReadAllLines(filePath, Encoding.Default);

            colsCount = lines[0].Split(';').Length;
            string[] line = new string[colsCount];

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i][0] == '#')
                    continue;

                if (!string.IsNullOrEmpty(lines[i]))
                {
                    line = lines[i].Split(';');
                    linesList.Add(line);
                }
            }
            return linesList;
        }

    }
}
