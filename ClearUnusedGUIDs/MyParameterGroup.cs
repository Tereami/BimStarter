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
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
#endregion

namespace ClearUnusedGUIDs
{
    public class MyParameterGroup
    {
        public string Name { get; set; }

#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
        public BuiltInParameterGroup Group { get; set; }
#else
        public ForgeTypeId Group { get; set; }
#endif
        public override string ToString()
        {
            return Name;
        }

        public static List<MyParameterGroup> GetGroupList()
        {
            List<MyParameterGroup> list = new List<MyParameterGroup>();
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            List<BuiltInParameterGroup> groups = Enum.GetValues(typeof(BuiltInParameterGroup)).Cast<BuiltInParameterGroup>().ToList();
            foreach (var group in groups)
            {
                string name = LabelUtils.GetLabelFor(group);
                MyParameterGroup mpg = new MyParameterGroup { Name = name, Group = group };
                list.Add(mpg);
            }
#else
            List<ForgeTypeId> groups = ParameterUtils.GetAllBuiltInGroups().ToList();
            foreach (var group in groups)
            {
                string name = LabelUtils.GetLabelForGroup(group);
                MyParameterGroup mpg = new MyParameterGroup { Name = name, Group = group };
                list.Add(mpg);
            }
#endif
            list = list.OrderBy(x => x.Name).ToList();
            return list;
        }
    }
}
