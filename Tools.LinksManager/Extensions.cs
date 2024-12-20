using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace Tools.LinksManager
{
    public static class Extensions
    {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023

        public static int GetElementId(this Element elem)
        {
            return elem.Id.IntegerValue;
        }
#else
        public static long GetElementId(this Element elem)
        {
            return elem.Id.Value;
        }
#endif


        public static List<RevitLinkInstance> DeleteDuplicates(List<RevitLinkInstance> links)
        {
            HashSet<string> names = new HashSet<string>();
            List<RevitLinkInstance> newLinks = new List<RevitLinkInstance>();

            foreach (RevitLinkInstance rli in links)
            {
                string docName = GetDocumentTitleFromLinkInstance(rli);
                if (names.Contains(docName)) continue;

                names.Add(docName);
                newLinks.Add(rli);
            }

            return newLinks;
        }


        public static string GetDocumentTitleFromLinkInstance(RevitLinkInstance rli)
        {
            string docName = rli.Name.Split(':').First();
            return docName;
        }
    }
}
