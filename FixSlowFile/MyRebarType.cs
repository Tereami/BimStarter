using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace FixSlowFile
{
    public class MyRebarType
    {
        public string Name { get; set; }
        public RebarBarType bartype;
        public Dictionary<string, MyParameterValue> ValuesStorage = new Dictionary<string, MyParameterValue>();

        public MyRebarType(RebarBarType BarType)
        {
            Name = BarType.Name;
            bartype = BarType;

            foreach (Parameter param in BarType.ParametersMap)
            {
                string paramName = param.Definition.Name;
                MyParameterValue mpv = new MyParameterValue(param);
                ValuesStorage.Add(paramName, mpv);
            }
        }
    }
}
