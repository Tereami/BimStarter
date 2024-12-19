#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2024, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2024, all rigths reserved.*/
#endregion
#region Usings
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace SelectHost
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("SelectHost"));

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            Selection sel = uiDoc.Selection;
            List<ElementId> selIds = sel.GetElementIds().ToList();
            if (selIds.Count == 0)
            {
                message = "No selected elements / Выберите элементы";
                return Result.Failed;
            }

            List<ElementId> hostIds = new List<ElementId>();

            foreach (ElementId elid in selIds)
            {
                Element selElem = doc.GetElement(elid);
                Trace.WriteLine($"Selected elem id: {selElem.Id}");

                ElementId hostId = null;

                if (selElem is AreaReinforcement)
                {
                    AreaReinforcement el = selElem as AreaReinforcement;
                    hostId = el.GetHostId();
                    Trace.WriteLine("It is area reinforcement");
                }
                else if (selElem is PathReinforcement)
                {
                    PathReinforcement el = selElem as PathReinforcement;
                    hostId = el.GetHostId();
                    Trace.WriteLine("It is path reinforcement");
                }
                else if (selElem is Rebar)
                {
                    Rebar el = selElem as Rebar;
                    hostId = el.GetHostId();
                    Trace.WriteLine("It is rebar");
                }
                else if (selElem is RebarInSystem)
                {
                    RebarInSystem el = selElem as RebarInSystem;
                    hostId = el.SystemId;
                    Trace.WriteLine("It is rebar in system");
                }
                else if (selElem is FamilyInstance)
                {
                    FamilyInstance el = selElem as FamilyInstance;
                    Element host = el.Host;
                    if (host != null)
                    {
                        hostId = host.Id;
                        Trace.WriteLine("It is family instance with host");
                    }
                    else
                    {
                        Element parentFamily = el.SuperComponent;
                        if (parentFamily != null)
                        {
                            Trace.WriteLine("It is family instance with parent family");
                            hostId = parentFamily.Id;
                        }
                    }
                }

                if (hostId == null)
                {
                    message = $"Failed to get host: {elid}";
                    Trace.WriteLine($"Host not found for element id {elid}");
                    return Result.Failed;
                }
                else
                {
                    Trace.WriteLine("Host is found for element id {elid}");
                    hostIds.Add(hostId);
                }
            }

            sel.SetElementIds(hostIds);
            return Result.Succeeded;
        }
    }
}
