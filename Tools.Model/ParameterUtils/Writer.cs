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
using System;
using System.Linq;
#endregion

namespace Tools.Model.ParameterUtils
{
    public static class Writer
    {
        /// <summary>
        /// Записывает значение параметра вэлемент, если параметр присутствует и не заблокирован
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns>true если параметр успешно записан, иначе false</returns>
        public static bool TryWriteParameter(Element elem, string paramName, string value)
        {
            Parameter param = elem.LookupParameter(paramName);
            if (param != null)
            {
                if (!param.IsReadOnly)
                {
                    param.Set(value);
                    return true;
                }
            }
            return false;
        }

        public static bool WriteValueFromParamToParam(Parameter sourceParam, Parameter targetParam)
        {
            bool result = false;
            if (sourceParam == null) return false;
            if (!sourceParam.HasValue) return false;

            if (sourceParam.StorageType == targetParam.StorageType)
            {
                switch (targetParam.StorageType)
                {
                    case StorageType.None:
                        break;
                    case StorageType.Integer:
                        targetParam.Set(sourceParam.AsInteger());
                        break;
                    case StorageType.Double:
                        targetParam.Set(sourceParam.AsDouble());
                        break;
                    case StorageType.String:
                        targetParam.Set(sourceParam.AsString());
                        break;
                    case StorageType.ElementId:
                        targetParam.Set(sourceParam.AsElementId());
                        break;
                    default:
                        break;
                }
            }
            else if (targetParam.StorageType == StorageType.String)
            {
                string val = Tools.Model.ParameterUtils.Getter.GetParameterValAsString(sourceParam);
                targetParam.Set(val);
            }
            else if (targetParam.StorageType == StorageType.Double && sourceParam.StorageType == StorageType.Integer)
            {
                int val = sourceParam.AsInteger();
                double dval = (double)val;
                targetParam.Set(dval);
            }
            else
            {
                string enumSourceParamTypeName = Enum.GetName(typeof(StorageType), sourceParam.StorageType);
                string enumTargetParamTypeName = Enum.GetName(typeof(StorageType), targetParam.StorageType);
                string msg = $"Unsopported convert: {enumSourceParamTypeName} - {enumTargetParamTypeName}";
                System.Windows.Forms.MessageBox.Show(msg);
                throw new Exception(msg);

            }
            result = true;
            return result;
        }

        public static bool WriteValueFromLevel(Parameter targetParam, Element elem, string sourceParamName)
        {
            bool result = false;
            Level lev = Tools.Model.LevelUtils.GetLevelOfElement(elem);
            if (lev == null) return false;
            Parameter levelParam = Tools.Model.ParameterUtils.Getter.GetParameter(lev, sourceParamName);
            if (levelParam == null || !levelParam.HasValue) return false; ;

            result = WriteValueFromParamToParam(levelParam, targetParam);
            return result;
        }

        public static bool SetValueByConstructor(string constructor, Element sourceElem, Parameter targetParam)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021
            if (targetParam.Definition.ParameterType != ParameterType.Text)
#else
            ForgeTypeId ft = targetParam.Definition.GetDataType();
            if (ft != SpecTypeId.String.Text)

#endif
            {
                throw new Exception($"Failed to write {targetParam.Definition.Name}! Only text parameter allowed for Constructor!");
            }

            if (!constructor.Contains("<") || !constructor.Contains(">"))
            {
                throw new Exception($"Incorrect Constructor value {constructor}");
            }

            string prefix = constructor.Split('<')[0];
            string result = prefix;

            string[] sa = constructor.Split('<');
            for (int i = 0; i < sa.Length; i++)
            {
                string s = sa[i];
                if (!s.Contains(">")) continue;

                string paramName = s.Split('>').First();
                string separator = s.Split('>').Last();

                string val = Tools.Model.ParameterUtils.Getter.GetParameterValAsString(sourceElem, paramName);

                result = result + val + separator;
            }
            targetParam.Set(result);
            return true;
        }

        public static bool SetValueConvertedFromString(Parameter param, string value)
        {
            bool result = false;
            switch (param.StorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Integer:
                    param.Set(int.Parse(value));
                    break;
                case StorageType.Double:
                    double doubleValByUser = double.Parse(value);
#if R2017 || R2018 || R2019 || R2020
                    DisplayUnitType units = param.DisplayUnitType;
                    double doubleVal = UnitUtils.ConvertToInternalUnits(doubleValByUser, units);
#else
                    ForgeTypeId units = param.GetUnitTypeId();
                    double doubleVal = UnitUtils.ConvertToInternalUnits(doubleValByUser, units);
#endif
                    param.Set(doubleVal);
                    break;
                case StorageType.String:
                    param.Set(value);
                    break;
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    int intval = int.Parse(value);
                    ElementId newId = new ElementId(intval);
#else
                    long longval = int.Parse(value);
                    ElementId newId = new ElementId(longval);
#endif
                    param.Set(newId);
                    break;
                default:
                    break;
            }
            result = true;
            return result;
        }
    }
}
