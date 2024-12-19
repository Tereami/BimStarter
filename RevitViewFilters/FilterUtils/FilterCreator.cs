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
#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace RevitViewFilters
{
    public static class FilterCreator
    {
        public static ParameterFilterElement createSimpleFilter(Document doc, List<ElementId> catsIds, string filterName, MyParameter mp, CriteriaType ctype)
        {
            List<ParameterFilterElement> filters = new FilteredElementCollector(doc)
                 .OfClass(typeof(ParameterFilterElement))
                 .Cast<ParameterFilterElement>()
                 .ToList();

            FilterRule rule = null;
            List<ParameterFilterElement> checkFilters = filters.Where(f => f.Name == filterName).ToList();
            ParameterFilterElement filter = null;
            if (checkFilters.Count != 0)
            {
                filter = checkFilters[0];
            }
            else
            {
                rule = FilterCreator.CreateRule(mp, ctype);

                if (rule == null) return null;

                List<FilterRule> filterRules = new List<FilterRule> { rule };
                try
                {
                    filter = ParameterFilterElement.Create(doc, filterName, catsIds);
#if R2017 || R2018
                    filter.SetRules(filterRules);
#else
                    filter.SetElementFilter(new ElementParameterFilter(filterRules));
#endif
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to create filter: {filterName}. {ex.Message}");
                }

            }
            return filter;
        }

        public static ParameterFilterElement CreateConstrFilter(Document doc, List<ElementId> catsIds, Parameter markParam, string mark, string filterNamePrefix)
        {
            string filterName = filterNamePrefix + "_Constr " + mark;

            ParameterFilterElement filter = DocumentGetter.GetFilterByName(doc, filterName);
            if (filter != null)
                return filter;

            FilterRule markEquals = CreateStringEqualsRule(markParam, mark);

#if R2017 || R2018
            filter = ParameterFilterElement.Create(doc, filterName, catsIds);
            filter.SetRules(new List<FilterRule> { markEquals });
#else
            ElementParameterFilter epf = new ElementParameterFilter(markEquals);
            filter = ParameterFilterElement.Create(doc, filterName, catsIds, epf);
#endif
            return filter;
        }

        public static ParameterFilterElement CreateRebarHostFilter(
            Document doc, List<ElementId> rebarCatsIds, Parameter rebarIsFamilyParam, Parameter rebarHostParam, Parameter rebarMrkParam,
            string hostMark, string filterNamePrefix, RebarFilterMode filterMode)
        {
            string filterName = filterNamePrefix + "_Rebar constr " + hostMark;

            if (filterMode == RebarFilterMode.IfcMode)
                filterName += " IFC";
            ParameterFilterElement filter = DocumentGetter.GetFilterByName(doc, filterName);
            if (filter != null)
                return filter;

            FilterRule ruleHostEquals = CreateStringEqualsRule(rebarHostParam, hostMark);

            if (filterMode == RebarFilterMode.SingleMode)
            {
#if R2017 || R2018
                filter = ParameterFilterElement.Create(doc, filterName, rebarCatsIds);
                filter.SetRules(new List<FilterRule> { ruleHostEquals });
#else
                ElementParameterFilter epf = new ElementParameterFilter(ruleHostEquals);
                filter = ParameterFilterElement.Create(doc, filterName, rebarCatsIds, epf);
#endif
                return filter;
            }

            FilterRule ruleIsNotFamily = ParameterFilterRuleFactory.CreateEqualsRule(rebarIsFamilyParam.Id, 0);
            FilterRule ruleIsFamily = ParameterFilterRuleFactory.CreateEqualsRule(rebarIsFamilyParam.Id, 1);
            FilterRule ruleMrkEquals = CreateStringEqualsRule(rebarMrkParam, hostMark);


#if R2017 || R2018

            if (filterMode == RebarFilterMode.StandardRebarMode)
            {
                filter = ParameterFilterElement.Create(doc, filterName, rebarCatsIds);
                filter.SetRules(new List<FilterRule> { ruleIsNotFamily, ruleHostEquals });
                return filter;
            }
            else if (filterMode == RebarFilterMode.IfcMode)
            {
                filter = ParameterFilterElement.Create(doc, filterName, rebarCatsIds);
                filter.SetRules(new List<FilterRule> { ruleIsFamily, ruleMrkEquals });
                return filter;
            }

#else
            if (filterMode == RebarFilterMode.DoubleMode)
            {
                ElementParameterFilter filterByStandardArm = new ElementParameterFilter(new List<FilterRule> { ruleIsNotFamily, ruleHostEquals });
                ElementParameterFilter filterForIfcArm = new ElementParameterFilter(new List<FilterRule> { ruleIsFamily, ruleMrkEquals });
                LogicalOrFilter orfilter = new LogicalOrFilter(filterByStandardArm, filterForIfcArm);
                filter = ParameterFilterElement.Create(doc, filterName, rebarCatsIds, orfilter);
                return filter;
            }
#endif
            return null;
        }


        public static FilterRule CreateStringEqualsRule(Parameter param, string value)
        {
            ElementId paramid = param.Id;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022
            FilterRule rule = ParameterFilterRuleFactory.CreateEqualsRule(paramid, value, true);
#else
            FilterRule rule = ParameterFilterRuleFactory.CreateEqualsRule(paramid, value);
#endif
            return rule;
        }


        public static FilterRule CreateRule(MyParameter mp, CriteriaType ctype)
        {
            Parameter param = mp.RevitParameter;
            FilterRule rule = null;
            if (ctype == CriteriaType.Equals)
            {
                switch (param.StorageType)
                {
                    case StorageType.None:
                        break;
                    case StorageType.Integer:
                        rule = ParameterFilterRuleFactory.CreateEqualsRule(param.Id, mp.AsInteger());
                        break;
                    case StorageType.Double:
                        rule = ParameterFilterRuleFactory.CreateEqualsRule(param.Id, mp.AsDouble(), 0.0001);
                        break;
                    case StorageType.String:
                        string val = mp.AsString();
                        if (val == null) break;
                        rule = CreateStringEqualsRule(param, val);
                        break;
                    case StorageType.ElementId:
                        rule = ParameterFilterRuleFactory.CreateEqualsRule(param.Id, mp.AsElementId());
                        break;
                    default:
                        break;
                }
            }

            if (ctype == CriteriaType.StartsWith)
            {
                switch (param.StorageType)
                {
                    case StorageType.None:
                        break;
                    case StorageType.String:
                        string val = mp.AsString();
                        if (val == null) break;
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022
                        rule = ParameterFilterRuleFactory.CreateBeginsWithRule(param.Id, val, true);
#else
                        rule = ParameterFilterRuleFactory.CreateBeginsWithRule(param.Id, val);
#endif
                        break;
                    default:
                        break;
                }
            }

            if (rule == null) throw new Exception("Unable to create the filter rule");
            return rule;

        }



        public static FilterRule CreateRule2(Parameter Param, string Function, string Value)
        {
            ElementId paramId = Param.Id;
            switch (Param.StorageType)
            {
                case StorageType.String:
                    FilterRule stringRule = CreateRule(paramId, Function, Value);
                    return stringRule;

                case StorageType.Integer:
                    int intValue = 0;
                    if (Value.Equals("Да") || Value.Equals("да") || Value.Equals("Yes") || Value.Equals("yes"))
                    {
                        intValue = 1;
                        goto Create;
                    }
                    if (Value.Equals("Нет") || Value.Equals("нет") || Value.Equals("No") || Value.Equals("no"))
                    {
                        intValue = 0;
                        goto Create;
                    }
                    int i = 0;
                    bool check = int.TryParse(Value, out i);
                    if (!check)
                    {
                        throw new Exception("Error with a parameter: " + Param.Definition.Name + " = " + Value);
                    }
                    else
                    {
                        intValue = int.Parse(Value);
                    }

                Create:
                    FilterRule intRule = CreateRule(paramId, Function, intValue);
                    return intRule;

                case StorageType.Double:
                    double doubleValue = double.Parse(Value);
                    FilterRule doubleRule = CreateRule(paramId, Function, doubleValue);
                    return doubleRule;

                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    int id = int.Parse(Value);
                    ElementId valueId = new ElementId(id);
#else
                    long id = long.Parse(Value);
                    ElementId valueId = new ElementId(id);
#endif
                    FilterRule idRule = CreateRule(paramId, Function, valueId);
                    return idRule;
            }
            return null;
        }


        private static FilterRule CreateRule(ElementId ParameterId, string Function, string Value)
        {
            switch (Function)
            {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022
                case "Равно":
                    return ParameterFilterRuleFactory.CreateEqualsRule(ParameterId, Value, true);
                case "Не равно":
                    return ParameterFilterRuleFactory.CreateNotEqualsRule(ParameterId, Value, true);
                case "Больше":
                    return ParameterFilterRuleFactory.CreateGreaterRule(ParameterId, Value, true);
                case "Больше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value, true);
                case "Меньше":
                    return ParameterFilterRuleFactory.CreateLessRule(ParameterId, Value, true);
                case "Меньше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value, true);
                case "Содержит":
                    return ParameterFilterRuleFactory.CreateContainsRule(ParameterId, Value, true);
                case "Не содержит":
                    return ParameterFilterRuleFactory.CreateNotContainsRule(ParameterId, Value, true);
                case "Начинается с":
                    return ParameterFilterRuleFactory.CreateBeginsWithRule(ParameterId, Value, true);
                case "Не начинается с":
                    return ParameterFilterRuleFactory.CreateNotBeginsWithRule(ParameterId, Value, true);
                case "Заканчивается на":
                    return ParameterFilterRuleFactory.CreateEndsWithRule(ParameterId, Value, true);
                case "Не заканчивается на":
                    return ParameterFilterRuleFactory.CreateNotEndsWithRule(ParameterId, Value, true);
                case "Поддерживает":
                    return ParameterFilterRuleFactory.CreateSharedParameterApplicableRule(Value);
#else
                case "Равно":
                    return ParameterFilterRuleFactory.CreateEqualsRule(ParameterId, Value);
                case "Не равно":
                    return ParameterFilterRuleFactory.CreateNotEqualsRule(ParameterId, Value);
                case "Больше":
                    return ParameterFilterRuleFactory.CreateGreaterRule(ParameterId, Value);
                case "Больше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value);
                case "Меньше":
                    return ParameterFilterRuleFactory.CreateLessRule(ParameterId, Value);
                case "Меньше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value);
                case "Содержит":
                    return ParameterFilterRuleFactory.CreateContainsRule(ParameterId, Value);
                case "Не содержит":
                    return ParameterFilterRuleFactory.CreateNotContainsRule(ParameterId, Value);
                case "Начинается с":
                    return ParameterFilterRuleFactory.CreateBeginsWithRule(ParameterId, Value);
                case "Не начинается с":
                    return ParameterFilterRuleFactory.CreateNotBeginsWithRule(ParameterId, Value);
                case "Заканчивается на":
                    return ParameterFilterRuleFactory.CreateEndsWithRule(ParameterId, Value);
                case "Не заканчивается на":
                    return ParameterFilterRuleFactory.CreateNotEndsWithRule(ParameterId, Value);
                case "Поддерживает":
                    return ParameterFilterRuleFactory.CreateSharedParameterApplicableRule(Value);
#endif
                default:
                    return null;
            }
        }

        private static FilterRule CreateRule(ElementId ParameterId, string Function, double Value)
        {
            switch (Function)
            {
                case "Равно":
                    return ParameterFilterRuleFactory.CreateEqualsRule(ParameterId, Value, 0.0001);
                case "Не равно":
                    return ParameterFilterRuleFactory.CreateNotEqualsRule(ParameterId, Value, 0.0001);
                case "Больше":
                    return ParameterFilterRuleFactory.CreateGreaterRule(ParameterId, Value, 0.0001);
                case "Больше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value, 0.0001);
                case "Меньше":
                    return ParameterFilterRuleFactory.CreateLessRule(ParameterId, Value, 0.0001);
                case "Меньше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value, 0.0001);
                default:
                    return null;
            }
        }


        private static FilterRule CreateRule(ElementId ParameterId, string Function, int Value)
        {
            switch (Function)
            {
                case "Равно":
                    return ParameterFilterRuleFactory.CreateEqualsRule(ParameterId, Value);
                case "Не равно":
                    return ParameterFilterRuleFactory.CreateNotEqualsRule(ParameterId, Value);
                case "Больше":
                    return ParameterFilterRuleFactory.CreateGreaterRule(ParameterId, Value);
                case "Больше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value);
                case "Меньше":
                    return ParameterFilterRuleFactory.CreateLessRule(ParameterId, Value);
                case "Меньше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, Value);
                default:
                    return null;
            }
        }

        private static FilterRule CreateRule(ElementId ParameterId, string Function, ElementId ValueId)
        {
            switch (Function)
            {
                case "Равно":
                    return ParameterFilterRuleFactory.CreateEqualsRule(ParameterId, ValueId);
                case "Не равно":
                    return ParameterFilterRuleFactory.CreateNotEqualsRule(ParameterId, ValueId);
                case "Больше":
                    return ParameterFilterRuleFactory.CreateGreaterRule(ParameterId, ValueId);
                case "Больше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, ValueId);
                case "Меньше":
                    return ParameterFilterRuleFactory.CreateLessRule(ParameterId, ValueId);
                case "Меньше или равно":
                    return ParameterFilterRuleFactory.CreateLessOrEqualRule(ParameterId, ValueId);
                default:
                    return null;
            }

        }
    }
}
