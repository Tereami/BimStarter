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
using System;
using System.Collections.Generic;
using System.Linq;

#endregion


namespace ParameterWriter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandWriteView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<View> views = doc.GetFiller3dViews();
            if (views.Count == 0)
            {
                TaskDialog.Show("Error", MyStrings.NoViewsFound);
                return Result.Failed;
            }

            Dictionary<long, Dictionary<string, string>> elemsAndParams = doc.GetElementParametersByViews(views);

            Dictionary<string, int> paramsCount = new Dictionary<string, int>();
            int elementsCount = 0;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Write by view");
                foreach (var kvp in elemsAndParams)
                {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    Element elem = doc.GetElement(new ElementId((int)kvp.Key)); //в версиях до 2024 можно приводить long к int
#else
                    Element elem = doc.GetElement(new ElementId(kvp.Key));
#endif
                    Dictionary<string, string> paramNameAndValues = kvp.Value;

                    foreach (var paramsAndValues in kvp.Value)
                    {
                        string paramName = paramsAndValues.Key;
                        string paramValue = paramsAndValues.Value;
                        Parameter elemParam = elem.LookupParameter(paramName);
                        if (elemParam == null) continue;
                        if (elemParam.IsReadOnly) continue;
                        bool success = false;
                        switch (elemParam.StorageType)
                        {
                            case StorageType.Integer:
                                int intValue = 0;
                                bool intCheck = int.TryParse(paramValue, out intValue);
                                if (!intCheck)
                                    throw new Exception($"Incorrect Int value {paramValue} for parameter {paramName}");
                                elemParam.Set(intValue);
                                success = true;
                                break;
                            case StorageType.Double:
                                double doubleValue = 0;
                                bool doubleCheck = double.TryParse(paramValue, out doubleValue);
                                if (!doubleCheck)
                                    throw new Exception($"Incorrect Double value {paramValue} for parameter {paramName}");
#if R2017 || R2018 || R2019 || R2020
                                double internalDouble = UnitUtils.ConvertToInternalUnits(doubleValue, elemParam.DisplayUnitType);
#else
                                double internalDouble = UnitUtils.ConvertToInternalUnits(doubleValue, elemParam.GetUnitTypeId());
#endif
                                elemParam.Set(internalDouble);
                                success = true;
                                break;
                            case StorageType.String:
                                elemParam.Set(paramValue);
                                success = true;
                                break;
                            default:
                                throw new Exception($"Unsoppurted parameter {paramName}");
                        }
                        if (success)
                        {
                            if (paramsCount.ContainsKey(paramName))
                                paramsCount[paramName]++;
                            else
                                paramsCount.Add(paramName, 1);
                        }
                    }
                    elementsCount++;
                }

                t.Commit();
            }

            string paramsMsg = MyStrings.ParametersWritten + System.Environment.NewLine +
                string.Join(System.Environment.NewLine, paramsCount.Select(i => $"{i.Key}: {i.Value}{MyStrings.Pieces}"));

            Tools.Forms.BalloonTip.Show(MyStrings.ElementsProcessed + elementsCount.ToString(), paramsMsg);
            return Result.Succeeded;
        }
    }
}
