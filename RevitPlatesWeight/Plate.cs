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
Zuev Aleksandr, 2020, all rigths reserved.

More about solution / Подробнее: http://weandrevit.ru/plagin-massa-plastin-km/
*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using RVTDocument = Autodesk.Revit.DB.Document;
#endregion

namespace RevitPlatesWeight
{
    public abstract class Plate
    {
        public abstract bool isSubelement { get; }
        public abstract double Volume { get; }
        public abstract double Thickness { get; }
#if !R2019
        public abstract double Length { get; }
        public abstract double Width { get; }
#endif

        public abstract ElementId MaterialId { get; }


        double _mass;
        public double Mass { get { return _mass; } }

        public string ThicknessName { get { return _thicknessName; } }


        string _thicknessName;
        public string MaterialName;


        public void CalculateValues(RVTDocument doc, Settings sets)
        {
            double density = Tools.Model.MaterialUtils.GetMaterialDensity(doc, MaterialId);
            MaterialName = doc.GetElement(MaterialId).Name;

            _mass = Volume * density;
            _thicknessName = sets.platePrefix + (Thickness * 304.8).ToString("F0");
        }


        public void WriteValues(Settings sets, RVTDocument _doc)
        {
            WriteParameter(_doc, sets.GroupConstParamName, StorageType.Integer, sets.GroupConstParamValue, sets.Rewrite);
            WriteParameter(_doc, sets.ElementTypeParamName, StorageType.Integer, sets.ElementTypeParamValue, sets.Rewrite);

            WriteParameter(_doc, sets.ProfileNameParamName, StorageType.String, sets.ProfileNameValue, sets.Rewrite);
            WriteParameter(_doc, sets.ElementWeightTypeParamName, StorageType.Integer, sets.ElementWeightTypeValue, sets.Rewrite);

            WriteParameter(_doc, sets.PlateNameParamName, StorageType.String, _thicknessName, true);
            WriteParameter(_doc, sets.WeightParamName, StorageType.Double, Mass, true);
            WriteParameter(_doc, sets.MaterialNameParam, StorageType.ElementId, MaterialId, true);
            WriteParameter(_doc, sets.VolumeParamName, StorageType.Double, Volume, true);

            if (sets.writeThickvalue)
            {
                WriteParameter(_doc, sets.ThicknessParamName, StorageType.Double, Thickness, true);
            }
#if !R2019
            if (sets.writePlatesLengthWidth)
            {
                WriteParameter(_doc, sets.PlateLengthParamName, StorageType.Double, Length, true);
                WriteParameter(_doc, sets.PlateWidthParamName, StorageType.Double, Width, true);
            }
#endif
        }


        public string GetKey(bool enableNumbering, bool calculateLengthWidth)
        {
            if (!enableNumbering)
            {
                return "1";
            }
            string key = $"m{Mass.ToString("F2")} t{(Thickness * 304.8).ToString("F2")}";
#if !R2019
            if (calculateLengthWidth)
            {
                key += $"l{Length.ToString("F2")} b {Width.ToString("F2")}";
            }
#endif
            return key;
        }
        abstract public double GetDimension<T>(T plate, DimensionKind kind);
        abstract public void WriteParameter(RVTDocument doc, string paramName, StorageType stype, object Value, bool rewrite);

        public static BuiltInParameter GetParameterByKind(DimensionKind kind)
        {
            BuiltInParameter param = BuiltInParameter.STEEL_ELEM_PLATE_THICKNESS;

#if !R2019
            switch (kind)
            {
                case DimensionKind.Length:
                    param = BuiltInParameter.STEEL_ELEM_PLATE_LENGTH;
                    break;
                case DimensionKind.Width:
                    param = BuiltInParameter.STEEL_ELEM_PLATE_WIDTH;
                    break;
                case DimensionKind.Thickness:
                    param = BuiltInParameter.STEEL_ELEM_PLATE_THICKNESS;
                    break;
            }
#endif
            return param;
        }
    }
}
