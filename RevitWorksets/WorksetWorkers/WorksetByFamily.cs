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
using System.Collections.Generic;
using System.ComponentModel;

namespace RevitWorksets.WorksetWorkers
{
    [Serializable]
    public class WorksetByFamily : WorksetBy
    {
        public List<string> FamilyNames;

        [DisplayName("Prefix")]
        public string FamiliesText
        {
            get
            {
                if (FamilyNames == null || FamilyNames.Count == 0) return "None";
                string cats = string.Join(", ", FamilyNames);
                return cats;
            }
        }

        public WorksetByFamily()
        {

        }


        public static BindingList<WorksetByFamily> GetDefault()
        {
            BindingList<WorksetByFamily> worksets = new BindingList<WorksetByFamily>
            {
                new WorksetByFamily
                {
                    WorksetName = MyStrings.DefaultWorksetConcrete,
                    FamilyNames = new List<string> { "201_", "202_", "204_", "210_", "211_", "220_" }
                },
                new WorksetByFamily
                {
                    WorksetName = MyStrings.DefaultWorksetHydro,
                    FamilyNames = new List<string> { "222_" }
                },
                new WorksetByFamily
                {
                    WorksetName = MyStrings.DefaultWorksetRebar,
                    FamilyNames = new List<string> { "261_", "262_", "263_", "264_", "266_" }
                },
                new WorksetByFamily
                {
                    WorksetName = MyStrings.DefaultWorksetRebarFreelength,
                    FamilyNames = new List<string> { "265_" }
                }
            };
            return worksets;
        }
    }
}
