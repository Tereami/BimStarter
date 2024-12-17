using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;

namespace RebarTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CommandRebarPresentation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("RebarPresentation"));
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View v = doc.ActiveView;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            List<Rebar> bars = new List<Rebar>();
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Rebar presentation");
                foreach (ElementId id in sel.GetElementIds())
                {
                    Rebar r = doc.GetElement(id) as Rebar;
                    if (r != null)
                    {
                        Debug.WriteLine($"Current rebar id: {id}");
                        bool check = r.CanApplyPresentationMode(v);
                        if (!check)
                        {
                            Debug.WriteLine("Cant apply presentation mode");
                            continue;
                        }

                        r.SetPresentationMode(v, RebarPresentationMode.Select);
                        int count = r.NumberOfBarPositions;
                        int middle = count / 2;
                        Debug.WriteLine("Bars count: " + count + ", middle bar: " + middle);
                        for (int i = 0; i < count; i++)
                        {
                            if (i == 0 || i == (count - 1) || i == middle)
                            {
                                r.SetBarHiddenStatus(v, i, false);
                                Debug.WriteLine("Bar " + i + ", set visible");
                            }
                            else
                            {
                                r.SetBarHiddenStatus(v, i, true);
                                Debug.WriteLine("Bar " + i + ", set invisible");
                            }
                        }
                    }
                    RebarInSystem ris = doc.GetElement(id) as RebarInSystem;
                    if (ris != null)
                    {
                        Debug.WriteLine($"Current RebarInSystem id: {id}");
                        bool check = ris.CanApplyPresentationMode(v);
                        if (!check)
                        {
                            Debug.WriteLine("Cant apply presentation mode");
                            continue;
                        }
                        ris.SetPresentationMode(v, RebarPresentationMode.Select);
                        int count = ris.NumberOfBarPositions;
                        int middle = count / 2;
                        for (int i = 0; i < count; i++)
                        {
                            if (i == 0 || i == (count - 1) || i == middle)
                            {
                                ris.SetBarHiddenStatus(v, i, false);
                                Debug.WriteLine("Bar " + i + ", set visible");
                            }
                            else
                            {
                                ris.SetBarHiddenStatus(v, i, true);
                                Debug.WriteLine("Bar " + i + ", set invisible");
                            }
                        }
                    }
                }
                t.Commit();
            }
            Debug.WriteLine("All done");
            return Result.Succeeded;
        }
    }
}
