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
using System;
using System.Collections.Generic;
using System.IO;
#endregion

namespace ClearUnusedGUIDs
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandAddParamsByAnalog : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document famdoc = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            string oldSharedParamFilePath = app.SharedParametersFilename;

            if (!famdoc.IsFamilyDocument)
            {
                message = MyStrings.ErrorAvailableOnlyInFamilyEditor;
                return Result.Failed;
            }

            System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
            openDialog.Title = MyStrings.MessageSelectFamilyAnalog;
            openDialog.Multiselect = false;

            if (openDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return Result.Cancelled;

            string familyPath = openDialog.FileName;

            Document analogFamilyDoc = commandData.Application.Application.OpenDocumentFile(familyPath);
            ParametersInFamily pif = new ParametersInFamily(analogFamilyDoc);
            List<SharedParameterContainer> analogParams = pif.parameters;


            string familyFolder = Path.GetDirectoryName(familyPath);
            string familyTitle = analogFamilyDoc.Title.Remove(analogFamilyDoc.Title.Length - 4);
            string txtPath = Path.Combine(familyFolder, familyTitle + ".txt");
            FileStream fs = File.Create(txtPath);
            fs.Close();

            commandData.Application.Application.SharedParametersFilename = txtPath;
            DefinitionFile defFile = app.OpenSharedParameterFile();

            foreach (SharedParameterContainer spc in analogParams)
            {
                DefinitionGroup tempGroup = defFile.Groups.Create(spc.name);
                Definitions defs = tempGroup.Definitions;
                ExternalDefinitionCreationOptions defOptions = new ExternalDefinitionCreationOptions(spc.name,
#if R2017 || R2018 || R2019 || R2020 || R2021
                spc.intDefinition.ParameterType);
#else
                spc.intDefinition.GetDataType());
#endif
                defOptions.GUID = spc.guid;

                spc.exDefinition = defs.Create(defOptions) as ExternalDefinition;
            }

            FamilyManager fMan = famdoc.FamilyManager;

            int c = 0, ex = 0, er = 0;
            string errMsg = "";

            foreach (SharedParameterContainer spc in analogParams)
            {
                bool checkExists = ParametersInFamily.ParamIsExists(famdoc, spc);
                if (checkExists)
                {
                    ex++;
                    continue;
                }
                try
                {
                    using (Transaction t = new Transaction(famdoc))
                    {
                        t.Start($"{MyStrings.TransactionAddParameter} {spc.name}");
                        fMan.AddParameter(spc.exDefinition, spc.paramGroup, spc.isInstance);
                        t.Commit();
                    }
                    c++;
                }
                catch (Exception exc)
                {
                    er++;
                    errMsg = exc.Message;
                }

            }





            analogFamilyDoc.Close(false);
            System.IO.File.Delete(txtPath);
            app.SharedParametersFilename = oldSharedParamFilePath;

            string msg = $"{MyStrings.ResultParametersAdded}: {c}";
            if (ex > 0) msg += $"\n{MyStrings.ResultParamsAlreadyAdded}: " + ex;
            if (er > 0) msg += $"\n{MyStrings.ResultFailedToAdd}: {er}\n{errMsg}";
            TaskDialog.Show(MyStrings.TransactionAddParameter, msg);

            return Result.Succeeded;
        }
    }
}
