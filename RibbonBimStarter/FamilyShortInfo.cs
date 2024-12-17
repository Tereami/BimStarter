using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RibbonBimStarter
{
    [Serializable]
    public class FamilyShortInfo
    {
        public string id;
        public string name;
        public string guid;
        public string document;
        public string munufacturer;
        public string groupid;
        public string groupname;
        public string description;
        public string dateadd;
        public string revitversion;
        public string category;
        public string catshortname;
        public string host;
        public string hostshortname;

        public string GetFamilyName()
        {
            string name = BuildFamilyName(this.groupid, this.name, this.catshortname, this.hostshortname);
            return name;
        }

        public static string BuildFamilyName(string Groupid, string Name, string Catshortname, string Hostshortname)
        {
            string name = Groupid + "_" + Name + " (" + Catshortname + "_" + Hostshortname + ")";
            return name;
        }
    }
}
