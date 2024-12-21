using Autodesk.Revit.DB;
using System;

namespace ParameterWriter
{
    public class ParameterContainer : IEquatable<ParameterContainer>
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
        public bool Equals(ParameterContainer other)
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
                    return this.AsElementId() == other.AsElementId();
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return Name;
        }


        //Конструктор
        public ParameterContainer(Parameter revitParameter)
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
    }
}