using Autodesk.Revit.DB;
using System;


namespace FixSlowFile
{
    public class MyParameterValue
    {
        public MyProjectSharedParameter projectParam;
        public StorageType storageType;
        public bool IsValid = false;
        public bool IsNull = false;

        public string StringValue;
        public double DoubleValue;
        public int IntegerValue;
        public int ElementIdValue;

        public MyParameterValue(Parameter revitParam)
        {
            if (!revitParam.HasValue)
            {
                IsNull = true;
                return;
            }
            storageType = revitParam.StorageType;
            switch (storageType)
            {
                case StorageType.None:
                    break;
                case StorageType.Integer:
                    IntegerValue = revitParam.AsInteger();
                    IsValid = true;
                    break;
                case StorageType.Double:
                    DoubleValue = revitParam.AsDouble();
                    IsValid = true;
                    break;
                case StorageType.String:
                    StringValue = revitParam.AsString();
                    IsValid = true;
                    break;
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    ElementIdValue = revitParam.AsElementId().IntegerValue;
#else
                    ElementIdValue = (int)revitParam.AsElementId().Value;
#endif

                    IsValid = true;
                    break;
                default:
                    IsValid = false;
                    break;
            }
        }

        public override string ToString()
        {
            switch (storageType)
            {
                case StorageType.None:
                    return "none value";
                case StorageType.Integer:
                    return IntegerValue.ToString();
                case StorageType.Double:
                    return DoubleValue.ToString("F2");
                case StorageType.String:
                    return StringValue;
                case StorageType.ElementId:
                    return ElementIdValue.ToString();
                default:
                    throw new Exception("Invalid value for StorageType");
            }
        }

        public void SetValue(Parameter revitParam)
        {
            if (revitParam.IsReadOnly) return;
            switch (revitParam.StorageType)
            {
                case StorageType.None:
                    return;
                case StorageType.Integer:
                    revitParam.Set(IntegerValue);
                    return;

                case StorageType.Double:
                    revitParam.Set(DoubleValue);
                    return;

                case StorageType.String:
                    revitParam.Set(StringValue);
                    return;

                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    ElementId id = new ElementId(ElementIdValue);
#else
                    ElementId id = new ElementId((long)ElementIdValue);
#endif

                    revitParam.Set(id);
                    return;

                default:
                    throw new Exception("Invalid value for StorageType");
            }
        }


    }
}
