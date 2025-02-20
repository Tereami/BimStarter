using Autodesk.Revit.DB;
using System;

namespace LinkWriter
{
    public class MyParameterValue
    {
        public StorageType storageType;
        public bool IsValid = false;
        public bool IsNull = false;
        public bool IsEnabled = false;

        public string StringValue;
        public double DoubleValue;
        public int IntegerValue;

        public bool ValueAsFamily = false;
        public ElementId ElementIdValue;
        public Family ElemValueFamily;
        public Document ElemValueFamilyDocument;
        public string ElemValueFamilyName;
        public string ElemValueTypeName;

        public string ParameterName;

        public Parameter sourceParameter;
        public Guid SharedParameterGuid;
        public BuiltInParameter BuiltInDefinition;


        public MyParameterValue(Parameter revitParam)
        {
            if (revitParam.IsReadOnly)
            {
                IsValid = false;
                return;
            }

            sourceParameter = revitParam;

            ParameterName = revitParam.Definition.Name;
            storageType = revitParam.StorageType;

            InternalDefinition intDef = revitParam.Definition as InternalDefinition;
            if (revitParam.IsShared)
            {
                SharedParameterGuid = revitParam.GUID;
            }
            else
            {
                BuiltInDefinition = intDef.BuiltInParameter;
            }

            Document doc = revitParam.Element.Document;



            if (!revitParam.HasValue)
            {
                IsNull = true;
            }
            else
            {
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
                        IsValid = false;
                        break;
                        FamilySymbol fs = doc.GetElement(revitParam.AsElementId()) as FamilySymbol;
                        if (fs != null)
                        {
                            ElemValueFamily = fs.Family;
                            ElemValueTypeName = fs.Name;
                            ElemValueFamilyName = fs.FamilyName;
                        }
                        ElementIdValue = revitParam.AsElementId();
                        IsValid = true;
                        break;
                }
            }
        }

        public string GetValueAsString()
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

        public override string ToString()
        {
            return sourceParameter.Definition.Name;
        }

        public void SetValue(Parameter revitParam)
        {
            if (revitParam.IsReadOnly) return;

            if (IsNull && !revitParam.HasValue) return;

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
                    revitParam.Set(ElementIdValue);
                    return;

                default:
                    throw new Exception("Invalid value for StorageType");
            }
        }
    }
}