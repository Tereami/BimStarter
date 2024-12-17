using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace GroupedAssembly
{
    public static class AssemblyUtil
    {
        public static List<ElementId> GetAllNestedIds(Document doc, List<ElementId> selectedIds)
        {
            List<ElementId> ids2 = new List<ElementId>();

            foreach (ElementId id in selectedIds)
            {
                ids2.Add(id);
                Element elem = doc.GetElement(id);
                if (elem is FamilyInstance fi)
                {
                    List<FamilyInstance> nestedFamInstances = GetAllSharedNestedFamInstances(fi);
                    if (nestedFamInstances.Count == 0) continue;

                    List<ElementId> nestedFiIds = nestedFamInstances.Select(i => i.Id).ToList();
                    ids2.AddRange(nestedFiIds);
                }
                else if (elem is Group)
                {
                    Group existGroup = elem as Group;
                    ids2.AddRange(existGroup.GetMemberIds());
                }
            }

            return ids2.Count == 0 ? null : ids2;
        }


        private static List<FamilyInstance> GetAllSharedNestedFamInstances(FamilyInstance fi)
        {
            var nestedFams = new List<FamilyInstance>();

            var doc = fi.Document;

            var nestedIds = fi.GetSubComponentIds();

            foreach (var id in nestedIds)
            {
                if (!(doc.GetElement(id) is FamilyInstance nestedFi)) continue;

                var nestedFam = nestedFi.Symbol.Family;
                if (string.IsNullOrEmpty(nestedFam.Name)) continue;
                if (nestedFam.get_Parameter(BuiltInParameter.FAMILY_SHARED).AsInteger() != 1) continue;
                if (nestedFam.IsEditable != true) continue;

                nestedFams.Add(nestedFi);

                var nestedFams2 = GetAllSharedNestedFamInstances(nestedFi);
                nestedFams.AddRange(nestedFams2);
            }

            return nestedFams;
        }
    }
}