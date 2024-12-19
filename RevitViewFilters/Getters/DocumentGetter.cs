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
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autodesk.Revit.DB;
#endregion

namespace RevitViewFilters
{
    public static class DocumentGetter
    {
        public static List<MyParameter> GetParameterValues(Document doc, List<Element> elems, string ParamName, int startSymbols)
        {
            bool isTypeParam = false;
            if (ParamName == "Имя типа" || ParamName == "Имя типа" || ParamName == "Type name" || ParamName == "Type Name")
            {
                isTypeParam = true;
            }

            HashSet<MyParameter> values = new HashSet<MyParameter>(); //список значений параметра
            foreach (Element elem in elems)
            {
                Parameter curParam = elem.LookupParameter(ParamName);

                if (curParam == null)
                {
                    ElementId typeElemId = elem.GetTypeId();
                    if (typeElemId == null) continue;
                    if (typeElemId == ElementId.InvalidElementId) continue;
                    Element typeElem = doc.GetElement(typeElemId);
                    curParam = typeElem.LookupParameter(ParamName);
                    if (curParam == null) continue;
                }

                MyParameter mp = new MyParameter(curParam);
                if (!mp.HasValue && !isTypeParam)
                {
                    continue;
                }
                if(isTypeParam)
                {
                    Parameter typeParam = elem.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM);
                    if (!typeParam.HasValue) continue;
                    string typeName = typeParam.AsValueString();
                    mp.Set(typeName);
                }

                if (startSymbols > 0)
                {
                    if (mp.RevitStorageType != StorageType.String)
                    {
                        throw new Exception(MyStrings.ErrorRuleBeginsWithOnlyForText);
                    }

                    string valTemp = mp.AsString();
                    string val = "";
                    if (valTemp.Length < startSymbols)
                        val = valTemp;
                    else
                        val = valTemp.Substring(0, startSymbols);

                    mp.Set(val);
                }
                values.Add(mp);
            }

            List<MyParameter> listParams = values.ToList();
            listParams.Sort();
            return listParams;
        }

        public static ElementId GetSolidFillPatternId(Document doc)
        {
            
            FillPatternElement fpe = new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement))
                .Cast<FillPatternElement>()
                .First(a => a.GetFillPattern().IsSolidFill);
            return fpe.Id;
        }

        public static ParameterFilterElement GetFilterByName(Document doc, string filterName)
        {
            List<ParameterFilterElement> filters = new FilteredElementCollector(doc)
                    .OfClass(typeof(ParameterFilterElement))
                    .Cast<ParameterFilterElement>()
                    .Where(f => f.Name == filterName)
                    .ToList();
            if (filters.Count == 0)
                return null;

            return filters.First();
        }
    }
}