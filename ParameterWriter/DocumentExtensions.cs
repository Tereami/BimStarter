#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2022, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2022, all rigths reserved.
*/
#endregion
#region usings
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
#endregion


namespace ParameterWriter
{
    public static class DocumentExtensions
    {
        public static long GetId(this Element elem)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            return elem.Id.IntegerValue;
#else
            return elem.Id.Value;
#endif
        }
        public static List<View> GetFiller3dViews(this Document doc)
        {
            List<View> views = new FilteredElementCollector(doc)
                .OfClass(typeof(View))
                .Cast<View>()
                .Where(i => i.Name.StartsWith("Filler#") && i.Name.Contains("=") && !i.IsTemplate)
                .ToList();

            return views;
        }

        public static List<Element> GetElementOnView(this View v)
        {
            Document doc = v.Document;
            FilteredElementCollector col = new FilteredElementCollector(doc, v.Id)
                .WhereElementIsNotElementType();

            List<Element> elems = new List<Element>();
            foreach (Element elem in col)
            {
                if (elem.Category == null)
                    continue;
                ElementId camerasCategoryId = new ElementId(BuiltInCategory.OST_Cameras);
                if (elem.Category.Id == camerasCategoryId)
                    continue;

                elems.Add(elem);
            }
            return elems;
        }

        public static Dictionary<long, Dictionary<string, string>> GetElementParametersByViews(this Document doc, List<View> views)
        {
            Dictionary<long, Dictionary<string, HashSet<string>>> elemIdsAndParamValues =
                new Dictionary<long, Dictionary<string, HashSet<string>>>();

            foreach (View curView in views)
            {
                List<Element> viewCol = curView.GetElementOnView();

                string viewName = curView.Name;
                string splitName = curView.Name.Split('#').Last();
                string[] splitParam = splitName.Split('=');
                string paramName = splitParam[0];
                string paramValue = splitParam[1];

                foreach (Element elem in viewCol)
                {
                    long elemId = elem.GetId();
                    if (elemIdsAndParamValues.ContainsKey(elemId))
                    {
                        if (elemIdsAndParamValues[elemId].ContainsKey(paramName))
                        {
                            elemIdsAndParamValues[elemId][paramName].Add(paramValue);
                        }
                        else
                        {
                            elemIdsAndParamValues[elemId].Add(paramName, new HashSet<string> { paramValue });
                        }
                    }
                    else
                    {
                        Dictionary<string, HashSet<string>> newElemInfo = new Dictionary<string, HashSet<string>>();
                        newElemInfo.Add(paramName, new HashSet<string> { paramValue });
                        elemIdsAndParamValues.Add(elemId, newElemInfo);
                    }
                }
            }

            Dictionary<long, Dictionary<string, string>> elemsAndJoinedValues = new Dictionary<long, Dictionary<string, string>>();
            foreach (var kvp in elemIdsAndParamValues)
            {
                Dictionary<string, string> curElementParams = new Dictionary<string, string>();
                foreach (KeyValuePair<string, HashSet<string>> paramNamesAndValues in kvp.Value)
                {
                    string paramName = paramNamesAndValues.Key;
                    List<string> valuesList = paramNamesAndValues.Value.ToList();
                    valuesList.Sort();
                    string joinedParamValues = string.Join(";", valuesList);
                    curElementParams.Add(paramName, joinedParamValues);
                }
                elemsAndJoinedValues.Add(kvp.Key, curElementParams);
            }

            return elemsAndJoinedValues;
        }
    }
}
