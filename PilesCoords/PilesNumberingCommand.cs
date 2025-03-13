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
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
#endregion

namespace PilesCoords
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class PilesNumberingCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("PilesNumbering"));
            Settings sets = null;

            Tools.SettingsSaver.Saver<Settings> saver = new Tools.SettingsSaver.Saver<Settings>();
            sets = saver.Activate(nameof(PilesCoords));

            Document doc = commandData.Application.ActiveUIDocument.Document;

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Element> selems = sel.GetElementIds().Select(i => doc.GetElement(i)).ToList();

            List<FamilyInstance> piles = SupportGetter.GetPiles(selems, sets);
            Trace.WriteLine("Selected piles count: " + piles.Count.ToString());
            if (piles.Count == 0)
            {
                message = MyStrings.MessageSelectPiles;
                return Result.Failed;
            }

            //Сортирую по координатам
            int numberingUpDown = 1;
            if (sets.numberingUpDown) numberingUpDown = -1;
            List<FamilyInstance> pilesSorted = piles
                .OrderBy(x => numberingUpDown * Math.Round((x.Location as LocationPoint).Point.Y))
                .ThenBy(x => Math.Round((x.Location as LocationPoint).Point.X))
                .ToList();
            Trace.WriteLine("Parameter for number: " + sets.paramPilePosition);
            //Указываю позиции по координатам
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionNumbering);
                for (int i = 0; i < pilesSorted.Count; i++)
                {
                    Element pile = pilesSorted[i];
                    SupportGetter.GetParameter(pile, sets.paramPilePosition, true).Set((i + sets.firstNumber).ToString());
                }
                t.Commit();
            }

            Trace.WriteLine("Numbering success");
            return Result.Succeeded;
        }
    }
}
