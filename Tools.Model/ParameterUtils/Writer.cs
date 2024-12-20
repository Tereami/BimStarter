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
    public static class Writer
    {
        /// <summary>
        /// Записывает значение параметра вэлемент, если параметр присутствует и не заблокирован
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns>true если параметр успешно записан, иначе false</returns>
        public static bool TryWriteParameter(Element elem, string paramName, string value)
        {
            Parameter param = elem.LookupParameter(paramName);
            if (param != null)
            {
                if (!param.IsReadOnly)
                {
                    param.Set(value);
                    return true;
                }
            }
            return false;
        }
    }
}
