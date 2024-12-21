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
#endregion

namespace Tools.Model.ParameterUtils
{
    public static class Getter
    {
        /// <summary>
        /// Получает параметр из экземпляра или типа элемента
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        public static Parameter GetParameter(Element elem, string ParameterName)
        {
            Parameter param = elem.LookupParameter(ParameterName);
            if (param != null) return param;

            ElementId typeId = elem.GetTypeId();
            if (typeId == null) return null;

            Element type = elem.Document.GetElement(typeId);

            param = type.LookupParameter(ParameterName);
            return param;
        }

        public static string GetParameterValAsString(Element e, string paramName)
        {
            Parameter param = Tools.Model.ParameterUtils.Getter.GetParameter(e, paramName);
            if (param == null) return string.Empty;

            string val = GetParameterValAsString(param);
            return val;
        }


        public static string GetParameterValAsString(Parameter param)
        {
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
                    doubval = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.DisplayUnitType);
#else
                    doubval = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.GetUnitTypeId());
#endif
                    val = doubval.ToString("F2");
                    break;
                case StorageType.String:
                    val = param.AsString();
                    break;
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    val = param.AsElementId().IntegerValue.ToString();
#else
                    val = param.AsElementId().Value.ToString();
#endif
                    break;
            }
            if (string.IsNullOrEmpty(val))
                return string.Empty;
            else
                return val;
        }
    }
}
