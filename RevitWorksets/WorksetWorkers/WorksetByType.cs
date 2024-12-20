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
    public class WorksetByType : WorksetBy
    {
        public List<string> TypeNames;

        [DisplayName("Prefix")]
        public string TypesText
        {
            get
            {
                if (TypeNames == null || TypeNames.Count == 0) return "None";
                string cats = string.Join(" ", TypeNames);
                return cats;
            }
        }

        public WorksetByType()
        {

        }

        public static BindingList<WorksetByType> GetDefault()
        {
            BindingList<WorksetByType> list = new BindingList<WorksetByType>
            {
                new WorksetByType { WorksetName = MyStrings.DefaultWorksetArch, TypeNames = new List<string> { "100_", "101_", "102_", "103_" } },
                new WorksetByType { WorksetName = MyStrings.DefaultWorksetMep, TypeNames = new List<string> { "40", "50", "60", "70", "80", "90" } }
            };
            return list;
        }
    }
}
