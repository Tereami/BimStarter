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
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion


namespace RebarSketch
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandCreatePictures : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            App.ActivatePaths();
            Trace.WriteLine("Start rebar sketch, revit version" + commandData.Application.Application.VersionName);
            string dllVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Trace.WriteLine($"Assembly version: {dllVersion}");

            Document doc = commandData.Application.ActiveUIDocument.Document;

            Trace.WriteLine("Read settings");
            //считываем файл настроек
            GlobalSettings sets = GlobalSettings.Read();

            //выбираем арматуру, которую будем обрабатывать
            Autodesk.Revit.UI.Selection.Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selIds = sel.GetElementIds().ToList();
            //bool checkSelectionRebar = true;
            if (selIds.Count == 0)
            {
                message = MyStrings.MessageNoSelectedRows;
                Trace.WriteLine("No selected elements");
                return Result.Failed;
            }


            /*if (selIds.Count > 0)
            {
                ElementId selId = selIds.First();
                Element selElem = doc.GetElement(selId);
                if (selElem.Category.Id.IntegerValue != new ElementId(BuiltInCategory.OST_Rebar).IntegerValue)
                {
                    message = "Перед запуском перейдите в Ведомость деталей, выберите все строчки и после этого запускайте плагин.";
                    Trace.WriteLine("No selected rebar elements");
                    return Result.Failed;
                }
            }*/

            List<Element> col = new List<Element>();
            foreach (ElementId rebarId in selIds)
            {
                Element elem = doc.GetElement(rebarId);
                col.Add(elem);
            }

            View activeView = commandData.Application.ActiveUIDocument.ActiveView;
            ViewSchedule vs = activeView as ViewSchedule;
            if (vs == null)
            {
                message = MyStrings.MessageNoSelectedRows;
                Trace.WriteLine("Active view is not ViewSchedule");
                return Result.Failed;
            }

            //очищаю ранее созданные картинки для данной ведомости деталей
            string imagesPrefix = vs.GetElementId().ToString();
            List<ElementId> oldImageIds = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .OfClass(typeof(ImageType))
                .Where(i => i.Name.StartsWith(imagesPrefix))
                .Select(i => i.Id)
                .ToList();
            Trace.WriteLine("Old scetch images found: " + oldImageIds.Count.ToString());

            if (oldImageIds.Count > 0)
            {
                using (Transaction t1 = new Transaction(doc))
                {
                    t1.Start("Очистка");
                    doc.Delete(oldImageIds);
                    t1.Commit();
                }
            }

            ScetchLibrary lib = new ScetchLibrary();
            lib.Activate(App.libraryPath);
            if (lib.templates.Count == 0)
            {
                message = MyStrings.MessageEmptyLibrary;
                return Result.Failed;
            }
            List<XmlSketchItem> oldFormatTemplates = lib.templates.Where(i => i.IsXmlSource == false).ToList();
            if (oldFormatTemplates.Count > 0)
            {
                message = MyStrings.ErrorLibraryOldVersion;
                return Result.Failed;
            }


            System.IO.Directory.CreateDirectory(sets.tempPath);
            Trace.WriteLine("Create temp folder: " + sets.tempPath);


            //разделяю арматуру на обычную и переменной длины
            List<Element> standartRebars = new List<Element>();
            List<Element> variableRebars = new List<Element>();
            foreach (Element rebar in col)
            {
                if (!rebar.IsValidObject)
                    continue;
                int checkIsVariable = rebar.IsVariableLength();
                if (checkIsVariable == -1) continue;

                if (checkIsVariable == 0)
                    standartRebars.Add(rebar);
                else
                    variableRebars.Add(rebar);
            }

            Trace.WriteLine("Standart rebars: " + standartRebars.Count.ToString() + ", variable rebars: " + variableRebars.Count.ToString());


            //группировка арматуры переменной длины по марке
            Dictionary<string, List<Element>> variableRebarBase = new Dictionary<string, List<Element>>();
            foreach (Element vRebar in variableRebars)
            {
                string mark = vRebar.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                if (mark == null)
                {
                    Trace.WriteLine("Non-marked variable rebars is found");
                    TaskDialog.Show("Ошибка", MyStrings.ErrorRebarVariableLengthNoMark);
                    return Result.Failed;
                }

                if (variableRebarBase.ContainsKey(mark))
                    variableRebarBase[mark].Add(vRebar);
                else
                    variableRebarBase.Add(mark, new List<Element> { vRebar });
            }

            Dictionary<string, ScetchImage> imagesBase = new Dictionary<string, ScetchImage>();

            HashSet<string> errorRebarNames = new HashSet<string>();

            using (Transaction t2 = new Transaction(doc))
            {
                t2.Start(MyStrings.TransactionRebarSketch);

                //заполняю картинки для обычной арматуры
                foreach (Element rebar in standartRebars)
                {
                    string formName = rebar.GetRebarFormName();
                    if (formName == "") continue;

                    XmlSketchItem xsi = lib.FindTemplate(rebar);
                    if (xsi == null)
                    {
                        errorRebarNames.Add(formName);
                        continue;
                        //return Result.Failed;
                    }

                    foreach (ScetchParameter sparam in xsi.parameters)
                    {
                        string paramName = sparam.Name;
                        bool isDegress = false;
                        double val = rebar.GetDoubleValue(paramName, out isDegress);
                        sparam.IsDegrees = isDegress;
                        sparam.SetValue(new List<double> { val });

                        Trace.WriteLine("ScetchParameter name " + sparam.Name + " value = " + val.ToString("F0"));
                    }

                    ScetchLibrary.SearchAndApplyScetch(imagesBase, rebar, xsi, imagesPrefix, sets);
                }


                //заполняю картинки для арматуры переменной длины
                foreach (var kvp in variableRebarBase)
                {
                    string mark = kvp.Key;
                    List<Element> rebars = kvp.Value;

                    string formName = rebars.First().GetRebarFormName();
                    XmlSketchItem xsi = lib.FindTemplate(rebars.First());
                    if (xsi == null)
                    {
                        errorRebarNames.Add(formName);
                        continue;
                    }

                    //получаю для каждого имени параметра список его возможных значений
                    Dictionary<string, HashSet<double>> variableValues = new Dictionary<string, HashSet<double>>();
                    foreach (Element rebar in rebars)
                    {
                        foreach (ScetchParameter sparam in xsi.parameters)
                        {
                            string paramName = sparam.Name;
                            bool isDegrees = false;
                            double val = rebar.GetDoubleValue(paramName, out isDegrees);
                            sparam.IsDegrees = isDegrees;
                            if (variableValues.ContainsKey(paramName))
                            {
                                variableValues[paramName].Add(val);
                            }
                            else
                            {
                                variableValues.Add(paramName, new HashSet<double> { val });
                            }
                            Trace.WriteLine("Add variableValues " + paramName + " = " + val.ToString());
                        }
                    }

                    foreach (ScetchParameter sparam in xsi.parameters)
                    {
                        string paramName = sparam.Name;
                        List<double> values = variableValues[paramName].ToList();
                        sparam.SetValue(values);

                    }

                    foreach (Element rebar in rebars)
                    {
                        Trace.WriteLine($"Processed rebar id {rebar.GetElementId()}");
                        ScetchImage si = new ScetchImage(rebar, xsi);

                        ScetchLibrary.SearchAndApplyScetch(imagesBase, rebar, xsi, imagesPrefix, sets);
                    }
                }
                t2.Commit();
            }

            FileSupport.CheckAndDeleteFolder(sets.tempPath);

            if (errorRebarNames.Count > 0)
            {
                string errorFamilyMessage = MyStrings.ErrorNoFamiliesInLibrary;
                foreach (string fam in errorRebarNames)
                {
                    errorFamilyMessage = errorFamilyMessage + fam + "; ";
                }
                Trace.WriteLine(errorFamilyMessage);
                TaskDialog.Show(MyStrings.Report, errorFamilyMessage);
            }

            Trace.WriteLine("Scetches finish success");
            return Result.Succeeded;
        }
    }
}
