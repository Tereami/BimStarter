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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace ClearUnusedGUIDs
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandClear : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("ClearGuids"));
            Debug.WriteLine("ClearGuid start");

            FormCheck form1 = new FormCheck();
            form1.ShowDialog();
            if (form1.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                return Result.Cancelled;
            }

            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<MyParameterDefinition> sparams = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(SharedParameterElement))
                .Cast<SharedParameterElement>()
                .Select(i => new MyParameterDefinition(i))
                .ToList();

            Debug.WriteLine("parameters found: " + sparams.Count.ToString());
            FormSelectGuids frm = new FormSelectGuids(sparams);

            if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return Result.Cancelled;
            }

            Debug.WriteLine("Parameters to delete: " + frm.selectedIds.Count.ToString());

            List<ElementId> ids = new List<ElementId>();
            foreach (int id in frm.selectedIds)
            {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                ids.Add(new ElementId(id));
#else
                ids.Add(new ElementId((long)id));
#endif
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionDeleteGuids);
                doc.Delete(ids);
                t.Commit();
            }

            Debug.WriteLine("Delete parameters is done");

            return Result.Succeeded;
        }

    }
}
