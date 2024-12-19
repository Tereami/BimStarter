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

namespace RevitViewFilters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class CommandWallHatch : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Guid topElevParamGuid = new Guid("ec353104-09f1-44f4-85cf-1d638dce02d3");
            Guid bottomElevParamGuid = new Guid("8a58ad74-0e15-499b-bcaf-35b45cd7fc1f");

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("WallHatch"));
            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            View curView = doc.ActiveView;
            if (!(curView is ViewPlan))
            {
                message = MyStrings.ErrorOnlyViewplan;
                Trace.WriteLine(message);
                return Result.Failed;
            }

            if (curView.ViewTemplateId != null && curView.ViewTemplateId != ElementId.InvalidElementId)
            {
                message = MyStrings.ErrorViewTemplate;
                Trace.WriteLine(message);
                return Result.Failed;
            }

            Selection sel = commandData.Application.ActiveUIDocument.Selection;
            Trace.WriteLine("Selected elements: " + sel.GetElementIds().Count);
            if (sel.GetElementIds().Count == 0)
            {
                message = MyStrings.ErrorWallsNotSelected;
                return Result.Failed;
            }
            List<Wall> walls = new List<Wall>();
            foreach (ElementId id in sel.GetElementIds())
            {
                Wall w = doc.GetElement(id) as Wall;
                if (w == null) continue;
                walls.Add(w);
            }
            Trace.WriteLine("Walls count: " + walls.Count);
            if (walls.Count == 0)
            {
                message = MyStrings.ErrorWallsNotSelected;
                return Result.Failed;
            }

            SortedDictionary<double, List<Wall>> wallHeigthDict = new SortedDictionary<double, List<Wall>>();
            if (wallHeigthDict.Count > 10)
            {
                message = MyStrings.ErrorWallsMoreThan10;
                Trace.WriteLine(message);
                return Result.Failed;
            }

            foreach (Wall w in walls)
            {
                Trace.WriteLine($"Current wall id: {w.Id}");
                double topElev = GetWallTopElev(doc, w, true);
                Trace.WriteLine("Top elevation: " + topElev.ToString("F1"));

                if (wallHeigthDict.ContainsKey(topElev))
                {
                    wallHeigthDict[topElev].Add(w);
                }
                else
                {
                    wallHeigthDict.Add(topElev, new List<Wall> { w });
                }
            }

            List<ElementId> catsIds = new List<ElementId> { new ElementId(BuiltInCategory.OST_Walls) };

            int i = 1;
            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionWallsElevation);

                foreach (ElementId filterId in curView.GetFilters())
                {
                    ParameterFilterElement filter = doc.GetElement(filterId) as ParameterFilterElement;
                    if (filter.Name.StartsWith("_cwh_"))
                    {
                        curView.RemoveFilter(filterId);
                    }
                }
                Trace.WriteLine("Old filters deleted");

                foreach (var kvp in wallHeigthDict)
                {
                    Trace.WriteLine("Current key: " + kvp.Key.ToString("F1"));
                    ElementId hatchId = GetHatchIdByNumber(doc, i);
                    ImageType image = GetImageTypeByNumber(doc, i);

                    double curHeigthMm = kvp.Key;
                    double curHeigthFt = curHeigthMm / 304.8;

                    List<Wall> curWalls = kvp.Value;

                    foreach (Wall w in curWalls)
                    {
                        w.get_Parameter(BuiltInParameter.ALL_MODEL_IMAGE).Set(image.Id);
                        Parameter topElevParam = w.get_Parameter(topElevParamGuid);
                        if (topElevParam == null)
                            throw new Exception(MyStrings.ErrorNoTopElevParam);

                        topElevParam.Set(curHeigthFt);

                        double bottomElev = GetWallTopElev(doc, w, false);
                        double bottomElevFt = bottomElev / 304.8;
                        Parameter bottomElevParam = w.get_Parameter(bottomElevParamGuid);
                        if (bottomElevParam == null)
                            throw new Exception(MyStrings.ErrorNoBottomElevParam);

                        bottomElevParam.Set(bottomElevFt);
                    }

                    string filterName = "_cwh_" + MyStrings.FilterTitleWallTopElev + curHeigthMm.ToString("F0");

                    Parameter topElevParamForRule = curWalls.First().get_Parameter(topElevParamGuid);
                    MyParameter mp = new MyParameter(topElevParamForRule);
                    ParameterFilterElement filter =
                        FilterCreator.createSimpleFilter(doc, catsIds, filterName, mp, CriteriaType.Equals);

                    curView.AddFilter(filter.Id);
                    OverrideGraphicSettings ogs = new OverrideGraphicSettings();

#if R2017 || R2018
                    ogs.SetProjectionFillPatternId(hatchId);
                    ogs.SetCutFillPatternId(hatchId);
#else
                    ogs.SetSurfaceForegroundPatternId(hatchId);
                    ogs.SetCutForegroundPatternId(hatchId);
#endif
                    curView.SetFilterOverrides(filter.Id, ogs);
                    i++;
                }
                t.Commit();
            }
            return Result.Succeeded;
        }

        private double GetWallTopElev(Document doc, Wall w, bool TopOrBottomElev)
        {
            Trace.WriteLine($"Try to get wall top elevation, wall id: {w.Id}");
            ElementId levelId = w.LevelId;
            if (levelId == null || levelId == ElementId.InvalidElementId)
                throw new Exception($"{MyStrings.ErrorNoWallBaseLevel} {w.Id}");

            Level lev = doc.GetElement(levelId) as Level;
            double levElev = lev.ProjectElevation;
            double baseOffset = w.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).AsDouble();

            double elev = levElev + baseOffset; // + wallHeigth;

            if (TopOrBottomElev)
            {
                double wallHeigth = w.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
                elev += wallHeigth;
            }

            double elevmm = elev * 304.8;

            double elevRoundMm = Math.Round(elevmm);
            return elevRoundMm;
        }

        private ElementId GetHatchIdByNumber(Document doc, int number)
        {
            string hatchName = GetHatchNameByNumber(doc, number);
            Trace.WriteLine("Hatch name: " + hatchName);
            ElementId hatchId = GetHatchIdByName(doc, hatchName);
            return hatchId;
        }

        private string GetHatchNameByNumber(Document doc, int number)
        {
            if (number == 1) return "Грунт естественный";
            if (number == 2) return "08.Грунт.Гравий";
            if (number == 3) return "05.Кирпич.В разрезе (45град) 3.5мм";
            if (number == 4) return "02.Крест (45град) 2мм";
            if (number == 5) return "06.Древесина 2";
            if (number == 6) return "02.Крест (45град) 1мм";
            if (number == 7) return "01.Диагональ (135град) 1.5мм";
            if (number == 8) return "Грунт: песок плотный";
            if (number == 9) return "01.Диагональ (45град) 1.5мм";
            if (number == 10) return "05.Кирпич.В разрезе (135град) 3.5мм";

            return "Сплошная заливка";
        }

        private ElementId GetHatchIdByName(Document doc, string hatchName)
        {
            List<FillPatternElement> fpes = new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement))
                .Where(i => i.Name.Contains(hatchName))
                .Cast<FillPatternElement>()
                .ToList();
            if (fpes.Count == 0)
            {
                Trace.WriteLine("Unable to find hatch: " + hatchName);
                throw new Exception("Не удалось найти штриховку " + hatchName);
            }
            Trace.WriteLine($"Hatch found:  {fpes.First().Id}");
            return fpes.First().Id;
        }

        private ImageType GetImageTypeByNumber(Document doc, int number)
        {
            string name = "ШтриховкаСтены_" + number.ToString() + ".png";
            Trace.WriteLine("Try to find image: " + name);

            List<ImageType> images = new FilteredElementCollector(doc)
                .OfClass(typeof(ImageType))
                .Cast<ImageType>()
                .Where(i => i.Name.Equals(name))
                .ToList();

            if (images.Count == 0)
            {
                List<ImageType> errImgs = new FilteredElementCollector(doc)
                    .OfClass(typeof(ImageType))
                    .Cast<ImageType>()
                    .Where(i => i.Name.Equals("Ошибка.png"))
                    .ToList();
                if (errImgs.Count == 0)
                {
                    Trace.WriteLine("No wall images in the project");
                    throw new Exception("Загрузите в проект картинки!");
                }

                ImageType errImg = errImgs.First();
                return errImg;
            }
            Trace.WriteLine($"Hatch found:  {images.First().Id}");
            return images.First();
        }
    }
}