#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2026, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2026, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ParameterWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Model.ParameterUtils;
#endregion


namespace DrawingsGenerator
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public static string AnnotFamilyParamName = "_AnnotationFamilyName";
        public static string SheetNameConstructorParamName = "_SheetNameConstructor";
        public static int TitleblockId = 2336650;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection sel = uidoc.Selection;
            if (sel.GetElementIds().Count == 0)
            {
                TaskDialog.Show("Error", "Select families to create drawings");
                return Result.Failed;
            }

            ElementId titleblockTypeId = new ElementId(TitleblockId);


            List<FamilyInstance> famInstances = new List<FamilyInstance>();
            foreach (ElementId selid in sel.GetElementIds())
            {
                FamilyInstance fi = doc.GetElement(selid) as FamilyInstance;
                if (fi == null) continue;
                famInstances.Add(fi);
            }
            if (famInstances.Count == 0)
            {
                TaskDialog.Show("Error", "Select families to create drawings");
                return Result.Failed;
            }

            View sheetToOpen = doc.ActiveView;

            foreach (FamilyInstance fi in famInstances)
            {
                Dictionary<string, ParameterContainer> sourceParamsDict = GetParamsAsDict(fi, false);

                //sheet name
                if (!sourceParamsDict.ContainsKey(SheetNameConstructorParamName))
                    throw new Exception($"No parameter: {SheetNameConstructorParamName}");
                ParameterContainer sheetNameConstructorParam = sourceParamsDict[SheetNameConstructorParamName];
                string sheetNameConstructor = sheetNameConstructorParam.AsString();
                string sheetName = Tools.Model.ParameterUtils.Getter.GetByConstructor(fi, sheetNameConstructor, false);


                //annotation family
                ParameterContainer annotFamilyNameParam = sourceParamsDict[AnnotFamilyParamName];
                if (annotFamilyNameParam == null)
                    throw new Exception($"No parameter: {AnnotFamilyParamName}");
                if (!annotFamilyNameParam.RevitParameter.HasValue)
                    throw new Exception($"Empty value: {AnnotFamilyParamName}");

                string annotFamilyName = annotFamilyNameParam.AsString();

                Document famDoc = doc.EditFamily(fi.Symbol.Family);
                Family annotfamily = new FilteredElementCollector(famDoc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .FirstOrDefault(i => i.Name == annotFamilyName);
                if (annotfamily == null)
                    throw new Exception($"No family found: {annotFamilyName}");
                Document annotFamDoc = famDoc.EditFamily(annotfamily);

                using (Transaction t = new Transaction(doc))
                {
                    Family loadedAnnotFamily = annotFamDoc.LoadFamily(doc, new FamilyLoadOptions());
                    ElementId annotFamSymbolId = loadedAnnotFamily.GetFamilySymbolIds().FirstOrDefault();
                    FamilySymbol annotSymbol = doc.GetElement(annotFamSymbolId) as FamilySymbol;

                    t.Start("Annotation");
                    annotSymbol.Activate();

                    ViewSheet newSheet = ViewSheet.Create(doc, titleblockTypeId);
                    newSheet.Name = sheetName;
                    sheetToOpen = newSheet;

                    XYZ point = new XYZ(0, 0, 0);
                    FamilyInstance annotfamInstance = doc.Create.NewFamilyInstance(point, annotSymbol, newSheet);
                    WriteCommonParameters(sourceParamsDict, annotfamInstance);


                    //titleblock parameters
                    Dictionary<string, ParameterContainer> annotFamParams = GetParamsAsDict(annotfamInstance, false);
                    Dictionary<string, ParameterContainer> annotTitleblockParams = annotFamParams
                        .Where(i => i.Key.StartsWith("#Titleblock_"))
                        .ToDictionary(i => i.Key.Replace("#Titleblock_", ""), i => i.Value);
                    FamilyInstance titleblock = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .OfClass(typeof(FamilyInstance))
                            .Cast<FamilyInstance>()
                            .FirstOrDefault(i => i.OwnerViewId == newSheet.Id);
                    WriteCommonParameters(annotTitleblockParams, titleblock);

                    t.Commit();
                }

                famDoc.Close(false);
                annotFamDoc.Close(false);
            }

            uidoc.ActiveView = sheetToOpen;

            return Result.Succeeded;
        }

        private List<BuiltInParameter> ignoredParameters = new List<BuiltInParameter>()
        {
            BuiltInParameter.ELEM_CATEGORY_PARAM,
            BuiltInParameter.ELEM_CATEGORY_PARAM_MT,
            BuiltInParameter.SYMBOL_FAMILY_NAME_PARAM,
            BuiltInParameter.ELEM_FAMILY_PARAM,
            BuiltInParameter.ELEM_TYPE_PARAM,
            BuiltInParameter.ELEM_FAMILY_AND_TYPE_PARAM
        };

        private Dictionary<string, ParameterContainer> GetParamsAsDict(Element elem, bool getEmptyParams)
        {
            List<ParameterContainer> parametersList = Getter.GetAllElementParameters(elem, getEmptyParams);
            Dictionary<string, ParameterContainer> paramsDict = new Dictionary<string, ParameterContainer>();
            foreach (ParameterContainer parameter in parametersList)
            {
                if (paramsDict.ContainsKey(parameter.Name)) continue;

                InternalDefinition intDef = (InternalDefinition)parameter.RevitParameter.Definition;
                if (ignoredParameters.Contains(intDef.BuiltInParameter)) continue;

                paramsDict.Add(parameter.Name, parameter);
            }
            return paramsDict;
        }

        private void WriteCommonParameters(Dictionary<string, ParameterContainer> sourceData, Element target)
        {
            Dictionary<string, ParameterContainer> targetParameters = GetParamsAsDict(target, true);
            List<string> commonKeys = sourceData.Keys.Intersect(targetParameters.Keys).ToList();

            foreach (string paramName in commonKeys)
            {
                Parameter sourceParam = sourceData[paramName].RevitParameter;
                if (!sourceParam.HasValue) continue;
                Parameter targetParam = targetParameters[paramName].RevitParameter;
                if (targetParam.IsReadOnly) continue;
                Writer.WriteValueFromParamToParam(sourceParam, targetParam);
            }
        }
    }
}
