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
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using RVTDocument = Autodesk.Revit.DB.Document;
#endregion

namespace RevitPlatesWeight
{
    public class PlateInJoint : Plate
    {
        Subelement _subelem;
        public override bool isSubelement => true;

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

        public PlateInJoint(Subelement subelem, RVTDocument doc, Settings sets)
        {
            _subelem = subelem;
            Reference rf = _subelem.GetReference();
            FilerObject filerObj = AdvanceSteelUtils.GetFilerObject(doc, rf);
            Autodesk.AdvanceSteel.Modelling.Plate pl = filerObj as Autodesk.AdvanceSteel.Modelling.Plate;
            //_volume = pl.Volume / (1000000 * 29.504);
            _volume = 1.0419239 * pl.Volume / (29504000);

            ParameterValue pv = _subelem.GetParameterValue(new ElementId(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM));
            ElementIdParameterValue idpv = pv as ElementIdParameterValue;
            materialId = idpv.Value;
            _thickness = GetDimension<Subelement>(_subelem, DimensionKind.Thickness);
#if !R2019
            if (sets.writePlatesLengthWidth)
            {
                _length = GetDimension(_subelem, DimensionKind.Length);
                _width = GetDimension(_subelem, DimensionKind.Width);
            }
#endif
        }


        public override double GetDimension<T>(T plate, DimensionKind kind)
        {
            BuiltInParameter param = Plate.GetParameterByKind(kind);
            Subelement plateAsSubelem = plate as Subelement;
            ParameterValue lengthParamValue = plateAsSubelem.GetParameterValue(new ElementId(param));
            if (lengthParamValue == null)
            {
                string msg = $"{MyStrings.ErrorNoParameter1} {Enum.GetName(typeof(DimensionKind), kind)} {MyStrings.ErrorNoParameter2} {plateAsSubelem.Element.Id}";
                TaskDialog.Show(MyStrings.Error, msg);
                throw new Exception(msg);
            }
            DoubleParameterValue lengthDoubleValue = lengthParamValue as DoubleParameterValue;
            return lengthDoubleValue.Value;
        }

        public override void WriteParameter(RVTDocument doc, string paramName, StorageType stype, object Value, bool rewrite)
        {
            ElementId paramId = null;
            List<ElementId> paramIds = _subelem.GetAllParameters().ToList();

            foreach (ElementId curParamId in paramIds)
            {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                int curParamIdValue = curParamId.IntegerValue;
#else
                long curParamIdValue = curParamId.Value;
#endif
                if (curParamIdValue > 0) //это общий параметр
                {
                    Element paramElement = doc.GetElement(curParamId);
                    if (paramElement.Name == paramName)
                    {
                        paramId = curParamId;
                        break;
                    }
                }
                else //это builtin параметр
                {
                    BuiltInParameter bip = (BuiltInParameter)curParamIdValue;
                    string builtinParamname = LabelUtils.GetLabelFor(bip);
                    if (builtinParamname == paramName)
                    {
                        paramId = curParamId;
                        break;
                    }
                }
            }
            /*
#if !R2019
            ElementId weightParamId = new ElementId(-1155141);
            DoubleParameterValue dpv = _subelem.GetParameterValue(weightParamId) as DoubleParameterValue;
            double weight = dpv.Value;
#endif
            */

            if (paramId == null)
            {
                string msg = $"{MyStrings.ErrorNoParameter1} {paramName}";
                TaskDialog.Show(MyStrings.Error, msg);
                throw new Exception(msg);
            }


            switch (stype)
            {
                case StorageType.Integer:
                    _subelem.SetParameterValue(paramId, new IntegerParameterValue((int)Value));
                    break;
                case StorageType.Double:
                    _subelem.SetParameterValue(paramId, new DoubleParameterValue((double)Value));
                    break;
                case StorageType.String:
                    _subelem.SetParameterValue(paramId, new StringParameterValue((string)Value));
                    break;
                case StorageType.ElementId:
                    _subelem.SetParameterValue(paramId, new ElementIdParameterValue((ElementId)Value));
                    break;
            }
        }
    }
}
