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
using System;
#endregion


namespace ColumnsParametrisation
{
    public static class ParameterUtils
    {
        public static Parameter SuperGetParameter(Element elem, object paramDefinition)
        {
            Parameter param = GetParamGeneric(elem, paramDefinition);
            if (param == null || !param.HasValue)
            {
                ElementId typeId = elem.GetTypeId();
                if (typeId != null && typeId != ElementId.InvalidElementId)
                {
                    Element eltype = elem.Document.GetElement(elem.GetTypeId());
                    param = GetParamGeneric(eltype, paramDefinition);
                }
            }
            return param;
        }

        private static Parameter GetParamGeneric(Element elem, object paramDef)
        {
            Parameter param = null;
            if (paramDef is BuiltInParameter)
                param = elem.get_Parameter((BuiltInParameter)paramDef);
            else if (paramDef is Guid)
                param = elem.get_Parameter((Guid)paramDef);
            else if (paramDef is string)
                param = elem.LookupParameter((string)paramDef);
            return param;
        }

        public static string GetParameterValAsString(Element e, object paramDefinition)
        {
            Parameter param = SuperGetParameter(e, paramDefinition);
            string val = GetParameterValAsString(param);
            return val;
        }

        public static string GetParameterValAsString(Parameter param)
        {
            if (param == null || !param.HasValue) return string.Empty;

            string val = string.Empty;

            switch (param.StorageType)
            {
                case StorageType.None:
                    return string.Empty;
                case StorageType.Integer:
                    val = param.AsInteger().ToString();
                    break;
                case StorageType.Double:
                    double doubval = param.AsDouble();
#if R2017 || R2018 || R2019 || R2020
                    doubval = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.DisplayUnitType) / 10;
#else
                    doubval = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.GetUnitTypeId()) / 10;
#endif
                    val = doubval.ToString("F0");
                    break;
                case StorageType.String:
                    val = param.AsString();
                    break;
                case StorageType.ElementId:
                    //#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    val = Command.doc.GetElement(param.AsElementId()).Name;
                    //#else
                    //                    val = param.AsElementId().Value.ToString();
                    //#endif
                    break;
            }
            if (string.IsNullOrEmpty(val))
                return string.Empty;
            else
                return val;
        }
    }
}