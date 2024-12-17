using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

namespace LinkWriter
{
    public class MyParameterValue
    {
        public StorageType storageType;
        public bool IsValid = false;
        public bool IsNull = false;

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

        public List<BuiltInParameter> skipParameters = new List<BuiltInParameter>
        {
            BuiltInParameter.SHEET_NUMBER,
            BuiltInParameter.SHEET_NAME,
            BuiltInParameter.VIEWER_SHEET_NUMBER,
        };

        public MyParameterValue(Parameter revitParam)
        {
            sourceParameter = revitParam;
            if (!revitParam.HasValue)
            {
                if (revitParam.StorageType == StorageType.String)
                {
                    StringValue = "";
                }
                else
                {
                    DoubleValue = 0;
                    IntegerValue = 0;
                    ElementIdValue = ElementId.InvalidElementId;
                    IsNull = true;
                }
                return;
            }
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
                if (skipParameters.Contains(BuiltInDefinition))
                {
                    IsValid = false;
                    IsNull = true;
                    return;
                }
            }

            Document doc = revitParam.Element.Document;

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