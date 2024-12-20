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
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace RevitWorksets.WorksetWorkers
{
    public abstract class WorksetBy
    {
        [DisplayName("Workset name")]
        public string WorksetName { get; set; }

        public Workset GetWorkset(Document doc)
        {
            Workset wset = GetOrCreateWorkset(doc, WorksetName);
            return wset;
        }

        public static Workset GetOrCreateWorkset(Document doc, string worksetName)
        {
            IList<Workset> userWorksets = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToWorksets();

            bool checkNotExists = WorksetTable.IsWorksetNameUnique(doc, worksetName);

            if (!checkNotExists)
            {
                Debug.WriteLine("Workset exists: " + worksetName);
                Workset wset = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToWorksets()
                .Where(w => w.Name == worksetName)
                .First();
                return wset;
            }
            else
            {
                Debug.WriteLine("Create workset: " + worksetName);
                Workset wset = Workset.Create(doc, worksetName);
                return wset;
            }
        }


        public static void SetWorkset(Element elem, Workset w)
        {
            Debug.WriteLine("Set workset: " + w.Name + " for elem id " + elem.Id.GetValue());

            bool elemNonGroup = elem.GroupId == null || elem.GroupId == ElementId.InvalidElementId;

            if (elemNonGroup)
            {
                Parameter wsparam = elem.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
                if (wsparam == null)
                {
                    Debug.WriteLine("Invalid workset parameter");
                    return;
                }
                if (wsparam.IsReadOnly)
                {
                    Debug.WriteLine("Workset parameter is readonly, skip");
                    return;
                }

                wsparam.Set(w.Id.IntegerValue);
                Debug.WriteLine("Set workset success");
            }
            else
            {
                Group gr = elem.Document.GetElement(elem.GroupId) as Group;
                Debug.WriteLine("Elem is in group; set workset for the group: " + gr.Name);
                SetWorkset(gr, w);
            }
        }
    }
}
