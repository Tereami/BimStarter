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
using System.Diagnostics;
using System.Linq;
#endregion

namespace PilesCoords
{

    /// <summary>
    /// Description of Support.
    /// </summary>
    public class SupportGetter
    {
        public static Parameter GetParameter(Element elem, string paramname, bool checkForWritable = false)
        {
            Parameter param = elem.LookupParameter(paramname);
            if (param == null)
            {
                ElementType etype = elem.Document.GetElement(elem.GetTypeId()) as ElementType;
                param = etype.LookupParameter(paramname);
                if (param == null)
                {
                    Trace.WriteLine("No parameter: " + paramname);
                }
            }
            if (checkForWritable && param.IsReadOnly)
            {
                Trace.WriteLine("Parameter is readonly: " + paramname);
            }
            return param;
        }

        public static List<FamilyInstance> GetPiles(List<Element> elems, Settings sets)
        {
            Trace.WriteLine("Search piles by name: " + sets.pileFamilyName);
            string[] famNames = sets.pileFamilyName.Split(';');
            List<FamilyInstance> piles = elems
                .Where(i => i is FamilyInstance)
                .Cast<FamilyInstance>()
                .Where(i => famNames.Contains(i.Symbol.FamilyName))
                .ToList();
            Trace.WriteLine("Piles found: " + piles.Count.ToString());
            return piles;
        }



        public static string GetPileUsesPrefix(Element pile)
        {
            int isAnker = pile.LookupParameter("Анкерная").AsInteger();
            int isTested = pile.LookupParameter("Испытуемая").AsInteger();
            string prefix = "Р";
            if (isAnker != 0 && isTested == 0) prefix = "А";
            if (isAnker == 0 && isTested != 0) prefix = "И";
            Trace.WriteLine($"Pile id {pile.Id} prefix {prefix}");
            return prefix;
        }

        public static string GetPileUsesText(Element pile)
        {
            int isAnker = pile.LookupParameter("Анкерная").AsInteger();
            int isTested = pile.LookupParameter("Испытуемая").AsInteger();
            string uses = "Рядовая";
            if (isAnker != 0 && isTested == 0) uses = "Анкеруемая";
            if (isAnker == 0 && isTested != 0) uses = "Подвергается стат. испытанию";
            Trace.WriteLine($"Pile id {pile.Id} prefix {uses}");
            return uses;
        }
    }
}
