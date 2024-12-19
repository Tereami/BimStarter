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
    public class FilterDataForRebars : IFilterData
    {
        //const string rebarIsFamilyParamName = "Арм.ВыполненаСемейством";
        public Guid rebarIsFamilyParamGuid = new Guid("fc0665b7-63dd-44f2-8805-558177eccfb1");
        //const string rebarHostParamName = "Метка основы";
        //const string rebarMrkParamName = "Мрк.МаркаКонструкции";
        public Guid rebarHostMarkParamGuid = new Guid("5d369dfb-17a2-4ae2-a1a1-bdfc33ba7405");

        public Parameter rebarIsFamilyParam;
        public Parameter rebarHostParamBuiltin;
        public Parameter rebarHostParamShared;
        public Parameter markParam;

        public List<ElementId> catIdRebar;
        public List<ElementId> catIdConstructions;

        public List<string> values;

        public bool _rebarIsFamilyParamExists = false;

        public FilterDataForRebars(Document doc)
        {
            //define is it weandrevit template or not
            List<long> projParamsIds = new List<long>();
            DefinitionBindingMapIterator it = doc.ParameterBindings.ForwardIterator();
            it.Reset();
            while (it.MoveNext())
            {
                InternalDefinition def = it.Key as InternalDefinition;
                if (def == null) continue;
                projParamsIds.Add(def.Id.GetValue());
            }
            List<SharedParameterElement> spes = new FilteredElementCollector(doc)
                .OfClass(typeof(SharedParameterElement))
                .Cast<SharedParameterElement>()
                .ToList();
            foreach (SharedParameterElement spe in spes)
            {
                if (!projParamsIds.Contains(spe.Id.GetValue()))
                    continue;
                Guid curGuid = spe.GuidValue;
                if (curGuid.Equals(rebarIsFamilyParamGuid))
                {
                    _rebarIsFamilyParamExists = true;
                    break;
                }
            }
        }

        public int ValuesCount => values.Count;

        private string _filterNamePrefix = "_rh_";
        public string FilterNamePrefix => _filterNamePrefix;

        public bool ApplyFilters(Document doc, View v, ElementId fillPatternId, bool colorLines, bool colorFill)
        {
            Trace.WriteLine("Start apply rebar filters for view: " + v.Name);
            for (int i = 0; i < values.Count; i++)
            {
                string val = values[i];
                Trace.WriteLine("Value: " + val);

                ParameterFilterElement filterConstr = FilterCreator.CreateConstrFilter(doc, catIdConstructions, markParam, val, _filterNamePrefix);
                ViewUtils.ApplyViewFilter(doc, v, filterConstr, fillPatternId, i, colorLines, colorFill);

                if (_rebarIsFamilyParamExists)
                {
#if R2017 || R2018
                    ParameterFilterElement filterRebarStandardRebar = FilterCreator
                        .CreateRebarHostFilter(doc, catIdRebar, rebarIsFamilyParam, rebarHostParamBuiltin, rebarHostParamShared, val, _filterNamePrefix, RebarFilterMode.StandardRebarMode);
                    ViewUtils.ApplyViewFilter(doc, v, filterRebarStandardRebar, fillPatternId, i, colorLines, colorFill);
                    Trace.WriteLine("Filter created and applied to view: " + filterRebarStandardRebar.Name);

                    ParameterFilterElement filterRebarIfcRebar = FilterCreator
                        .CreateRebarHostFilter(doc, new List<ElementId> { new ElementId(BuiltInCategory.OST_Rebar) }, rebarIsFamilyParam, rebarHostParamBuiltin, rebarHostParamShared, val, _filterNamePrefix, RebarFilterMode.IfcMode);
                    ViewUtils.ApplyViewFilter(doc, v, filterRebarIfcRebar, fillPatternId, i, colorLines, colorFill);
                    Trace.WriteLine("Filter created and applied to view: " + filterRebarIfcRebar.Name);
#else
                    ParameterFilterElement filterRebarOrStyle = FilterCreator
                        .CreateRebarHostFilter(doc, catIdRebar, rebarIsFamilyParam, rebarHostParamBuiltin, rebarHostParamShared, val, _filterNamePrefix, RebarFilterMode.DoubleMode);
                    ViewUtils.ApplyViewFilter(doc, v, filterRebarOrStyle, fillPatternId, i, colorLines, colorFill);
                    Trace.WriteLine("Filter created and applied to view: " + filterRebarOrStyle.Name);
#endif
                }
                else
                {
                    ParameterFilterElement filterRebarSingleMode = FilterCreator
                        .CreateRebarHostFilter(doc, catIdRebar, rebarIsFamilyParam, rebarHostParamBuiltin, rebarHostParamShared, val, _filterNamePrefix, RebarFilterMode.SingleMode);
                    ViewUtils.ApplyViewFilter(doc, v, filterRebarSingleMode, fillPatternId, i, colorLines, colorFill);
                    continue;
                }
            }
            Trace.WriteLine("All filters applied");
            return true;
        }

        public MyDialogResult CollectValues(Document doc, View v)
        {
            Trace.WriteLine("Collect parameter values for view: " + v.Name);
            MyDialogResult result = new MyDialogResult(ResultType.ok, "");
            catIdRebar = new List<ElementId> {
                new ElementId(BuiltInCategory.OST_Rebar),
                new ElementId(BuiltInCategory.OST_AreaRein),
                new ElementId(BuiltInCategory.OST_PathRein)
            };

            catIdConstructions = new List<ElementId>{
                new ElementId(BuiltInCategory.OST_Walls),
                new ElementId(BuiltInCategory.OST_Floors),
                new ElementId(BuiltInCategory.OST_StructuralColumns),
                new ElementId(BuiltInCategory.OST_StructuralFraming),
                new ElementId(BuiltInCategory.OST_StructuralFoundation)
            };

            List<Element> rebars = new FilteredElementCollector(doc, v.Id)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Rebar)
                .ToElements()
                .ToList();
            Trace.WriteLine("Rebars count: " + rebars.Count);

            HashSet<string> hostMarksList = new HashSet<string>();

            foreach (Element e in rebars)
            {
                if (markParam == null)
                    markParam = e.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);
                
                Parameter curRebarHostParamBuiltin = e.get_Parameter(BuiltInParameter.REBAR_ELEM_HOST_MARK);
                if (curRebarHostParamBuiltin != null)
                {
                    if(rebarHostParamBuiltin == null)
                        rebarHostParamBuiltin = curRebarHostParamBuiltin;

                    if (curRebarHostParamBuiltin.HasValue)
                    {
                        string hostMarkVal = curRebarHostParamBuiltin.AsString();
                        hostMarksList.Add(hostMarkVal);
                    }
                }

                if (_rebarIsFamilyParamExists)
                {
                    if (rebarIsFamilyParam == null)
                    {
                        rebarIsFamilyParam = e.get_Parameter(rebarIsFamilyParamGuid);
                        if (rebarIsFamilyParam == null)
                        {
                            ElementType etype = doc.GetElement(e.GetTypeId()) as ElementType;
                            rebarIsFamilyParam = etype.get_Parameter(rebarIsFamilyParamGuid);
                        }
                    }

                    Parameter curRebarHostParamShared = e.get_Parameter(rebarHostMarkParamGuid);
                    if (curRebarHostParamShared != null)
                    {
                        if (rebarHostParamShared == null)
                            rebarHostParamShared = curRebarHostParamShared;

                        rebarHostParamShared = curRebarHostParamShared;
                        if(curRebarHostParamShared.HasValue)
                        {
                            string hostMarkVal = curRebarHostParamShared.AsString();
                            hostMarksList.Add(hostMarkVal);
                        }
                    }
                }
            }

            /*if (rebarIsFamilyParam == null)
            {
                result.ResultType = ResultType.warning;
                result.Message += "Не найден параметр Арм.ВыполненаСемейством. Фильтры будут созданы только по параметру Метка основы.";
            }*/

            values = hostMarksList.ToList();
            values.Sort();
            Trace.WriteLine("Collect values: " + values.Count);
            return result;
        }
    }
}