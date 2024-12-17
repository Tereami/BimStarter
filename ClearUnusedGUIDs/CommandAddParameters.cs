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
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ClearUnusedGUIDs
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandAddParameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            if (!doc.IsFamilyDocument)
            {
                message = MyStrings.ErrorAvailableOnlyInFamilyEditor;
                return Result.Failed;
            }

            Application app = commandData.Application.Application;

            DefinitionFile defFile = app.OpenSharedParameterFile();
            if (defFile == null)
            {
                message = MyStrings.ErrorNoSharedParamsFile;
                return Result.Failed;
            }


            DefinitionGroups defGroups = defFile.Groups;
            List<Definition> defs = new List<Definition>();

            foreach (DefinitionGroup defGroup in defGroups)
            {
                List<Definition> groupDefs = defGroup.Definitions.ToList();
                defs.AddRange(groupDefs);
            }

            List<MyExParamDefinition> myDefs = defs
                .Select(i => new MyExParamDefinition(i))
                .ToList();
            myDefs.Sort();

            var allGroups = MyParameterGroup.GetGroupList();

            FormAddParameters form = new FormAddParameters(myDefs, allGroups);

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            myDefs = form.defs;
            bool isInstance = form.isInstance;

            MyParameterGroup paramGroup = form.selectedGroup;

            FamilyManager fManager = doc.FamilyManager;

            int c = 0, e = 0;
            string errmsg = "";

            foreach (MyExParamDefinition myDef in myDefs)
            {
                try
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start($"{MyStrings.TransactionAddParameter}: {myDef.exDef.Name}");
                        ExternalDefinition exDef = myDef.exDef;
                        fManager.AddParameter(exDef, paramGroup.Group, isInstance);
                        t.Commit();
                    }
                    c++;
                }
                catch (Exception ex)
                {
                    e++;
                    errmsg = ex.Message;
                }
            }
            string msg = $"{MyStrings.ResultParametersAdded}: {c}";

            if (e > 0)
            {
                msg += $"\n:{MyStrings.ResultFailedToAdd} {e}\n{errmsg}";
            }

            TaskDialog.Show(MyStrings.TransactionAddParameter, msg);


            return Result.Succeeded;
        }
    }
}
