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

using System;
using System.Diagnostics;

namespace RevitWorksets.WorksetWorkers
{
    [Serializable]
    public class WorksetByLink
    {
        public string separator = "_";
        public int partNumberAfterSeparator = 0;
        public int ignoreFirstCharsAfterSeparation = 0;
        public int ignoreLastCharsAfterSeparation = 0;
        public string prefixForLinkWorksets = "#";

        public string GetWorksetName(string filename)
        {
            if (string.IsNullOrEmpty(separator)) separator = "_";

            char separatorChar = separator[0];
            string[] arr = filename.Split(separatorChar);
            if (partNumberAfterSeparator >= arr.Length)
            {
                partNumberAfterSeparator = arr.Length - 1;
            }
            string linkWorksetName = arr[partNumberAfterSeparator];
            int sumIgnoreChars = ignoreFirstCharsAfterSeparation + ignoreLastCharsAfterSeparation;
            if (sumIgnoreChars < linkWorksetName.Length)
            {
                linkWorksetName = linkWorksetName
                    .Substring(ignoreFirstCharsAfterSeparation, linkWorksetName.Length - sumIgnoreChars);
            }
            linkWorksetName = prefixForLinkWorksets + linkWorksetName;
            Debug.WriteLine("Workset name: " + linkWorksetName);
            return linkWorksetName;
        }
    }
}
