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
Zuev Aleksandr, 2020, all rigths reserved.

More about solution / Подробнее: http://weandrevit.ru/plagin-massa-plastin-km/
 */
#endregion
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RVTDocument = Autodesk.Revit.DB.Document;
using RVTransaction = Autodesk.Revit.DB.Transaction;

#if R2019 || R2020
using FabricationTransaction = RvtDwgAddon.FabricationTransaction;
#else
using FabricationTransaction = Autodesk.SteelConnectionsDB.FabricationTransaction;
#endif

using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Steel;
#endregion

namespace RevitPlatesWeight
{
    public enum DimensionKind { Length, Width, Thickness }
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("SteelParametrisation"));
            int revitVersionNumber = int.Parse(commandData.Application.Application.VersionNumber);
            if (revitVersionNumber < 2019)
            {
                TaskDialog.Show(MyStrings.Error, MyStrings.ErrorRevit2019);
                Trace.WriteLine(MyStrings.ErrorRevit2019);
                return Result.Cancelled;
            }

            Tools.SettingsSaver.Saver<Settings> saver = new Tools.SettingsSaver.Saver<Settings>();
            Settings sets = saver.Activate("RevitPlatesWeight");
            SettingsForm form = new SettingsForm(sets, saver.GetXmlPath());
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                if (form.DialogResult == System.Windows.Forms.DialogResult.Retry)
                {
                    saver.Reset();
                }
                return Result.Cancelled;
            }
            sets = form.newSets;

            if (sets == null) return Result.Cancelled;
            int platesCount = 0;

            RVTDocument doc = commandData.Application.ActiveUIDocument.Document;

            View calculateView = doc.ActiveView;
            if (!(calculateView is View3D) || calculateView.DetailLevel != ViewDetailLevel.Fine)
            {
                TaskDialog.Show(MyStrings.Error, MyStrings.ErrorDetailLevel);
                Trace.WriteLine("Unable to use view " + calculateView.Name);
                return Result.Failed;
            }

            FilteredElementCollector collectorConstrs = null;
            FilteredElementCollector collectorPlatesFree = null;
            FilteredElementCollector collectorJoints = null;

            if (sets.useOnlyVisibleOnCurrentView)
            {
                collectorConstrs = new FilteredElementCollector(doc, calculateView.Id);
                collectorPlatesFree = new FilteredElementCollector(doc, calculateView.Id);
                collectorJoints = new FilteredElementCollector(doc, calculateView.Id);
            }
            else
            {
                collectorConstrs = new FilteredElementCollector(doc);
                collectorPlatesFree = new FilteredElementCollector(doc);
                collectorJoints = new FilteredElementCollector(doc);
            }



            List<Plate> plates = collectorPlatesFree
                .WhereElementIsNotElementType()
                .OfClass(typeof(SteelProxyElement))
                .Cast<SteelProxyElement>()
                .Where(i => i.GeomType == GeomObjectType.Plate)
                .Select(i => new PlateFree(i, calculateView, sets))
                .ToList<Plate>();
            Trace.WriteLine("PlatesFree found: " + plates.Count.ToString());


            List<StructuralConnectionHandler> joints = collectorJoints
                .WhereElementIsNotElementType()
                .OfClass(typeof(StructuralConnectionHandler))
                .Cast<StructuralConnectionHandler>()
                .ToList();
            Trace.WriteLine("Joints found: " + joints.Count.ToString());

            using (FabricationTransaction t = new FabricationTransaction(doc, true, "Test plates"))
            {
                Trace.WriteLine("Start fabrication transaction");
                foreach (StructuralConnectionHandler joint in joints)
                {
                    List<Subelement> subelems = joint.GetSubelements().ToList();
                    foreach (Subelement subelem in subelems)
                    {
                        if (subelem.Category.Id != new ElementId(BuiltInCategory.OST_StructConnectionPlates)) continue;
                        PlateInJoint pij = new PlateInJoint(subelem, doc, sets);
                        plates.Add(pij);
                    }
                }
                Trace.WriteLine("Fabrication transaction success");
            }


            Dictionary<string, List<Plate>> platesGrouped = plates
            .GroupBy(i => i.GetKey(sets.enablePlatesNumbering, sets.writePlatesLengthWidth))
            .ToDictionary(j => j.Key, j => j.ToList());
            Trace.WriteLine("Plates groups: " + platesGrouped.Keys.Count.ToString());

            using (RVTransaction t = new RVTransaction(doc))
            {
                t.Start("Plates weight");
                Trace.WriteLine("Start plates parametrisation");
                foreach (Plate plate in plates)
                {
                    plate.CalculateValues(doc, sets);
                    plate.WriteValues(sets, doc);
                    platesCount++;
                }
                t.Commit();
                Trace.WriteLine("Plates parametrisation completed");
            }


            if (sets.writeBeamLength || sets.writeColumnLength)
            {
                using (RVTransaction t = new RVTransaction(doc))
                {
                    t.Start(MyStrings.TransactionName);
                    Trace.WriteLine("Start beams and columns parametrisation");

                    List<BuiltInCategory> constrCats = new List<BuiltInCategory> {
                        BuiltInCategory.OST_StructuralFraming,
                        BuiltInCategory.OST_StructuralColumns,
                    };
                    List<Element> constrs = collectorConstrs
                        .WhereElementIsNotElementType()
                        .WherePasses(new ElementMulticategoryFilter(constrCats))
                        .ToElements()
                        .ToList();
                    Trace.WriteLine("Beams and columns found: " + constrs.Count.ToString());
                    foreach (Element elem in constrs)
                    {
                        Parameter lengthParam = null;

                        if (elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralColumns))
                        {
#if !R2019
                            lengthParam = elem.get_Parameter(BuiltInParameter.STEEL_ELEM_CUT_LENGTH);
#endif
                        }
                        else if (elem.Category.Id == new ElementId(BuiltInCategory.OST_StructuralFraming))
                        {
                            lengthParam = elem.get_Parameter(BuiltInParameter.STRUCTURAL_FRAME_CUT_LENGTH);
                        }
                        double? cutLength = null;
                        if (lengthParam != null && lengthParam.HasValue)
                        {
                            cutLength = lengthParam.AsDouble();
                        }
                        else
                        {
                            cutLength = Tools.Geometry.Orientation.FindTheLongestCurveLength(doc.ActiveView, elem);
                        }
                        if (cutLength == null || cutLength == 0) continue;

                        Parameter trueLengthParam = elem.get_Parameter(new Guid("b62d0a35-0f0f-432d-9d3d-e821093a7d02")); // Рзм.ДлинаБалкиИстинная
                        if (trueLengthParam == null || !trueLengthParam.HasValue)
                        {
                            Trace.WriteLine("No parameter Dim.BeamTrueLeigth");
                            continue;
                        }
                        double trueLength = trueLengthParam.AsDouble();

                        double delta = (double)cutLength - trueLength;

                        Parameter deltaParam = elem.get_Parameter(new Guid("bcbcac9a-58f2-429e-bac0-f9d823ef8043")); //Рзм.КорректировкаДлины
                        if (deltaParam == null) continue;
                        if (deltaParam.IsReadOnly) continue;

                        deltaParam.Set(delta);
                    }

                    t.Commit();
                    Trace.WriteLine("Beams and columns finished");
                }
            }

            if (sets.enablePlatesNumbering)
            {
                using (RVTransaction t = new RVTransaction(doc))
                {
                    t.Start("Plates numbering");
                    Trace.WriteLine("Start plates numbering");
                    int platePosition = sets.plateNumberingStartWith;
                    foreach (var kvp in platesGrouped)
                    {
                        List<Plate> curPlates = kvp.Value;
                        Trace.WriteLine("Plate group key: " + kvp.Key);

                        foreach (Plate plate in curPlates)
                        {
                            plate.WriteParameter(doc, sets.plateNumberingParamName, StorageType.String, platePosition.ToString(), true);
                        }
                        platePosition++;
                    }
                    t.Commit();
                    Trace.WriteLine("Plates numbering finished");
                }
            }

            Tools.Forms.BalloonTip.Show(MyStrings.Success, $"{MyStrings.PlatesProcessed}: {platesCount}");
            saver.Save(sets);
            return Result.Succeeded;
        }
    }
}