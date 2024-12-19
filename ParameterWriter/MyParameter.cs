using Autodesk.Revit.DB;
using System;
using System.Linq;

namespace ParameterWriter
{
    public class MyParameter : IEquatable<MyParameter>
    {
        public Parameter RevitParameter { get; }
        public string Name { get; }
        public StorageType RevitStorageType { get; }

        private double doubleValue;
        private int intValue;
        private string stringValue;
        private ElementId elemIdValue;

        public bool HasValue;

        //Интерфейсы
        public bool Equals(MyParameter other)
        {
            switch (this.RevitStorageType)
            {
                case StorageType.None:
                    return false;
                case StorageType.Integer:
                    return this.AsInteger() == other.AsInteger();
                case StorageType.Double:
                    return Math.Round(this.AsDouble(), 6) == Math.Round(other.AsDouble(), 6);
                case StorageType.String:
                    return this.AsString() == other.AsString();
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    return this.AsElementId() == other.AsElementId();
#else
                    return this.AsElementId().Value == other.AsElementId().Value;
#endif
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return Name;
        }


        //Конструктор
        public MyParameter(Parameter revitParameter)
        {
            RevitParameter = revitParameter;
            RevitStorageType = revitParameter.StorageType;
            HasValue = false;
        }

        //Методы
        public void Set(object value)
        {
            switch (RevitStorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Integer:
                    intValue = (int)value;
                    HasValue = true;
                    break;
                case StorageType.Double:
                    doubleValue = (double)value;
                    HasValue = true;
                    break;
                case StorageType.String:
                    stringValue = (string)value;
                    HasValue = true;
                    break;
                case StorageType.ElementId:
                    elemIdValue = (ElementId)value;
                    HasValue = true;
                    break;
                default:
                    break;
            }
        }

        public double AsDouble()
        {
            switch (this.RevitStorageType)
            {
                case StorageType.None:
                    return 0.0;
                case StorageType.Integer:
                    return (double)intValue;
                case StorageType.Double:
                    return doubleValue;
                case StorageType.String:
                    return 0.0;
                case StorageType.ElementId:
                    return 0.0;
                default:
                    return 0.0;
            }
        }

        public int AsInteger()
        {
            switch (this.RevitStorageType)
            {
                case StorageType.None:
                    return 0;
                case StorageType.Integer:
                    return intValue;
                case StorageType.Double:
                    return (int)doubleValue;
                case StorageType.String:
                    return 0;
                case StorageType.ElementId:
                    return 0;
                default:
                    return 0; ;
            }
        }

        public string AsString()
        {
            switch (this.RevitStorageType)
            {
                case StorageType.None:
                    return "";
                case StorageType.Integer:
                    return "";
                case StorageType.Double:
                    return "";
                case StorageType.String:
                    return stringValue;
                case StorageType.ElementId:
                    return "";
                default:
                    return "";
            }
        }

        public ElementId AsElementId()
        {
            switch (this.RevitStorageType)
            {
                case StorageType.None:
                    return ElementId.InvalidElementId;
                case StorageType.Integer:
                    return ElementId.InvalidElementId;
                case StorageType.Double:
                    return ElementId.InvalidElementId;
                case StorageType.String:
                    return ElementId.InvalidElementId;
                case StorageType.ElementId:
                    return elemIdValue;
                default:
                    return ElementId.InvalidElementId;
            }
        }

        public string AsValueString()
        {
            switch (this.RevitStorageType)
            {
                case StorageType.None:
                    return "";
                case StorageType.Integer:
                    return intValue.ToString();
                case StorageType.Double:
                    return doubleValue.ToString();
                case StorageType.String:
                    return stringValue;
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    return elemIdValue.IntegerValue.ToString();
#else
                    return elemIdValue.Value.ToString();
#endif
                default:
                    return "";
            }
        }

        /// <summary>
        /// Запись в элемент значения параметра
        /// </summary>
        public static bool SetValue(Element elem, WriterSettings sets)
        {
            bool result = false;
            Parameter targetParam = MyParameter.SuperGetParameter(elem, sets.targetParamName);
            if (targetParam == null) return false;
            if (targetParam.IsReadOnly)
            {
                //string errmsg = "Параметр " + sets.targetParamName + " недоступен для записи";
                //System.Windows.Forms.MessageBox.Show(errmsg);
                //throw new Exception(errmsg);
                return false;
            }
            switch (sets.sourceMode)
            {
                case SourceMode.FixValue:
                    result = SetFixValue(targetParam, sets.ConstValue);
                    break;
                case SourceMode.OtherParameter:
                    Parameter sourceParam = MyParameter.SuperGetParameter(elem, sets.sourceParameterName);
                    result = SetValueByParam(sourceParam, targetParam);
                    break;
                case SourceMode.Constructor:
                    result = SetValueByConstructor(sets.constructor, elem, targetParam);
                    break;
                case SourceMode.Level:
                    result = SetValueFromLevel(targetParam, elem, sets.levelParamName);
                    break;
            }
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
                Autodesk.Revit.UI.TaskDialog.Show(MyStrings.Error, MyStrings.ConstructorOnlyText);
                throw new Exception(MyStrings.ConstructorOnlyText);
            }

            string prefix = constructor.Split('<').First();
            string result = prefix;

            string[] sa = constructor.Split('<');
            for (int i = 0; i < sa.Length; i++)
            {
                string s = sa[i];
                if (!s.Contains(">")) continue;

                string paramName = s.Split('>').First();
                string separator = s.Split('>').Last();

                string val = GetParameterValAsString(sourceElem, paramName);

                result = result + val + separator;
            }
            targetParam.Set(result);
            return true;
        }

        public static Parameter SuperGetParameter(Element Elem, string ParamName)
        {
            Parameter param = Elem.LookupParameter(ParamName);
            if (param == null)
            {
                ElementId typeId = Elem.GetTypeId();
                if (typeId != null && typeId != ElementId.InvalidElementId)
                {
                    Element eltype = Elem.Document.GetElement(typeId);
                    if (eltype != null)
                    {
                        param = eltype.LookupParameter(ParamName);
                    }
                }
            }
            return param;
        }




        public static string GetParameterValAsString(Element e, string paramName)
        {
            Parameter param = SuperGetParameter(e, paramName);
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



        public static bool SetValueByParam(Parameter sourceParam, Parameter targetParam)
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
                string val = GetParameterValAsString(sourceParam);
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
                string msg = $"{MyStrings.UnsopportedConvert} {enumSourceParamTypeName} - {enumTargetParamTypeName}";
                System.Windows.Forms.MessageBox.Show(msg);
                throw new Exception(msg);

            }
            result = true;
            return result;
        }

        public static bool SetFixValue(Parameter param, string value)
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

        public static bool SetValueFromLevel(Parameter targetParam, Element elem, string sourceParamName)
        {
            bool result = false;
            Level lev = Tools.Model.LevelUtils.GetLevelOfElement(elem);
            if (lev == null) return false;
            Parameter levelParam = SuperGetParameter(lev, sourceParamName);
            if (levelParam == null || !levelParam.HasValue) return false; ;

            result = SetValueByParam(levelParam, targetParam);
            return result;
        }
    }
}