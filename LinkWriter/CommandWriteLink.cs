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
using LinkWriter.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.LinksManager;
using Tools.Model.CategoryTools;
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

            FormSelectParameters formSelectParams = new FormSelectParameters(valuesSettings);
            if (formSelectParams.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return Result.Cancelled;
            }
            valuesSettings = formSelectParams.Settings;

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

            List<RevitCategory> allCategories = RevitCategory.LoadAllCategories(mainDoc);
            FormSelectParameterValues formValues = new FormSelectParameterValues(linkNames, valuesSettings, allCategories);
            if (formValues.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine("Cancelled");
                return Result.Cancelled;
            }

            save.SetSelectedParams(valuesSettings);
            save.SetCustomParams(formValues);
            saver.Save(save);
            Debug.WriteLine("Saved enabled parameters");
            int docsCount = 0;
            int paramsCount = 0;

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
                                paramsCount += WriteParameters(mySheet.sheet, formValues.ValuesSheets[linkName]);

                            if (formValues.ValuesTitleblocks.ContainsKey(linkName))
                                paramsCount += WriteParameters(linkTitleblockInstance, formValues.ValuesTitleblocks[linkName]);

                            if (formValues.ValuesTitleblockType.ContainsKey(linkName))
                                paramsCount += WriteParameters(linkTitleBlockType, formValues.ValuesTitleblockType[linkName]);

                            paramsCount += WriteParameters(linkDoc.ProjectInformation, formValues.ValuesProjectInfo);
                        }

                        if (formValues.ValuesCustomParameters.ContainsKey(linkName))
                        {
                            paramsCount += WriteParameters(linkDoc, formValues.ValuesCustomParameters[linkName]);
                        }

                        t.Commit();
                        docsCount++;
                    }
                    myLinkDoc.CloseDocument(true);
                }
                catch (System.Exception ex)
                {
                    myLinkDoc.CloseDocument(false);
                    throw ex;
                }
                //myLinkDoc.LinkType.Reload(); //перезагрузка ссылки уже происходит при myLinkDoc.CloseDocument
            }

            Tools.Forms.BalloonTip.Show("Link write completed", $"Write {paramsCount} parameters in {docsCount} documents");
            return Result.Succeeded;
        }



        private int WriteParameters(Element elem, List<NameAndValue> values)
        {
            int count = 0;
            Debug.WriteLine($"Write {values.Count} parameters to element {elem.Name} {elem.Id}");
            foreach (Parameter p in elem.ParametersMap)
            {
                if (p.IsReadOnly) continue;
                string paramName = p.Definition.Name;
                NameAndValue nav = values.FirstOrDefault(i => i.Name == paramName);
                if (nav == null) continue;
                string valueString = nav.Value;
                if (valueString == null) continue;

                ParseAndSetValue(elem.Document, p, valueString);
                count++;
            }
            return count;
        }

        private int WriteParameters(Document doc, List<NameValueCategories> values)
        {
            int count = 0;
            Debug.WriteLine($"Write {values.Count} parameters to a document {doc.Title}");

            foreach (var param in values)
            {
                string paramName = param.Name;
                string value = param.Value;
                List<BuiltInCategory> cats = param.Categories;
                Debug.WriteLine($"Write {paramName}={value} for categories: {string.Join(",", cats)}");

                List<Element> elements = new List<Element>();

                foreach (BuiltInCategory bic in cats)
                {
                    List<Element> elems = new FilteredElementCollector(doc)
                        .OfCategory(bic)
                        .WhereElementIsNotElementType()
                        .ToElements()
                        .ToList();

                    elements.AddRange(elems);
                }
                Debug.WriteLine($"Elements found: {elements.Count}");

                foreach (Element e in elements)
                {
                    Parameter p = e.LookupParameter(paramName);
                    ParseAndSetValue(doc, p, value);
                }
                Debug.WriteLine($"Done {paramName}");
            }

            return count;
        }

        private void ParseAndSetValue(Document doc, Parameter p, string value)
        {
            if (p == null) return;
            if (p.IsReadOnly) return;

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
