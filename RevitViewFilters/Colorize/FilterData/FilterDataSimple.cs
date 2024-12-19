using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitViewFilters
{
    class FilterDataSimple : IFilterData
    {
        public static Dictionary<string, string> ForbiddenFilterNameSymbols = new Dictionary<string, string>
        {
            { ":", "_colon_" },
            { ";", "_semicolon" },
            { "[", "_bracket_" },
            { "]", "_bracket_" },
            { "{", "_brace_" },
            { "}", "_brace_" },
            { "?", "_question" },
            { "~", "_tilda_" },
            { "`", "_gravis_" },
            { "|", "_pipe_" },
            { "<", "_less_" },
            { ">", "_greater_" },
            { "\\", "_slash_" },
        };

        public int ValuesCount => valuesList.Count;

        private string _filterNamePrefix = "_vc_";
        public string FilterNamePrefix => _filterNamePrefix;

        List<MyParameter> valuesList;

        private string catsName;
        private string paramName;
        List<ElementId> catsIds;
        List<Element> elems;
        int startSymbols;
        CriteriaType _criteriaType;

        public FilterDataSimple(Document doc, List<Element> Elems, MyParameter MyParam, int StartSymbols, CriteriaType CritType)
        {
            
            paramName = MyParam.Name;
            catsIds = MyParam.FilterableCategoriesIds;
            elems = Elems;
            
            _criteriaType = CritType;
            if (CritType == CriteriaType.Equals)
                startSymbols = 0;
            else
                startSymbols = StartSymbols;
        }

        public bool ApplyFilters(Document doc, View v, ElementId fillPatternId, bool colorLines, bool colorFill)
        {
            for (int i = 0; i < valuesList.Count; i++)
            {
                MyParameter mp = valuesList[i];
                Parameter param = mp.RevitParameter;
                string filterName = _filterNamePrefix + catsName + " " + paramName;

                if (_criteriaType == CriteriaType.Equals)
                    filterName = filterName + MyStrings.FilterRuleEquals;
                else if (_criteriaType == CriteriaType.StartsWith)
                    filterName = filterName + MyStrings.FilterRuleBeginsWith;
                filterName += mp.AsValueString();

                foreach (var kvp in ForbiddenFilterNameSymbols)
                {
                    if (filterName.Contains(kvp.Key))
                        filterName = filterName.Replace(kvp.Key, kvp.Value);
                }

                ParameterFilterElement filter = FilterCreator.createSimpleFilter(doc, catsIds, filterName, mp, _criteriaType);
                if (filter == null) continue;

                ViewUtils.ApplyViewFilter(doc, v, filter, fillPatternId, i, colorLines, colorFill);

            }
            return true;
        }

        public MyDialogResult CollectValues(Document doc, View v)
        {
            MyDialogResult result = new MyDialogResult(ResultType.ok, "");
            if (catsIds.Count > 1)
            {
                List<MyCategory> mycats = new List<MyCategory>();
                foreach(ElementId catid in catsIds)
                {
                    mycats.Add(new MyCategory(doc, catid));
                }
                FormSelectCategories formSelCats = new FormSelectCategories(doc, mycats);
                if (formSelCats.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    result.ResultType = ResultType.cancel;
                    return result;
                }
                    
                catsIds = formSelCats.checkedCategoriesIds;

                //перефильтрую элементы после того как перевыбрал категории
                elems = elems.Where(e => catsIds.Contains(e.Category.Id)).ToList();
            }

            catsName = ViewUtils.GetCategoriesName(doc, catsIds);

            valuesList = DocumentGetter.GetParameterValues(doc, elems, paramName, startSymbols);

            return result;
        }
    }
}
