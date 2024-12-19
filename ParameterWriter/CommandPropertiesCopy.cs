using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ParameterWriter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandPropertiesCopy : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("PropertiesCopy"));

            Document doc = commandData.Application.ActiveUIDocument.Document;
            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            Element firstElem;

            List<ElementId> selIds = sel.GetElementIds().ToList();
            Trace.WriteLine("Selected elements: " + selIds.Count.ToString());
            if (selIds.Count > 0)
            {
                firstElem = doc.GetElement(selIds.First());
            }
            else
            {
                Reference r1;
                try
                {
                    r1 = sel.PickObject(ObjectType.Element, MyStrings.TextSelectElementToCopy);
                }
                catch
                {
                    return Result.Cancelled;
                }

                firstElem = doc.GetElement(r1.ElementId);
                Trace.WriteLine($"First elem id: {firstElem.Id}");
            }

            if (firstElem == null)
            {
                message += MyStrings.ErrorSomethingWentWrong;
                Trace.WriteLine("First elem is null");
                return Result.Failed;
            }

            ParameterMap parameters = firstElem.ParametersMap;
            Trace.WriteLine("Parameters found: " + parameters.Size.ToString());

            while (true)
            {
                try
                {
                    SameTypeSelectionFilter selFilter = new SameTypeSelectionFilter(firstElem);
                    Reference r = sel.PickObject(ObjectType.Element, selFilter, MyStrings.TextSelectElementToCopy);
                    if (r == null) continue;
                    ElementId curId = r.ElementId;
                    if (curId == null || curId == ElementId.InvalidElementId) continue;
                    Element curElem = doc.GetElement(curId);
                    if (curElem == null) continue;
                    Trace.WriteLine($"Cur element id: {curId}");

                    try
                    {
                        ElementId firstTypeId = firstElem.GetTypeId();
                        ElementId curTypeId = curElem.GetTypeId();
                        if (firstTypeId != curTypeId)
                        {
                            using (Transaction t1 = new Transaction(doc))
                            {
                                t1.Start(MyStrings.TransactionTypeCopy);

                                curElem.get_Parameter(BuiltInParameter.ELEM_TYPE_PARAM).Set(firstTypeId);

                                t1.Commit();
                                Trace.WriteLine("Type of element is changed");
                            }
                        }
                    }
                    catch { }


                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Sopy properties");

                        foreach (Parameter param in parameters)
                        {
                            if (param == null) continue;
                            if (!param.HasValue) continue;
                            try
                            {
                                Parameter curParam = curElem.get_Parameter(param.Definition);

                                switch (param.StorageType)
                                {
                                    case StorageType.None:
                                        break;
                                    case StorageType.Integer:
                                        curParam.Set(param.AsInteger());
                                        break;
                                    case StorageType.Double:
                                        curParam.Set(param.AsDouble());
                                        break;
                                    case StorageType.String:
                                        curParam.Set(param.AsString());
                                        break;
                                    case StorageType.ElementId:
                                        curParam.Set(param.AsElementId());
                                        break;
                                    default:
                                        break;
                                }
                                Trace.WriteLine("Param value is written: " + curParam.Definition.Name + " = " + curParam.AsValueString());
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.Message);
                                continue;
                            }
                        }
                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    return Result.Succeeded;
                }
            }
        }
    }
}