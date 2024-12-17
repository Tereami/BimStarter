using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonBimStarter
{
    public class FamilySuperGroupInfo
    {
        public static List<FamilySuperGroupInfo> SuperGroups;

        public int MinIndex;
        public int MaxIndex;
        public string Name;

        public FamilySuperGroupInfo(int minIndex, int maxIndex, string name)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Name = name;
        }


        public static void Activate()
        {
            SuperGroups = new List<FamilySuperGroupInfo>();

            SuperGroups.Add(new FamilySuperGroupInfo(0, 99, "2D"));
            SuperGroups.Add(new FamilySuperGroupInfo(100, 199, "АР"));
            SuperGroups.Add(new FamilySuperGroupInfo(200, 219, "1_ЖбКонстр"));
            SuperGroups.Add(new FamilySuperGroupInfo(220, 229, "2_Закладные"));
            SuperGroups.Add(new FamilySuperGroupInfo(230, 239, "3_Проемы"));
            SuperGroups.Add(new FamilySuperGroupInfo(240, 259, "4_Металл"));
            SuperGroups.Add(new FamilySuperGroupInfo(260, 260, "6_Арм формы"));
            SuperGroups.Add(new FamilySuperGroupInfo(261, 262, "6_Арм IFC"));
            SuperGroups.Add(new FamilySuperGroupInfo(263, 263, "6_Арм плит"));
            SuperGroups.Add(new FamilySuperGroupInfo(264, 264, "6_Арм стен"));
            SuperGroups.Add(new FamilySuperGroupInfo(265, 265, "6_Арм вып"));
            SuperGroups.Add(new FamilySuperGroupInfo(266, 279, "6_Арм каркас"));
            SuperGroups.Add(new FamilySuperGroupInfo(280, 299, "7_Узлы КМ"));
            SuperGroups.Add(new FamilySuperGroupInfo(300, 349, "9_КМ влож"));
            SuperGroups.Add(new FamilySuperGroupInfo(350, 399, "9_КЖ влож"));
            SuperGroups.Add(new FamilySuperGroupInfo(400, 999, "MEP"));
        }

        public static string GetSupergroupByIndex(string indexText)
        {
            int index = -1;
            if (!int.TryParse(indexText, out index)) return indexText;

            FamilySuperGroupInfo sg = SuperGroups.Find(i => i.MinIndex <= index && i.MaxIndex >= index);
            if (sg == null) return "null";

            return sg.Name;
        }
    }
}
