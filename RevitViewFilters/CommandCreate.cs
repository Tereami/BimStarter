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
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace RevitViewFilters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    class CommandCreate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dllFolder = Path.GetDirectoryName(App.assemblyPath);
            string xmlFolder = Path.Combine(dllFolder, "RevitViewFilters_data");
            string ver = System.IO.File.GetLastWriteTime(App.assemblyPath).ToString();


            OpenFileDialog openCsvDialog = new OpenFileDialog();
            openCsvDialog.Filter = "CSV file|*.csv";
            openCsvDialog.Title = MyStrings.TitleSelectCsvFile + " ver. " + ver;
            openCsvDialog.Multiselect = false;
            openCsvDialog.InitialDirectory = xmlFolder;

            if (openCsvDialog.ShowDialog() != DialogResult.OK)
                return Result.Cancelled; ;

            //считываю файл
            string path = openCsvDialog.FileName;
            List<string[]> data = ReadDataFromCSV.Read(path);

            string msg = "";
            int filterCount = 0;

            CategoriesCollection catCol = new CategoriesCollection(doc);

            //одна строка в файле - один фильтр
            foreach (string[] line in data)
            {
                FilterSourceInfo filterSource = new FilterSourceInfo(line);
                string filterName = filterSource.FilterName;

                //Добавляю категории
                List<ElementId> catIds = new List<ElementId>();
                foreach (string stringCat in filterSource.Categories)
                {
                    BuiltInCategory cat = catCol.GetCategoryByName(stringCat);
                    catIds.Add(new ElementId(cat));
                }


                //добавляю критерии фильтрации
                List<FilterRule> filterRules = new List<FilterRule>();

                foreach (string[] sourceRule in filterSource.SourceRules)
                {
                    string paramName = sourceRule[0];
                    string function = sourceRule[1];
                    string value = sourceRule[2];

                    string catName = filterSource.Categories[0];
                    BuiltInCategory cat = catCol.GetCategoryByName(catName);
                    if (cat == BuiltInCategory.OST_Sections || cat == BuiltInCategory.OST_Elev || cat == BuiltInCategory.OST_Callouts)
                        cat = BuiltInCategory.OST_Views;

                    FilteredElementCollector collector = new FilteredElementCollector(doc).OfCategory(cat);

                    Parameter param = null;
                    try
                    {
                        foreach (Element elem in collector)
                        {
                            param = elem.LookupParameter(paramName);
                            if (param == null)
                                continue;
                            break;
                        }
                    }
                    catch { }

                    if (collector.Count() == 0 || param == null)
                    {
                        message = MyStrings.ErrorFilterCreation1 + filterName;
                        message += MyStrings.ErrorFilterCreation2 + filterSource.Categories[0];
                        message += MyStrings.ErrorFilterCreation3 + paramName;
                        return Result.Failed;
                    }

                    FilterRule rule = FilterCreator.CreateRule2(param, function, value);
                    filterRules.Add(rule);
                }

                try
                {
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start(MyStrings.TransactionCreateFilters + filterName);
                        ParameterFilterElement filter = ParameterFilterElement.Create(doc, filterName, catIds);
#if R2017 || R2018
                        
                        filter.SetRules(filterRules);
#else
                        ElementParameterFilter epf = new ElementParameterFilter(filterRules);
                        filter.SetElementFilter(epf);
#endif
                        filterCount++;
                        t.Commit();
                    }
                }
                catch
                {
                    msg += filterName + "\n";
                }

            }
            string finalMessage = MyStrings.MessageCreateFiltersSuccess + filterCount.ToString() + "\n";
            if (msg.Length != 0)
            {
                finalMessage += MyStrings.MessageCreateFiltersFailed + msg;
            }

            Autodesk.Revit.UI.TaskDialog.Show("Batch filter create", finalMessage);
            return Result.Succeeded;
        }
    }
}
