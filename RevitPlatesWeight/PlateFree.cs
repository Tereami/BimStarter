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
#region Using
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Steel;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.Linq;
using RVTDocument = Autodesk.Revit.DB.Document;
#endregion

namespace RevitPlatesWeight
{
    public class PlateFree : Plate
    {
        SteelProxyElement _spe;

        public override bool isSubelement => false;

        double _volume;
        public override double Volume => _volume;

        double _thickness;
        public override double Thickness => _thickness;

        ElementId materialId;
        public override ElementId MaterialId => materialId;

#if !R2019
        double _length;
        public override double Length => _length;

        double _width;
        public override double Width => _width;
#endif

        public PlateFree(SteelProxyElement plateFree, View calculateView, Settings sets)
        {
            //Trace.WriteLine("Create PlateFree from steelproxy id" + plateFree.Id.IntegerValue.ToString());
            _spe = plateFree;
            materialId = _spe.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM).AsElementId();
            //Trace.WriteLine("Material Id: " + materialId.IntegerValue.ToString());
            Options opt = new Options() { View = calculateView };
            GeometryElement geoElem = _spe.get_Geometry(opt);
            if (geoElem == null)
            {
                string msg = $"{MyStrings.ErrorFailedToGetGeometry1} id {plateFree.Id}. {MyStrings.ErrorFailedToGetGeometry2}";
                System.Windows.Forms.MessageBox.Show(msg);
                Trace.WriteLine(msg);
                throw new Exception(msg);
            }

            GeometryInstance geoIns = geoElem.First() as GeometryInstance;
            GeometryElement geosol = geoIns.GetInstanceGeometry();
            Solid sol = geosol.First() as Solid;
            _volume = sol.Volume;
            _thickness = GetDimension(_spe, DimensionKind.Thickness);

#if !R2019
            if (sets.writePlatesLengthWidth)
            {
                _length = GetDimension(_spe, DimensionKind.Length);
                _width = GetDimension(_spe, DimensionKind.Width);
            }
#endif
        }

        public override double GetDimension<T>(T plate, DimensionKind kind)
        {
            BuiltInParameter param = Plate.GetParameterByKind(kind);
            SteelProxyElement plateFree = plate as SteelProxyElement;
            Parameter plateParam = plateFree.get_Parameter(param);
            if (plateParam == null || !plateParam.HasValue)
            {
                string msg = $"{MyStrings.ErrorNoParameter1} {Enum.GetName(typeof(DimensionKind), kind)} {MyStrings.ErrorNoParameter2} {plateFree.Id}";
                Trace.Write(msg);
                TaskDialog.Show(MyStrings.Error, msg);
                throw new Exception(msg);
            }
            double dim = plateParam.AsDouble();
            return dim;
        }

        public override void WriteParameter(RVTDocument doc, string paramName, StorageType stype, object Value, bool rewrite)
        {
            /*
#if !R2019
            BuiltInParameter weightBuilinParam = (BuiltInParameter)(-1155141);
            double weightTest = _spe.get_Parameter(weightBuilinParam).AsDouble();
#endif
            */

            Parameter param = _spe.LookupParameter(paramName);
            if (param == null)
            {
                string msg = $"{MyStrings.ErrorNoParameter1} {paramName}";
                Trace.WriteLine(msg);
                TaskDialog.Show(MyStrings.Error, msg);
                throw new Exception(msg);
            }
            if (param.HasValue && !rewrite) return;
            switch (stype)
            {
                case StorageType.Integer:
                    param.Set((int)Value);
                    break;
                case StorageType.Double:
                    param.Set((double)Value);
                    break;
                case StorageType.String:
                    param.Set((string)Value);
                    break;
                case StorageType.ElementId:
                    param.Set((ElementId)Value);
                    break;
            }
        }
    }
}
