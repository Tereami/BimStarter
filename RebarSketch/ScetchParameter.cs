using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RebarSketch
{
    public class ScetchParameter : IEquatable<ScetchParameter>
    {
        public string Name = MyStrings.RebarAparam;
        public float PositionX = 200;
        public float PositionY = 100;
        public float Rotation = 0;

        public float FontSize = 55;
        public bool IsNarrow = false;
        public bool ShowVariableLengthInterval = true;
        public string Suffix = "";
        public string Prefix = "";

        public double LengthAccuracy = 5;
        //public double MinValueForRound = 20;

        [System.Xml.Serialization.XmlIgnore]
        public string value = "0000";

        [System.Xml.Serialization.XmlIgnore]
        public bool IsDegrees = false;
        [System.Xml.Serialization.XmlIgnore]
        public bool IsVariable = false;
        [System.Xml.Serialization.XmlIgnore]
        public bool HaveSpacing = false;
        [System.Xml.Serialization.XmlIgnore]
        public string SpacingValue;


        public ScetchParameter()
        {

        }

        public bool Equals(ScetchParameter other)
        {
            if (this.Name != other.Name) return false;
            if (this.value != other.value) return false;

            return true;
        }

        public void SetValue(List<double> values)
        {
            HashSet<double> roundValues = new HashSet<double>();
            foreach (double val in values)
            {
                double valueRound = LengthAccuracy * Math.Round(val / LengthAccuracy);
                roundValues.Add(valueRound);
            }

            int count = roundValues.Count();
            double minValue = roundValues.Min();
            double maxValue = roundValues.Max();

            if (minValue == maxValue || count == 1)
            {
                value = minValue.ToString("F0");
                IsVariable = false;
                HaveSpacing = false;
            }
            else
            {
                value = minValue.ToString("F0") + "..." + maxValue.ToString("F0");
                IsVariable = true;
                if (count > 2 && !IsDegrees && ShowVariableLengthInterval)
                {
                    HaveSpacing = true;
                    double spacing = (maxValue - minValue) / (count - 1);
                    double spacingRound = LengthAccuracy * Math.Round(spacing / LengthAccuracy);
                    SpacingValue = MyStrings.SpacingPrefix + spacing.ToString("F0");
                }
            }

            if (IsDegrees)
                value += "˚"; //"°";

            value = Prefix + value + Suffix;
        }
    }
}
