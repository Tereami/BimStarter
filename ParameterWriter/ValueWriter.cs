using Autodesk.Revit.DB;
using Tools.Model.ParameterUtils;

namespace ParameterWriter
{
    public static class ValueWriter
    {
        public static bool SetValue(Element elem, WriterSettings sets)
        {
            bool result = false;
            Parameter targetParam = Tools.Model.ParameterUtils.Getter.GetParameter(elem, sets.targetParamName);
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
                    result = Tools.Model.ParameterUtils.Writer.SetValueConvertedFromString(targetParam, sets.ConstValue);
                    break;
                case SourceMode.OtherParameter:
                    Parameter sourceParam = Tools.Model.ParameterUtils.Getter.GetParameter(elem, sets.sourceParameterName);
                    result = Writer.WriteValueFromParamToParam(sourceParam, targetParam);
                    break;
                case SourceMode.Constructor:
                    result = Writer.SetValueByConstructor(sets.constructor, elem, targetParam);
                    break;
                case SourceMode.Level:
                    result = Writer.WriteValueFromLevel(targetParam, elem, sets.levelParamName);
                    break;
            }
            return result;
        }
    }
}
