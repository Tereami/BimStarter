#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ColumnsParametrisation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public static Document doc;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            FormMain formMain = new FormMain();
            if (formMain.ShowDialog() != System.Windows.Forms.DialogResult.OK) return Result.Cancelled;

            doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<ElementId> selids = sel.GetElementIds().ToList();
            if (selids.Count == 0)
            {
                TaskDialog.Show(MyStrings.Error, MyStrings.NoElementsSelected);
                return Result.Failed;
            }

            PhaseUtils.LoadPhases(doc, out ElementId phaseMainId, out ElementId phaseOneId);

            Dictionary<string, List<FamilyInstance>> famsDict = new Dictionary<string, List<FamilyInstance>>();

            int counter = 0;
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionName);
                foreach (ElementId selid in selids)
                {
                    FamilyInstance fi = doc.GetElement(selid) as FamilyInstance;
                    if (fi == null)
                    {
                        TaskDialog.Show(MyStrings.Error, MyStrings.NoElementsSelected);
                        return Result.Failed;
                    }

                    string allModelMark = fi.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                    if (string.IsNullOrEmpty(allModelMark))
                    {
                        TaskDialog.Show(MyStrings.Error, $"{MyStrings.ErrorMarkNotSet} {fi.GetElementId()}");
                        return Result.Failed;
                    }
                    Parameter markConstrParam = fi.get_Parameter(new Guid("5d369dfb-17a2-4ae2-a1a1-bdfc33ba7405")); //Мрк.МаркаКонструкции
                    if (markConstrParam == null || markConstrParam.IsReadOnly)
                    {
                        TaskDialog.Show(MyStrings.Error, $"{MyStrings.ErrorConstMarkNotSet} {fi.GetElementId()}");
                        return Result.Failed;
                    }
                    markConstrParam.Set(allModelMark);

                    List<string> markParts = new List<string>()
                    {
                        ParameterUtils.GetParameterValAsString(fi, new Guid("3aa2840f-b18e-4ca0-95bb-9784cc9b532f")), //Мрк.НаименованиеСоставноеПрефикс
                        ParameterUtils.GetParameterValAsString(fi, new Guid("8f2e4f93-9472-4941-a65d-0ac468fd6a5d")), //Рзм.Ширина
                        ParameterUtils.GetParameterValAsString(fi, new Guid("293f055d-6939-4611-87b7-9a50d0c1f50e")), //Рзм.Толщина
                        ParameterUtils.GetParameterValAsString(fi, new Guid("b62d0a35-0f0f-432d-9d3d-e821093a7d02")), //Рзм.ДлинаБалкиИстинная
                        ParameterUtils.GetParameterValAsString(fi, new Guid("048c9fcd-b9b1-45ca-9a56-27b5232a9403")), //Арм.МаркаТипаАрмирования
					};
                    string mark = string.Join(".", markParts);

                    string materialName = ParameterUtils.GetParameterValAsString(fi, BuiltInParameter.STRUCTURAL_MATERIAL_PARAM); //"Материал несущих конструкций"
                    mark += "." + materialName;


                    Parameter detailmarkParam = ParameterUtils.SuperGetParameter(fi, new Guid("92ae0425-031b-40a9-8904-023f7389963b")); //Мрк.МаркаИзделия
                    if (detailmarkParam == null || detailmarkParam.IsReadOnly)
                    {
                        throw new Exception(MyStrings.ErrorNoProductMark);
                    }

                    detailmarkParam.Set(mark);
                    fi.get_Parameter(BuiltInParameter.PHASE_CREATED).Set(phaseMainId);
                    counter++;

                    if (famsDict.ContainsKey(mark))
                        famsDict[mark].Add(fi);
                    else
                        famsDict.Add(mark, new List<FamilyInstance> { fi });
                }

                foreach (var kvp in famsDict)
                {
                    FamilyInstance firstFi = kvp.Value[0];
                    firstFi.get_Parameter(BuiltInParameter.PHASE_CREATED).Set(phaseOneId);
                }

                t.Commit();
            }

            Tools.Forms.BalloonTip.Show(MyStrings.TransactionName, $"{MyStrings.MessageResult}: {counter}");

            return Result.Succeeded;
        }
    }
}
