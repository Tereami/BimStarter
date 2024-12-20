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
#endregion

namespace Tools.Model.ParameterUtils
{
    public static class Getter
    {
        /// <summary>
        /// Получает параметр из экземпляра или типа элемента
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="ParameterName"></param>
        /// <returns></returns>
        public static Parameter GetParameter(Element elem, string ParameterName)
        {
            Parameter param = elem.LookupParameter(ParameterName);
            if (param != null) return param;

            ElementId typeId = elem.GetTypeId();
            if (typeId == null) return null;

            Element type = elem.Document.GetElement(typeId);

            param = type.LookupParameter(ParameterName);
            return param;
        }
    }
}
