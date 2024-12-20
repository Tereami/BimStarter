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
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace RebarParametrisation
{
    public static class RebarParameters
    {
        /// <summary>
        /// Заполняет параметры BDS_RebarHostId, BDS_RebarUniqueHostId, Мрк.МаркаКонструкции, Рзм.ТолщинаОсновы. В случае нескольких элемеетов знчения записываются через разделитель "|"
        /// </summary>
        /// <param name="rebar"></param>
        /// <param name="hostElem"></param>
        public static string WriteHostInfoSingleRebar(Autodesk.Revit.ApplicationServices.Application revitApp, CategorySet rebarCatSet, Element rebar, List<Element> hostElements, RebarParametrisationSettings sets)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            BuiltInParameterGroup invalidParamGroup = BuiltInParameterGroup.INVALID;
#else
            ForgeTypeId invalidParamGroup = new ForgeTypeId(string.Empty);
#endif

            if (sets.UseHostId)
            {
                HashSet<string> hostIds = new HashSet<string>();
                foreach (Element hostElem in hostElements)
                {
                    string tempid = hostElem.Id.ToString();
                    hostIds.Add(tempid);
                }
                Parameter rebarHostIdParam = Tools.Model.ParameterUtils.Adder.CheckAndAddSharedParameter(rebar, revitApp, rebarCatSet, "BDS_RebarHostId", invalidParamGroup, true);
                string hostIdString = ConvertHashsetToString(hostIds);
                rebarHostIdParam.Set(hostIdString);
            }
            if (sets.UseUniqueHostId)
            {
                HashSet<string> hostUniqIds = new HashSet<string>();
                foreach (Element hostElem in hostElements)
                {
                    hostUniqIds.Add(hostElem.UniqueId);
                }

                Parameter rebarHostUniqueIdParam = Tools.Model.ParameterUtils.Adder.CheckAndAddSharedParameter(rebar, revitApp, rebarCatSet, "BDS_RebarUniqueHostId", invalidParamGroup, true);
                string hostUniqueId = ConvertHashsetToString(hostUniqIds);
                rebarHostUniqueIdParam.Set(hostUniqueId);
            }

            if (sets.UseHostMark)
            {
                HashSet<string> hostMarks = new HashSet<string>();
                foreach (Element hostElem in hostElements)
                {
                    Parameter hostMarkParam = hostElem.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);
                    string tempMark = hostMarkParam.AsString();
                    if (string.IsNullOrEmpty(tempMark))
                    {
                        return $"Не заполнена марка у конструкции: {hostElem.Id} в файле {hostElem.Document.Title}";
                    }
                    else
                    {
                        hostMarks.Add(tempMark);
                    }
                }

                string hostMark = ConvertHashsetToString(hostMarks);
                Tools.Model.ParameterUtils.Writer.TryWriteParameter(rebar, "Мрк.МаркаКонструкции", hostMark);
            }

            if (sets.UseHostThickness)
            {
                Parameter thicknessRebarParam = rebar.LookupParameter("Рзм.ТолщинаОсновы");
                if (thicknessRebarParam != null)
                {
                    if (!thicknessRebarParam.IsReadOnly)
                    {
                        HashSet<double> hostWidths = new HashSet<double>();

                        foreach (Element hostElem in hostElements)
                        {
                            if (hostElem is Wall)
                            {
                                Wall hostWall = hostElem as Wall;
                                hostWidths.Add(hostWall.Width);
                            }
                            if (hostElem is Floor)
                            {
                                Floor hostFloor = hostElem as Floor;
                                Parameter thicknessParam = hostFloor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM);
                                if (thicknessParam == null)
                                {
                                    thicknessParam = hostFloor.get_Parameter(BuiltInParameter.FLOOR_ATTR_DEFAULT_THICKNESS_PARAM);
                                    if (thicknessParam == null) hostWidths.Add(0);
                                }
                                if (thicknessParam != null)
                                {
                                    hostWidths.Add(thicknessParam.AsDouble());
                                }
                            }
                        }

                        double hostWidth = 0;
                        if (hostWidths.Count == 1)
                            hostWidth = hostWidths.First();
                        thicknessRebarParam.Set(hostWidth);
                    }
                }
            }
            return string.Empty;
        }

        private static string ConvertHashsetToString(HashSet<string> values)
        {
            List<string> vals = values.ToList();
            string result = vals[0];
            for (int i = 1; i < vals.Count; i++)
            {
                result += "|" + vals[i];
            }
            return result;
        }
    }
}