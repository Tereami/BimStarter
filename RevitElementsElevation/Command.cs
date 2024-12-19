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
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.Model;

#endregion


namespace RevitElementsElevation
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger(nameof(RevitElementsElevation)));

            Document doc = commandData.Application.ActiveUIDocument.Document;

            Tools.SettingsSaver.Saver<Config> saver = new Tools.SettingsSaver.Saver<Config>();
            Config cfg = saver.Activate(nameof(RevitElementsElevation));
            if (cfg == null)
            {
                Trace.WriteLine("Failed to read config xml file");
                return Result.Cancelled;
            }

            List<FamilyInstance> fams = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>().ToList();

            List<FamilyInstance> famsHoles = new List<FamilyInstance>();

            List<Element> ColumnsAndWalls = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .ToList();
            ColumnsAndWalls.AddRange(new FilteredElementCollector(doc).OfClass(typeof(Wall)).ToList());
            Trace.WriteLine("Columns and walls found: " + ColumnsAndWalls.Count);

            int ColumnAndWallsCount = 0;

            foreach (FamilyInstance fi in fams)
            {
                Parameter paramBaseLevelElev = fi.LookupParameter(cfg.paramBaseLevel);
                Parameter paramElevOnLevel = fi.LookupParameter(cfg.paramElevOnLevel);

                if (paramBaseLevelElev != null && paramElevOnLevel != null)
                {
                    famsHoles.Add(fi);
                }
            }
            Trace.WriteLine($"Openings found: {famsHoles.Count}");

            int count = 0;
            int err = 0;

            BasePoint projectBasePoint = new FilteredElementCollector(doc)
                .OfClass(typeof(BasePoint))
                .WhereElementIsNotElementType()
                .Cast<BasePoint>()
                .Where(i => i.IsShared == false)
                .First();
            double projectPointElevation = projectBasePoint.get_BoundingBox(null).Min.Z;
            Trace.WriteLine("Project base point elevation: " + (projectPointElevation * 304.8).ToString());

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Transaction");
                if (famsHoles.Count == 0 && ColumnsAndWalls.Count == 0)
                {
                    TaskDialog.Show("Holes elevation", MyStrings.ErrorFamiliesNotFound);
                    return Result.Failed;
                }
                else
                {
                    foreach (FamilyInstance fi in famsHoles)
                    {
                        Trace.WriteLine($"Current family {fi.Name}, id {fi.Id}");
                        Level baseLevel = LevelUtils.GetLevelOfElement(fi);
                        double elev = 0;

                        if (baseLevel != null) //обычные семейства на основе стены
                        {
                            elev = LevelUtils.GetOffsetFromBaseLevel(fi);

                        }
                        else //семейства без основы - худший вариант; у семейства нет ни уровня, ни основы. ищу ближайший уровень через координаты
                        {
                            Trace.WriteLine("Family is has no base level!");
                            LocationPoint lp = fi.Location as LocationPoint;
                            if (lp == null)
                            {
                                Trace.WriteLine("No location point");
                                continue;
                            }

                            XYZ point = lp.Point;
                            Trace.WriteLine("Location point: " + point.ToString());
                            baseLevel = LevelUtils.GetNearestLevel(point, doc);

                            if (baseLevel == null)
                            {
                                message += $"{MyStrings.ErrorNoLevel1} {fi.Name} id {fi.Id} {MyStrings.ErrorNoLevel2} {point.Z * 304.8}";
                                Trace.WriteLine($"Failed to get level. {message}");
                                elements.Insert(fi);
                            }
                            elev = point.Z - baseLevel.Elevation - projectPointElevation;
                            Trace.WriteLine($"Nearest level id: {baseLevel.Id}, elevation: {elev.ToString("F1")}");
                        }

                        GetParameter(fi, cfg.paramElevOnLevel, true).Set(elev);
                        GetParameter(fi, cfg.paramBaseLevel, true).Set(baseLevel.Elevation);
                        count++;
                    }
                }
                Trace.WriteLine("Families found: " + count);

                if (cfg.useWallAndColumns)
                {
                    Trace.WriteLine("Start wall and columns processing");
                    foreach (Element elem in ColumnsAndWalls)
                    {
                        Trace.WriteLine($"Current elem id: {elem.Id}");

                        Level baseLevel = LevelUtils.GetLevelOfElement(elem);
                        if (baseLevel == null)
                        {
                            Trace.WriteLine("Failed to find base level");
                            continue;
                        }

                        double baseLevElev = baseLevel.Elevation;
                        double baseLevelOffset = LevelUtils.GetOffsetFromBaseLevel(elem);
                        double baseElev = baseLevElev + baseLevelOffset;
                        bool baseElevSuccess = SetElevParamValue(elem, baseElev, cfg.paramBottomElevName, cfg);
                        if (baseElevSuccess)
                            ColumnAndWallsCount++;

                        double topElev = 0;
                        Level topLevel = LevelUtils.GetTopLevel(elem);
                        if (topLevel != null)
                        {
                            double topLevElevation = topLevel.Elevation;
                            double topOffset = LevelUtils.GetOffsetFromTopLevel(elem);
                            topElev = topLevElevation + topOffset;
                        }
                        else
                        {
                            Trace.WriteLine("No top level, try to get height");
                            double height = LevelUtils.GetElementHeight(elem);
                            if (height != 0)
                            {
                                topElev = baseElev + height;
                            }
                            else
                            {
                                Trace.WriteLine("Failed to get top elevation");
                                continue;
                            }
                        }

                        SetElevParamValue(elem, topElev, cfg.paramTopElevName, cfg);
                    }
                    Trace.WriteLine("Walls and columns done: " + ColumnAndWallsCount);
                }

                t.Commit();
            }

            string msg = MyStrings.MessageOpeningsFound + famsHoles.Count.ToString()
                + "\n" + MyStrings.MessageFamiliesProceed + count.ToString()
                + "\n" + MyStrings.MessageFamiliesNotProceed + err;
            if (cfg.useWallAndColumns)
            {
                msg += "\n" + MyStrings.MessageWallAndColumns + ColumnAndWallsCount;
            }
            Trace.WriteLine(msg);
            Tools.Forms.BalloonTip.Show("Holes elevation", msg);

            saver.Save(cfg);
            return Result.Succeeded;
        }

        private Parameter GetParameter(Element elem, string paramname, bool checkForWritable = false)
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
            if (checkForWritable && param != null && param.IsReadOnly)
            {
                Trace.WriteLine("Parameter is readonly: " + paramname);
            }
            return param;
        }

        private bool SetElevParamValue(Element elem, double elevation, string paramName, Config cnf)
        {
            Parameter userParam = GetParameter(elem, paramName);
            if (userParam == null)
            {
                Trace.WriteLine($"No parameter {paramName} in element {elem.Id}");
                return false;
            }
            if (userParam.IsReadOnly)
            {
                Trace.WriteLine($"Failed to write {paramName} in element {elem.Id}");
                return false;
            }

#if R2017 || R2018 || R2019 || R2020
            double elevMm = UnitUtils.ConvertFromInternalUnits(elevation, DisplayUnitType.DUT_MILLIMETERS);
#else
            double elevMm = UnitUtils.ConvertFromInternalUnits(elevation, UnitTypeId.Millimeters);
#endif


            if (cnf.elevIsCurrency)
                userParam.Set(elevMm);
            else
                userParam.Set(elevation);

            Trace.WriteLine(paramName + " = " + elevMm.ToString("F2"));
            return true;
        }
    }
}
