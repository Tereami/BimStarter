#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.LinksManager;
#endregion


namespace LinkWriter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandWriteLink : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("BatchPrint"));
            Trace.WriteLine("Start CommandWriteLinkTitleblock");
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Trace.WriteLine($"Assembly version: {version}");
            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Document mainDoc = commandData.Application.ActiveUIDocument.Document;

            Tools.SettingsSaver.Saver<Save> saver = new Tools.SettingsSaver.Saver<Save>();
            Save save = saver.Activate("WriteLink");

            WriteLinkSettings valuesSettings = WriteLinkSettings.LoadAllValues(commandData, out message, save);
            if (valuesSettings == null)
            {
                return Result.Failed;
            }

            MyRevitMainDocument myMainDoc = new MyRevitMainDocument(mainDoc, true);

            List<MyRevitLinkDocument> linkDocs = myMainDoc.GetLinkDocuments();

            FormSelectLinks formSelectLinks = new FormSelectLinks(linkDocs, true);
            if (formSelectLinks.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled");
                return Result.Cancelled;
            }
            linkDocs = formSelectLinks.selectedLinks;
            List<string> linkNames = linkDocs.Select(x => x.Name).ToList();

            FormSelectParameterValues formValues = new FormSelectParameterValues(linkNames, valuesSettings);
            if (formValues.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled");
                return Result.Cancelled;
            }

            save.AddValues(formValues);
            saver.Save(save);

            return Result.Succeeded;

            foreach (MyRevitLinkDocument myLinkDoc in formSelectLinks.selectedLinks)
            {
                myLinkDoc.OpenLinkDocument(commandData, false);

                Document linkDoc = myLinkDoc.Doc;

                try
                {
                    using (Transaction t = new Transaction(linkDoc))
                    {
                        t.Start("Write parameters");

                        foreach (MySheet mySheet in myLinkDoc.Sheets)
                        {
                            if (mySheet.titleBlocks.Count != 1)
                            {
                                message = $"More that 1 titleblock on the sheet in the file {myLinkDoc.Name}.";
                                linkDoc.Close(false);
                                return Result.Failed;
                            }
                            FamilyInstance linkTitleblockInstance = mySheet.titleBlocks[0];
                            ElementType linkTitleBlockType = linkDoc.GetElement(linkTitleblockInstance.GetTypeId()) as ElementType;

                            WriteParameterValues(mySheet.sheet, valuesSettings.SheetParams);

                            WriteParameterValues(linkDoc.ProjectInformation, valuesSettings.ProjectParams);

                            WriteParameterValues(linkTitleblockInstance, valuesSettings.TitleblockParams);

                            WriteParameterValues(linkTitleBlockType, valuesSettings.TypeParams);
                        }
                        t.Commit();
                    }
                    myLinkDoc.CloseDocument(true);
                }
                catch (System.Exception ex)
                {
                    myLinkDoc.CloseDocument(false);
                    throw ex;
                }
                myLinkDoc.LinkType.Reload();
            }

            return Result.Succeeded;
        }


        private void WriteParameterValues(Element elem, List<MyParameterValue> values)
        {
            foreach (Parameter p in elem.ParametersMap)
            {
                if (p.IsReadOnly) continue;
                string paramName = p.Definition.Name;
                MyParameterValue sourceValue = values.FirstOrDefault(i => i.ParameterName == paramName);
                if (sourceValue == null) continue;
                sourceValue.SetValue(p);
            }
        }
    }
}
