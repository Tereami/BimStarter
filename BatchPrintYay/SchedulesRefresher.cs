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

namespace BatchPrintYay
{
    public static class SchedulesRefresher
    {
        public static List<long> groupIds = new List<long>();
        public static void Start(Document doc, View sheet)
        {
            List<ScheduleSheetInstance> ssis = new FilteredElementCollector(doc)
                .OfClass(typeof(ScheduleSheetInstance))
                .Cast<ScheduleSheetInstance>()
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                .Where(i => i.OwnerViewId.IntegerValue == sheet.Id.IntegerValue)
#else
                .Where(i => i.OwnerViewId.Value == sheet.Id.Value)
#endif
                .Where(i => !i.IsTitleblockRevisionSchedule)
                .ToList();

            List<ScheduleSheetInstance> pinnedSchedules = new List<ScheduleSheetInstance>();

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionSchedulesRefresh + " 1");

                foreach (ScheduleSheetInstance ssi in ssis)
                {
                    if (ssi.Pinned && (ssi.GroupId == null || ssi.GroupId == ElementId.InvalidElementId))
                    {
                        ssi.Pinned = false;
                        pinnedSchedules.Add(ssi);
                    }
                    MoveScheduleOrGroup(doc, ssi, 0.1);
                }

                t.Commit();
            }

            groupIds.Clear();

            using (Transaction t2 = new Transaction(doc))
            {
                t2.Start(MyStrings.TransactionSchedulesRefresh + " 2");

                foreach (ScheduleSheetInstance ssi in ssis)
                {
                    MoveScheduleOrGroup(doc, ssi, -0.1);
                }

                foreach (ScheduleSheetInstance ssi in pinnedSchedules)
                {
                    ssi.Pinned = true;
                }

                t2.Commit();
            }
        }

        private static void MoveScheduleOrGroup(Document doc, ScheduleSheetInstance ssi, double distance)
        {
            if (ssi.GroupId == null || ssi.GroupId == ElementId.InvalidElementId)
            {
                ElementTransformUtils.MoveElement(doc, ssi.Id, new XYZ(distance, 0, 0));
            }
            else
            {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                int groupId = ssi.GroupId.IntegerValue;
#else
                long groupId = ssi.GroupId.Value;
#endif
                Element group = doc.GetElement(ssi.GroupId);
                if (groupIds.Contains(groupId)) return;

                if (group.Pinned)
                {
                    group.Pinned = false;
                }
                ElementTransformUtils.MoveElement(doc, ssi.GroupId, new XYZ(distance, 0, 0));
                groupIds.Add(groupId);
            }
        }
    }
}
