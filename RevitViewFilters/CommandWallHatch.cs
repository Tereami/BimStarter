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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Tools.Geometry;
#endregion

namespace RevitViewFilters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    class CommandWallHatch : IExternalCommand
    {
        private string FiltersPrefix = "_cwh_";
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("WallHatch"));
            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            View curView = doc.ActiveView;
            if (!(curView is ViewPlan))
            {
                message = MyStrings.ErrorOnlyViewplan;
                Debug.WriteLine(message);
                return Result.Failed;
            }

            if (curView.ViewTemplateId != null && curView.ViewTemplateId != ElementId.InvalidElementId)
            {
                message = MyStrings.ErrorViewTemplate;
                Debug.WriteLine(message);
                return Result.Failed;
            }

            List<Wall> walls = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();
            Debug.WriteLine("Walls count: " + walls.Count);
            if (walls.Count == 0)
            {
                message = MyStrings.ErrorWallsNotSelected;
                return Result.Failed;
            }

            Dictionary<string, List<Wall>> wallsDict = new Dictionary<string, List<Wall>>();

            Tools.SettingsSaver.Saver<WallHatchSettings> saver = new Tools.SettingsSaver.Saver<WallHatchSettings>();
            WallHatchSettings sets = saver.Activate("WallHatch");
            FormWallHatchSettings form = new FormWallHatchSettings(sets);
            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return Result.Cancelled;
            }
            sets = form.Sets;

            foreach (Wall w in walls)
            {
                Debug.WriteLine($"Current wall id: {w.Id}");
                string key = "wall_";
                HeightResult heightResult = Tools.Geometry.Height.GetMaxMinHeightPoints(w);

                if (sets.UseType)
                {
                    key += w.WallType.Name;
                }
                if (sets.UseHeight)
                {
                    key += "_h" + heightResult.HeightMm.ToString("0.#");
                }
                if (sets.UseBaseLevel)
                {
                    key += "_b" + heightResult.BottomElevationMm.ToString("0.#");
                }
                if (sets.UseThickness)
                {
                    double t = w.Width * 304.8;
                    key += "_t" + t.ToString("0.#");
                }
                Debug.Write("Key: " + key);

                if (wallsDict.ContainsKey(key))
                {
                    wallsDict[key].Add(w);
                }
                else
                {
                    wallsDict.Add(key, new List<Wall> { w });
                }
            }

            int keysCount = wallsDict.Count;

            OrderedDictionary wallsOrdered = new OrderedDictionary();
            foreach (var kv in wallsDict.OrderByDescending(kv => kv.Value.Count))
                wallsOrdered.Add(kv.Key, kv.Value);

            Debug.Write("Wall types found: " + keysCount);
            Dictionary<int, FillPatternElement> hatches = CollectHatches(doc, sets.HatchPrefix, keysCount);
            Dictionary<int, ImageType> images = CollectImages(doc, sets.ImagePrefix, keysCount);

            List<ElementId> catsIds = new List<ElementId> { new ElementId(BuiltInCategory.OST_Walls) };

            using (Transaction t = new Transaction(doc))
            {
                t.Start(MyStrings.TransactionWallsElevation);
                Debug.WriteLine("Transaction start: " + MyStrings.TransactionWallsElevation);

                foreach (ElementId filterId in curView.GetFilters())
                {
                    ParameterFilterElement filter = doc.GetElement(filterId) as ParameterFilterElement;
                    if (filter.Name.StartsWith(FiltersPrefix))
                    {
                        doc.Delete(filter.Id);
                        Debug.WriteLine("Old filter deleted: " + filter.Name);
                    }
                }

                int i = 1;
                foreach (DictionaryEntry kvp in wallsOrdered)
                {
                    string key = (string)kvp.Key;
                    List<Wall> curWalls = (List<Wall>)kvp.Value;
                    Debug.WriteLine("Current key: " + kvp.Key + ", walls count: " + curWalls.Count);
                    FillPatternElement hatch = hatches[i];
                    ElementId hatchId = hatch.Id;
                    ImageType image = images[i];

                    foreach (Wall w in curWalls)
                    {
                        Debug.WriteLine("Assign image to wall id " + w.Id);
                        w.get_Parameter(BuiltInParameter.ALL_MODEL_IMAGE).Set(image.Id);
                    }

                    OverrideGraphicSettings ogs = new OverrideGraphicSettings();

#if R2017 || R2018
                    ogs.SetProjectionFillPatternId(hatchId);
                    ogs.SetCutFillPatternId(hatchId);
#else
                    ogs.SetSurfaceForegroundPatternId(hatchId);
                    ogs.SetCutForegroundPatternId(hatchId);
#endif
                    foreach (Wall w in curWalls)
                    {
                        Debug.WriteLine("Assign graphics override to wall id " + w.Id);
                        doc.ActiveView.SetElementOverrides(w.Id, ogs);
                    }

                    i++;
                }
                t.Commit();
                Debug.WriteLine("Transaction committed: " + MyStrings.TransactionWallsElevation);
            }
            saver.Save(sets);
            Debug.WriteLine("Settings saved");

            string msg = MyStrings.WallHatchFinalMsg1 + walls.Count + MyStrings.WallHatchFinalMsg2 + wallsOrdered.Count;

            Tools.Forms.BalloonTip.Show("Walls hatch", msg);

            return Result.Succeeded;
        }


        private Dictionary<int, FillPatternElement> CollectHatches(Document doc, string prefix, int count)
        {
            Dictionary<int, FillPatternElement> patternsDict = new Dictionary<int, FillPatternElement>();
            Debug.WriteLine("Try to find fill patterns: " + prefix);
            List<FillPatternElement> fpes = new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement))
                .Where(i => i.Name.StartsWith(prefix))
                .Cast<FillPatternElement>()
                .ToList();
            if (fpes.Count == 0)
            {
                string errMsg = MyStrings.ErrorNoHatch + prefix;
                Debug.WriteLine(errMsg);
                TaskDialog.Show("Error", errMsg);
                throw new Exception(errMsg);
            }

            for (int i = 1; i <= count; i++)
            {
                string hatchName = prefix + i;
                FillPatternElement fpe = fpes
                    .FirstOrDefault(p => p.Name == hatchName);
                if (fpe == null)
                {
                    string errMsg = MyStrings.ErrorNoHatch + hatchName;
                    TaskDialog.Show("Error", errMsg);
                    Debug.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                Debug.WriteLine("Number " + i + ", fill pattern found: " + fpe.Name + " id " + fpe.Id);
                patternsDict.Add(i, fpe);
            }
            Debug.WriteLine("Fill patterns found: " + patternsDict.Count);
            return patternsDict;
        }

        private Dictionary<int, ImageType> CollectImages(Document doc, string prefix, int count)
        {
            Dictionary<int, ImageType> imagesDict = new Dictionary<int, ImageType>();
            Debug.WriteLine("Try to find images: " + prefix);

            List<ImageType> images = new FilteredElementCollector(doc)
                .OfClass(typeof(ImageType))
                .Cast<ImageType>()
                .Where(i => i.Name.StartsWith(prefix))
                .ToList();
            if (images.Count == 0)
            {
                string errmsg = "NO IMAGES " + prefix;
                Debug.WriteLine(errmsg);
                TaskDialog.Show("Error", errmsg);
                throw new Exception(errmsg);
            }

            for (int i = 1; i <= count; i++)
            {
                string imageName = prefix + i.ToString() + ".png";
                ImageType curImage = images
                    .FirstOrDefault(img => img.Name == imageName);
                if (curImage == null)
                {
                    string errMsg = MyStrings.ErrorNoImage + imageName;
                    TaskDialog.Show("Error", errMsg);
                    Debug.WriteLine(errMsg);
                    throw new Exception(errMsg);
                }
                Debug.WriteLine("Number " + i + ", image found: " + curImage.Name + " id " + curImage.Id);
                imagesDict.Add(i, curImage);
            }
            Debug.WriteLine("Images found: " + imagesDict.Count);
            return imagesDict;
        }
    }
}