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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
#endregion

namespace RevitViewFilters
{
    public class MyParameter : IEquatable<MyParameter>, IComparable
    {
        public Parameter RevitParameter { get; }
        public ElementId ParameterId { get; }
        public string Name { get; }
        public StorageType RevitStorageType { get; }
        public List<ElementId> FilterableCategoriesIds { get; }

        private double doubleValue;
        private int intValue;
        private string stringValue;
        private ElementId elemIdValue;

        public bool HasValue;

        int IComparable.CompareTo(object obj)
        {
            string myval = this.AsValueString();
            string otherval = (obj as MyParameter).AsValueString();
            int compresult = myval.CompareTo(otherval);
            return compresult;
        }


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
                    return this.AsElementId().GetValue() == other.AsElementId().GetValue();
                default:
                    return false;
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 0;
                if (Name == null) return 0;
                hash = Name.GetHashCode();
                if (string.IsNullOrEmpty(AsValueString()))
                    return hash;
                else
                    return hash ^ AsValueString().GetHashCode();
            }
        }

        public override string ToString()
        {
            return Name;
        }


        //Конструкторы
        public MyParameter(Parameter revitParameter)
        {
            RevitParameter = revitParameter;
            Name = revitParameter.Definition.Name;
            RevitStorageType = revitParameter.StorageType;
            HasValue = false;

            if(!revitParameter.HasValue)
                return;

            switch (revitParameter.StorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Integer:
                    this.intValue = revitParameter.AsInteger();
                    HasValue = true;
                    break;
                case StorageType.Double:
                    this.doubleValue = revitParameter.AsDouble();
                    HasValue = true;
                    break;
                case StorageType.String:
                    string tempString = revitParameter.AsString();
                    if (string.IsNullOrEmpty(tempString)) break;
                    this.stringValue = tempString;
                    HasValue = true;
                    break;
                case StorageType.ElementId:
                    this.elemIdValue = revitParameter.AsElementId();
                    break;
                default:
                    break;
            }
        }

        public MyParameter(ElementId paramId, string name, List<ElementId> filterableCategoriesIds)
        {
            ParameterId = paramId;
            Name = name;
            FilterableCategoriesIds = filterableCategoriesIds;
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
                    double mm = ConvertFromInternal(doubleValue, this.RevitParameter);
                    return mm.ToString("F0");
                case StorageType.String:
                    return stringValue;
                case StorageType.ElementId:
                    return elemIdValue.GetValue().ToString();
                default:
                    return "";
            }
        }



        /*public static void SetValue(Parameter param, string value)
        {
            switch (param.StorageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Integer:
                    param.Set(int.Parse(value));
                    break;
                case StorageType.Double:
                    param.Set(double.Parse(value) / 304.8);
                    break;
                case StorageType.String:
                    param.Set(value);
                    break;
                case StorageType.ElementId:
                    int intval = int.Parse(value);
                    ElementId newId = new ElementId(intval);
                    param.Set(newId);
                    break;
                default:
                    break;
            }
        }*/

        /*public static string GetAsString(Parameter param)
        {
            switch (param.StorageType)
            {
                case StorageType.None:
                    return "";
                case StorageType.Integer:
                    return param.AsInteger().ToString();
                case StorageType.Double:
                    double doubleval = param.AsDouble();
                    double mm = ConvertFromInternal(doubleval, param);
                    return mm.ToString("F1");
                case StorageType.String:
                    return param.AsString();
                case StorageType.ElementId:
                    int intval = param.AsElementId().IntegerValue;
                    return intval.ToString();
                default:
                    return "";
            }
        }*/

        public static double ConvertFromInternal(double val, Parameter param)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021
            double val2 = UnitUtils.ConvertFromInternalUnits(val, param.DisplayUnitType);
#else
            ForgeTypeId forgeType = param.GetUnitTypeId();
            double val2 = UnitUtils.ConvertFromInternalUnits(val, forgeType);
#endif
            return val2;
        }
    }
}
