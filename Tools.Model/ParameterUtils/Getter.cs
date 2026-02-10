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
using ParameterWriter;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Tools.Model.ParameterUtils
{
    public static class Getter
    {
        /// <summary>
        /// Получает параметр из экземпляра или типа элемента, либо из информации о проекте
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        public static Parameter GetParameter(Element elem, string ParameterName, bool GetFromType, bool GetFromProjectInfo)
        {
            Parameter param = elem.LookupParameter(ParameterName);
            if (param != null) return param;

            if (GetFromType)
            {
                ElementId typeId = elem.GetTypeId();
                if (typeId == null || typeId == ElementId.InvalidElementId) return null;

                Element type = elem.Document.GetElement(typeId);

                param = type.LookupParameter(ParameterName);
                if (param != null) return param;
            }

            if (GetFromProjectInfo)
            {
                ProjectInfo pi = elem.Document.ProjectInformation;
                param = pi.LookupParameter(ParameterName);
                if (param != null) return param;
            }
            return param;
        }

        public static string GetParameterValAsString(Element e, string paramName)
        {
            Parameter param = Tools.Model.ParameterUtils.Getter.GetParameter(e, paramName, true, true);
            if (param == null) return string.Empty;

            string val = GetParameterValAsString(param);
            return val;
        }


        public static string GetParameterValAsString(Parameter param)
        {
            string val = string.Empty;

            switch (param.StorageType)
            {
                case StorageType.None:
                    return string.Empty;
                case StorageType.Integer:
                    val = param.AsInteger().ToString();
                    break;
                case StorageType.Double:
                    double doubval = param.AsDouble();
#if R2017 || R2018 || R2019 || R2020
                    doubval = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.DisplayUnitType);
#else
                    doubval = UnitUtils.ConvertFromInternalUnits(param.AsDouble(), param.GetUnitTypeId());
#endif
                    val = doubval.ToString("F2");
                    break;
                case StorageType.String:
                    val = param.AsString();
                    break;
                case StorageType.ElementId:
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
                    val = param.AsElementId().IntegerValue.ToString();
#else
                    val = param.AsElementId().Value.ToString();
#endif
                    break;
            }
            if (string.IsNullOrEmpty(val))
                return string.Empty;
            else
                return val;
        }

        public static List<ParameterContainer> GetAllElementParameters(Element elem, bool getEmptyParams)
        {
            List<ParameterContainer> parameters = new List<ParameterContainer>();
            foreach (Parameter p in elem.Parameters)
            {
                if (!getEmptyParams && !p.HasValue) continue;
                ParameterContainer pc = new ParameterContainer(p);
                parameters.Add(pc);
            }

            ElementId typeId = elem.GetTypeId();
            if (typeId == null || typeId == ElementId.InvalidElementId) return parameters;

            ElementType elemType = elem.Document.GetElement(typeId) as ElementType;
            if (elemType == null) return parameters;

            parameters.AddRange(GetAllElementParameters(elemType, getEmptyParams));
            return parameters;
        }


        /// <summary>
        /// Формирует текст на базе строки-"конструктора", содержащего имена параметров,которые будут заменены на значения параметров из данного элемента 
        /// </summary>
        /// <param name="constructor">Строка конструктора. Имена параметров должны быть включены в треугольные скобки.</param>
        /// <returns>Сформированный текст</returns>
        public static string GetByConstructor(Element elem, string constructor, bool clearIllegalChars)
        {
            string name = "";

            string prefix = constructor.Split('<').First();
            name = name + prefix;

            string[] sa = constructor.Split('<');
            for (int i = 0; i < sa.Length; i++)
            {
                string s = sa[i];
                if (!s.Contains(">")) continue;

                string paramName = s.Split('>').First();
                string separator = s.Split('>').Last();

                Parameter valparam = GetParameter(elem, paramName, true, true);
                if (valparam == null || !valparam.HasValue) continue;
                string val = GetParameterValAsString(valparam);
                if (clearIllegalChars)
                    val = Tools.Extensions.Paths.ClearIllegalCharacters(val);

                name = name + val;
                name = name + separator;
            }

            char[] arr = name.Where(c => (char.IsLetterOrDigit(c) ||
                             char.IsWhiteSpace(c) ||
                             c == '-' ||
                             c == '_' ||
                             c == ',' ||
                             c == '.')).ToArray();

            name = new string(arr);

            return name;
        }
    }
}
