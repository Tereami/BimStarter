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
using System;
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
            Trace.Listeners.Add(new Tools.Logger.Logger("LinkWriter"));
            Debug.WriteLine("Start CommandWriteLinkTitleblock");
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Debug.WriteLine($"Assembly version: {version}");
            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Document mainDoc = commandData.Application.ActiveUIDocument.Document;

            Tools.SettingsSaver.Saver<Save> saver = new Tools.SettingsSaver.Saver<Save>();
            Save save = saver.Activate("LinkWriter");

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
                Debug.WriteLine("Cancelled");
                return Result.Cancelled;
            }
            linkDocs = formSelectLinks.selectedLinks;
            List<string> linkNames = linkDocs.Select(x => x.Name).ToList();

            FormSelectParameterValues formValues = new FormSelectParameterValues(linkNames, valuesSettings);
            if (formValues.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine("Cancelled");
                return Result.Cancelled;
            }

            save.AddValues(formValues);
            saver.Save(save);
            Debug.WriteLine("Saved enabled parameters");

            foreach (MyRevitLinkDocument myLinkDoc in formSelectLinks.selectedLinks)
            {
                string linkName = myLinkDoc.Name;
                Debug.WriteLine($"Try to open a link: {linkName}");
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

                            if (formValues.ValuesSheets.ContainsKey(linkName))
                                WriteParameters(mySheet.sheet, formValues.ValuesSheets[linkName]);

                            if (formValues.ValuesTitleblocks.ContainsKey(linkName))
                                WriteParameters(linkTitleblockInstance, formValues.ValuesTitleblocks[linkName]);

                            if (formValues.ValuesTitleblockType.ContainsKey(linkName))
                                WriteParameters(linkTitleBlockType, formValues.ValuesTitleblockType[linkName]);

                            WriteParameters(linkDoc.ProjectInformation, formValues.ValuesProjectInfo);
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



        private void WriteParameters(Element elem, List<(string, string)> values)
        {
            Debug.WriteLine($"Write {values.Count} parameters to element {elem.Name} {elem.Id}");
            foreach (Parameter p in elem.ParametersMap)
            {
                if (p.IsReadOnly) continue;
                string paramName = p.Definition.Name;
                string valueString = values.FirstOrDefault(i => i.Item1 == paramName).Item2;
                if (valueString == null) continue;

                ParseAndSetValue(elem.Document, p, valueString);
            }
        }

        private void ParseAndSetValue(Document doc, Parameter p, string value)
        {
            switch (p.StorageType)
            {
                case StorageType.None:
                    return;
                case StorageType.Integer:
                    if (!int.TryParse(value, out int valueInt))
                        throw new Exception($"INTEGER PARSE ERROR UNABLE TO WRITE {value} INTO {p.Definition.Name}");
                    p.Set(valueInt);
                    return;
                case StorageType.Double:
                    if (!double.TryParse(value, out double valueDouble))
                        throw new Exception($"DOUBLE PARSE ERROR UNABLE TO WRITE DOUBLE {value} INTO {p.Definition.Name}");
                    p.Set(valueDouble);
                    return;
                case StorageType.String:
                    p.Set(value);
                    return;

                case StorageType.ElementId:
                    ElementId id = GetElementIdByName(doc, value);
                    p.Set(id);
                    return;

                default:
                    throw new Exception("Invalid value for StorageType");
            }
        }

        private ElementId GetElementIdByName(Document doc, string name)
        {
            Element elem = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .FirstOrDefault(i => i.Name == name);

            if (elem == null)
            {
                throw new Exception($"UNABLE TO FOUND ELEMENT {name} IN DOCUMENT {doc.Title}");
            }

            return elem.Id;
        }
    }
}
