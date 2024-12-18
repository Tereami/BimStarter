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
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ViewsSheetsTools
{
    class DuplicateNamesHandler : IDuplicateTypeNamesHandler
    {
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            Document doc = args.Document;
            List<ElementId> ids = args.GetTypeIds().ToList();
            foreach (ElementId id in ids)
            {
                Element elem = doc.GetElement(id);
                if (elem is View)
                {
                    DuplicateTypes.types.Add(elem.Name);
                }
            }
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}
