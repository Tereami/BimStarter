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
using RevitWorksets.WorksetWorkers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.Model.CategoryTools;
#endregion

namespace RevitWorksets
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger(nameof(RevitWorksets)));
            Document doc = commandData.Application.ActiveUIDocument.Document;

            if (!doc.IsWorkshared)
            {
                message = MyStrings.ErrorFileIsNotWorkshared;
                Debug.WriteLine("File is not workshared");
                return Result.Failed; ;
            }

            string xmlPath = "";
            InfosStorage storage = InfosStorage.Load(out xmlPath);

            List<RevitCategory> allCategories = RevitCategory.LoadAllCategories(doc);
            FormMain form = new FormMain(storage, xmlPath, allCategories);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Debug.WriteLine("CANCELLED");
                return Result.Cancelled;
            }
            storage = form.Model;
            xmlPath = form.XmlPath;

            List<WorksetElementsStorage> worksetsAndElements = new List<WorksetElementsStorage>();

            if (storage.WorksetByCategoryEnabled && storage.worksetsByCategory != null && storage.worksetsByCategory.Count > 0)
            {
                Debug.WriteLine("Start worksets by category");
                foreach (WorksetByCategory wb in storage.worksetsByCategory)
                {
                    Debug.WriteLine("Current workset: " + wb.WorksetName);
                    List<BuiltInCategory> cats = wb.revitCategories;
                    if (cats == null) continue;
                    if (cats.Count == 0) continue;

                    List<Element> worksetElements = new List<Element>();

                    foreach (BuiltInCategory bic in cats)
                    {
                        List<Element> elems = new FilteredElementCollector(doc)
                            .OfCategory(bic)
                            .WhereElementIsNotElementType()
                            .ToElements()
                            .ToList();

                        worksetElements.AddRange(elems);
                    }
                    Debug.WriteLine("Elements found:" + worksetElements.Count);
                    worksetsAndElements.Add(new WorksetElementsStorage(wb.WorksetName, worksetElements));
                }
                Debug.WriteLine("Finish worksets by category");
            }

            if (storage.WorksetByFamilyEnabled && storage.worksetsByFamily != null && storage.worksetsByFamily.Count > 0)
            {
                Debug.WriteLine("Start worksets by family names");
                List<FamilyInstance> famIns = new FilteredElementCollector(doc)
                        .WhereElementIsNotElementType()
                        .OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>()
                        .ToList();
                Debug.WriteLine("Family instances found: " + famIns.Count);
                foreach (WorksetByFamily wb in storage.worksetsByFamily)
                {
                    Debug.WriteLine("Current workset:" + wb.WorksetName);

                    List<string> famNamePrefixes = wb.FamilyNames;
                    if (famNamePrefixes == null) continue;
                    if (famNamePrefixes.Count == 0) continue;

                    List<Element> worksetElements = new List<Element>();

                    foreach (string familyName in famNamePrefixes)
                    {
                        List<FamilyInstance> curFamIns = famIns
                            .Where(f => f.Symbol.FamilyName.StartsWith(familyName))
                            .ToList();

                        worksetElements.AddRange(curFamIns);
                    }
                    Debug.WriteLine("Elements found:" + worksetElements.Count);
                    worksetsAndElements.Add(new WorksetElementsStorage(wb.WorksetName, worksetElements));
                }
                Debug.WriteLine("Finish worksets by family names");
            }

            Debug.WriteLine("Getting all model elements...");
            List<Element> allElems = new FilteredElementCollector(doc)
                            .WhereElementIsNotElementType()
                            .ToElements()
                            .ToList();
            Debug.WriteLine("Elements found: " + allElems.Count);

            if (storage.WorksetByTypeEnabled && storage.worksetsByType != null && storage.worksetsByType.Count > 0)
            {
                Debug.WriteLine("Start worksets by type names");
                foreach (WorksetByType wb in storage.worksetsByType)
                {
                    Debug.WriteLine("Current workset:" + wb.WorksetName);
                    List<string> typeNames = wb.TypeNames;
                    if (typeNames == null) continue;
                    if (typeNames.Count == 0) continue;

                    List<Element> worksetElements = new List<Element>();
                    foreach (string typeName in typeNames)
                    {
                        foreach (Element elem in allElems)
                        {
                            ElementId typeId = elem.GetTypeId();
                            if (typeId == null || typeId == ElementId.InvalidElementId) continue;
                            ElementType elemType = doc.GetElement(typeId) as ElementType;
                            if (elemType == null) continue;
                            Debug.WriteLine($"Element id: {elem.Id}, TypeName: {elemType.Name}");

                            if (elemType.Name.StartsWith(typeName))
                            {
                                worksetElements.Add(elem);
                            }
                        }
                    }
                    worksetsAndElements.Add(new WorksetElementsStorage(wb.WorksetName, worksetElements));
                    Debug.WriteLine("Elements found:" + worksetElements.Count);
                }
                Debug.WriteLine("Finish worksets by type names");
            }

            if (storage.worksetByParameter != null && storage.WorksetByParameterEnabled)
            {
                Debug.WriteLine("Start worksets by parameters");
                string paramName = storage.worksetByParameter.ParameterName;
                foreach (Element elem in allElems)
                {
                    Parameter p = elem.LookupParameter(paramName);
                    if (p == null) continue;
                    if (!p.HasValue) continue;
                    if (p.StorageType != StorageType.String)
                    {
                        string errmsg = "Only string parameter allowed: " + paramName;
                        Debug.WriteLine(errmsg);
                        throw new Exception(errmsg);
                    }
                    string wsetParamValue = p.AsString();

                    worksetsAndElements.Add(new WorksetElementsStorage(wsetParamValue, elem));

                }
                Debug.WriteLine("Finish worksets by parameter");
            }


            if (storage.worksetByLink != null && storage.WorksetByLinkEnabled)
            {
                Debug.WriteLine("Start worksets for link files");
                WorksetByLink wsetbylink = storage.worksetByLink;
                List<RevitLinkInstance> links = new FilteredElementCollector(doc)
                    .OfClass(typeof(RevitLinkInstance))
                    .Cast<RevitLinkInstance>()
                    .ToList();
                Debug.WriteLine("Links found: " + links.Count);

                foreach (RevitLinkInstance rli in links)
                {
                    Debug.WriteLine("Current link: " + rli.Name);
                    RevitLinkType linkFileType = doc.GetElement(rli.GetTypeId()) as RevitLinkType;
                    if (linkFileType == null)
                    {
                        Debug.WriteLine("LinkType is invalid");
                        continue;
                    }
                    if (linkFileType.IsNestedLink)
                    {
                        Debug.WriteLine("It is nested link");
                        continue;
                    }

                    string linkWorksetName = wsetbylink.GetWorksetName(linkFileType.Name);

                    List<Element> curElements = new List<Element>
                        {
                            rli,
                            linkFileType
                        };

                    worksetsAndElements.Add(new WorksetElementsStorage(linkWorksetName, curElements));

                }
                Debug.WriteLine("Finish worksets for link files");
            }

            if (storage.worksetByDwg != null && storage.WorksetByDwgEnabled)
            {
                Debug.WriteLine("Start workset for dwg links");
                WorksetByDwg wsetDwg = storage.worksetByDwg;
                List<ImportInstance> linkInstances = new FilteredElementCollector(doc)
                    .OfClass(typeof(ImportInstance))
                    .Cast<ImportInstance>()
                    .ToList();
                List<CADLinkType> linkTypes = new FilteredElementCollector(doc)
                    .OfClass(typeof(CADLinkType))
                    .Cast<CADLinkType>()
                    .ToList();

                List<Element> dwgElements = new List<Element>();
                dwgElements.AddRange(linkInstances);
                dwgElements.AddRange(linkTypes);

                worksetsAndElements.Add(new WorksetElementsStorage(wsetDwg.WorksetName, dwgElements));

                Debug.WriteLine("Finish workset for dwg links");
            }

            Dictionary<string, int> worksetNamesAndElementsCount = new Dictionary<string, int>();
            foreach (WorksetElementsStorage welems in worksetsAndElements)
            {
                string wsetName = welems.Name;
                int elemsCount = welems.Elements.Count;
                if (worksetNamesAndElementsCount.ContainsKey(wsetName))
                    worksetNamesAndElementsCount[wsetName] += elemsCount;
                else
                    worksetNamesAndElementsCount.Add(wsetName, elemsCount);
            }

            if (storage.NoEmptyWorksets)
            {
                Debug.WriteLine("Removing empty worksets");
                for (int i = 0; i < worksetsAndElements.Count; i++)
                {
                    WorksetElementsStorage welems = worksetsAndElements[i];
                    string name = welems.Name;
                    int elemsSumCount = worksetNamesAndElementsCount[name];
                    if (elemsSumCount == 0)
                    {
                        worksetNamesAndElementsCount.Remove(name);
                        worksetsAndElements.Remove(welems);
                        i--;
                        Debug.WriteLine("Workset is empty, removed: " + name);
                    }
                }
                Debug.WriteLine("Removing empty worksets - DONE");
            }

            IList<Workset> userWorksets = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToWorksets();
            int newWorksetsCreated = 0;
            int counter = 0;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create worksets");

                Dictionary<string, Workset> worksetsDict = new Dictionary<string, Workset>();

                foreach (var kvp in worksetNamesAndElementsCount)
                {
                    string name = kvp.Key;

                    bool checkNotExists = WorksetTable.IsWorksetNameUnique(doc, name);
                    if (checkNotExists)
                    {
                        Debug.WriteLine("Create workset: " + name);
                        Workset newWset = Workset.Create(doc, name);
                        worksetsDict.Add(name, newWset);
                        newWorksetsCreated++;
                    }
                    else
                    {
                        Debug.WriteLine("Workset exists: " + name);
                        Workset existedWset = userWorksets
                          .Where(w => w.Name == name)
                          .First();
                        worksetsDict.Add(name, existedWset);
                    }
                }

                doc.Regenerate();

                foreach (WorksetElementsStorage wsetElems in worksetsAndElements)
                {
                    string name = wsetElems.Name;
                    Workset wset = worksetsDict[name];
                    foreach (Element elem in wsetElems.Elements)
                    {
                        WorksetBy.SetWorkset(elem, wset);
                        counter++;
                    }
                }

                t.Commit();
            }

            string msg = $"{MyStrings.MessageElementsProcessed}: {counter}{Environment.NewLine}{MyStrings.MessageWorksetsCreated}: {newWorksetsCreated}";

            Tools.Forms.BalloonTip.Show(MyStrings.MessageFinished, msg);
            storage.Save(xmlPath);
            Debug.WriteLine(msg);
            return Result.Succeeded;
        }
    }
}
