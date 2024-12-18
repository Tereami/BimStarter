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
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion


namespace ParameterWriter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View curView = doc.ActiveView;
            //Dictionary<string, HashSet<object>> paramsbase = new Dictionary<string, HashSet<object>>();
            //HashSet<string> parameters = new HashSet<string>();

            List<ElementId> elemIds = new List<ElementId>();
            FilteredElementCollector col;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selids = sel.GetElementIds().ToList();
            bool haveSelectedElems = selids.Count > 0;

            if (haveSelectedElems)
            {
                elemIds = selids;
            }
            else
            {
                col = new FilteredElementCollector(doc, curView.Id)
                    .WhereElementIsNotElementType();
                elemIds = col
                    .ToElementIds()
                    .ToList();
            }

            //заполняю варианты значений для всех параметров
            Dictionary<string, HashSet<string>> valuesBase = new Dictionary<string, HashSet<string>>();
            foreach (ElementId id in elemIds)
            {
                Element elem = doc.GetElement(id);
                List<Parameter> allParams = new List<Parameter>();
                foreach (Parameter p in elem.Parameters)
                {
                    allParams.Add(p);
                }

                ElementId typeId = elem.GetTypeId();
                if (typeId == null) continue;
                if (typeId == ElementId.InvalidElementId) continue;
                ElementType elemType = doc.GetElement(typeId) as ElementType;
                if (elemType == null) continue;
                foreach (Parameter p in elemType.Parameters)
                {
                    allParams.Add(p);
                }

                foreach (Parameter p in allParams)
                {
                    string pname = p.Definition.Name;
                    string val = MyParameter.GetParameterValAsString(elem, pname);

                    if (valuesBase.ContainsKey(pname))
                        valuesBase[pname].Add(val);
                    else
                        valuesBase.Add(pname, new HashSet<string> { val });
                }
            }


            List<Element> levels = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Level))
                .ToElements()
                .ToList();
            if (levels.Count == 0)
                throw new Exception("NO LEVELS IN THIS MODEL!");
            Element level = levels[0];
            List<string> levelParametersNames = new List<string>();
            foreach (Parameter p in level.Parameters)
            {
                levelParametersNames.Add(p.Definition.Name);
            }



            //открываю диалоговое окно
            FormWriter form = new FormWriter(haveSelectedElems, valuesBase, levelParametersNames);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;
            List<WriterSettings> sets = form.allSettings;
            if (sets.Count == 0) return Result.Failed;
            //string sourceParamName = form.otherParamName;


            List<Element> elems = new List<Element>();
            if (form.writerMode == WriterMode.AllInProject)
            {
                elems = new FilteredElementCollector(doc)
                        .WhereElementIsNotElementType()
                        .ToElements()
                        .ToList();
            }
            else
            {
                foreach (ElementId id in elemIds)
                {
                    elems.Add(doc.GetElement(id));
                }
            }

            List<string> log = new List<string>();

            List<string> msgs = new List<string>();
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Writer");

                foreach (WriterSettings ws in sets)
                {
                    string curName = ws.targetParamName;
                    int curCount = 0;
                    if (form.ShowLog)
                        log.Add($"Write {curName}:");
                    foreach (Element elem in elems)
                    {
                        bool success = MyParameter.SetValue(elem, ws);
                        if (success)
                            curCount++;

                        if (form.ShowLog)
                            log.Add($"For element ID {elem.Id} {elem.Name}: {success} ");
                    }
                    msgs.Add(ws.targetParamName + MyStrings.WriteFor + curCount.ToString() + MyStrings.ForElements);
                }

                t.Commit();
            }

            string title = MyStrings.ScenariosProcessed + msgs.Count.ToString();
            string msg = string.Join(System.Environment.NewLine, msgs);

            if (form.ShowLog)
            {
                string logString = string.Join(System.Environment.NewLine, log);
                Tools.Forms.FormLog formLog = new Tools.Forms.FormLog(logString);
                formLog.ShowDialog();
            }

            Tools.Forms.BalloonTip.Show(title, msg);

            return Result.Succeeded;
        }
    }
}
