#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonBimStarter
{
    [Serializable]
    public class FamilyCard
    {
        public bool exists;
        public string guid;
        public FamilyShortInfo shortinfo;
        public List<FamilyShortInfo> nested;
        public List<FamilyShortInfo> parent;
        public List<FamilyVersion> versions;

        public FamilyVersion GetLastActualVersion()
        {
            List<FamilyVersion> okversions = GetOkVersions();
            int v = okversions.Max(a => a.version);
            FamilyVersion actualVers = okversions.Where(i => i.version == v).First();
            return actualVers;
        }

        public int GetLastActualVersionNumber()
        {
            List<FamilyVersion> okversions = GetOkVersions();
            int v = okversions.Max(a => a.version);
            return v;
        }

        public int GetLastVersionNumber()
        {
            int v = versions.Max(a => a.version);
            return v;
        }

        private List<FamilyVersion> GetOkVersions()
        {
            List<FamilyVersion> okversions = versions.Where(i => i.status == "ok").ToList();
            if (okversions.Count == 0)
            {
                return versions;
            }
            return okversions;
        }


    }
}
