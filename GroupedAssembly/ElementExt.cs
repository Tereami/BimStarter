//Данный код опубликован под лицензией Creative Commons Attribution-NonCommercial-ShareAlike.
//Разрешено редактировать, изменять и брать данный код за основу для производных в некоммерческих целях,
//при условии указания авторства и если производные лицензируются на тех же условиях.
//Программа поставляется "как есть". Автор не несет ответственности за возможные последствия её использования.
//Зуев Александр, 2019, все права защищены.
//This code is listed under the Creative Commons Attribution-NonCommercial-ShareAlike license.
//You may redistribute, remix, tweak, and build upon this work non-commercially, 
//as long as you credit the author by linking back and license your new creations under the same terms.
//This code is provided 'as is'. Author disclaims any implied warranty. 
//Zuev Aleksandr, 2019, all rigths reserved.


using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace GroupedAssembly
{
    public static class ElementExt
    {
        public static bool CanAssembling(this Element e)
        {
            return AssemblyInstance.AreElementsValidForAssembly(e.Document, new List<ElementId> {e.Id},
                ElementId.InvalidElementId);
        }

        public static int GetValue(this ElementId elementId)
        {
#if R2017 || R2018 || R2019 || R2020 || R2021 || R2022 || R2023
            return elementId.IntegerValue;
#else
            return (int)elementId.Value;
#endif
        }
    }
}