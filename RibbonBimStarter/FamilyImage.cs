#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-NonСommercial-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных 
в некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2021, все права защищены.
This code is listed under the Creative Commons Attribution-NonСommercial-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2021, all rigths reserved.*/
#endregion
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Autodesk.Revit.DB;
using System.Drawing.Imaging;
using DrawingRectangle = System.Drawing.Rectangle;
using DrawingColor = System.Drawing.Color;
#endregion

namespace RibbonBimStarter
{
    public static class FamilyImage
    {
        public static string ImageCreate(Document FamilyDoc)
        {
            using (Transaction t = new Transaction(FamilyDoc))
            {
                t.Start("Создание вида для экспорта");

                View view = null;
                bool is2Dfamily = false;
                try
                {
                    view = Create3DView(FamilyDoc, "EXPORT");
                }
                catch
                {
                    view = GetAnyView(FamilyDoc);
                    is2Dfamily = true;
                }

                if (view == null)
                    return null;

                try
                {
                    view.DisplayStyle = DisplayStyle.ShadingWithEdges;
                    view.Scale = 5;
                    view.DetailLevel = ViewDetailLevel.Fine;
                }
                catch { }

                try
                {
                    if (is2Dfamily)
                    {
                        view.SetCategoryHidden(new ElementId(BuiltInCategory.OST_Dimensions), true);
                        view.SetCategoryHidden(new ElementId(BuiltInCategory.OST_ReferenceLines), true);
                        view.SetCategoryHidden(new ElementId(BuiltInCategory.OST_CLines), true);
                    }
                    else
                    {
                        view.AreAnnotationCategoriesHidden = true;
                    }
                }
                catch { }

                try
                {
                    List<ElementId> ids = new FilteredElementCollector(FamilyDoc)
                        .OfClass(typeof(ConnectorElement))
                        .ToElementIds()
                        .ToList();
                    FamilyDoc.Delete(ids);
                }
                catch { }


                if (!is2Dfamily)
                {
                    try
                    {
                        List<CurveElement> lines = new FilteredElementCollector(FamilyDoc)
                            .OfCategory(BuiltInCategory.OST_Lines)
                            .OfClass(typeof(CurveElement))
                            .WhereElementIsNotElementType()
                            .Cast<CurveElement>()
                            .ToList();

                        List<ElementId> lineIds = new List<ElementId>();
                        foreach (CurveElement line in lines)
                        {
                            if (line.CurveElementType == CurveElementType.ModelCurve
                                || line.CurveElementType == CurveElementType.ReferenceLine)
                            {
                                lineIds.Add(line.Id);
                            }
                        }
                        FamilyDoc.Delete(lineIds);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
                

                t.Commit();


                IList<ElementId> viewsId = new List<ElementId>();
                viewsId.Add(view.Id);

                string name = FamilyDoc.Title.Replace(".rfa", "");
                string folder = Path.GetDirectoryName(FamilyDoc.PathName);


                ImageExportOptions opt = new ImageExportOptions
                {
                    ZoomType = ZoomFitType.FitToPage,
                    PixelSize = 1000,
                    FilePath = Path.Combine(folder, name),
                    FitDirection = FitDirectionType.Vertical,
                    HLRandWFViewsFileType = ImageFileType.JPEGLossless,
                    ImageResolution = ImageResolution.DPI_300,
                    ExportRange = ExportRange.SetOfViews
                };

                opt.SetViewsAndSheets(viewsId);


                FamilyDoc.ExportImage(opt);

                string viewTypename = FamilyDoc.GetElement(view.GetTypeId()).Name;
                string imagepathjpg = Path.Combine(folder, name + " - " + viewTypename + " - " + view.Name + ".jpg");

                if (!System.IO.File.Exists(imagepathjpg))
                {
                    if (!System.IO.Directory.Exists(folder))
                        throw new Exception("No folder " + folder);
                    string[] files = System.IO.Directory.GetFiles(folder);
                    foreach (string file in files)
                    {
                        string shortname = System.IO.Path.GetFileName(file);
                        if (shortname.StartsWith(name))
                        {
                            imagepathjpg = file;
                            break;
                        }
                    }
                }

                return imagepathjpg;
            }
        }


        private static View3D Create3DView(Document doc, string name)
        {
            List<View3D> views = FindViews(doc, name);

            if (views.Count > 0)
            {
                View3D existView = views.First();
                return existView;
            }

            FilteredElementCollector col2 = new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType));
            List<ViewFamilyType> viewTypes = col2.Cast<ViewFamilyType>().ToList();

            viewTypes = viewTypes.Where(i => i.ViewFamily == ViewFamily.ThreeDimensional).ToList();

            ViewFamilyType viewType = viewTypes.First();

            ElementId viewTypeId = viewType.Id;

            View3D view = View3D.CreateIsometric(doc, viewTypeId);

            view.Name = name;

            return view;
        }

        private static List<View3D> FindViews(Document doc, string name)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            IList<Element> elems = col.OfClass(typeof(View3D)).ToElements();
            List<View3D> views = col.Cast<View3D>().ToList();
            views = views.Where(i => i.Name.Equals(name)).ToList();
            views = views.Where(i => !i.IsTemplate).ToList();

            return views;
        }


        private static View GetAnyView(Document FamilyDoc)
        {
            List<ViewPlan> views = new FilteredElementCollector(FamilyDoc)
                .OfClass(typeof(ViewPlan))
                .Cast<ViewPlan>().ToList();

            if (views.Count != 0)
                return views.First() as View;

            List<ViewSheet> sheets = new FilteredElementCollector(FamilyDoc)
                .OfClass(typeof(ViewSheet))
                .Cast<ViewSheet>().ToList();

            if (sheets.Count != 0)
                return sheets.First() as View;

            return null;
        }

        public static string FitImageInSquare(string jpgpath, int size, int quality)
        {
            string finalimgpath = "";
            using (Image curimg = Image.FromFile(jpgpath) as Bitmap)
            {
                DrawingRectangle curRect = new DrawingRectangle(0, 0, curimg.Width, curimg.Height);
                DrawingRectangle newRect = new DrawingRectangle();
                Bitmap targetBitmap = new Bitmap(size, size);
                if (curimg.Width > curimg.Height) //изображение широкое и низкое
                {
                    double coef = (double)curimg.Width / (double)size;
                    int newheight = (int)(curimg.Height / coef);
                    int topFieldHeight = (size - newheight) / 2;
                    newRect = new DrawingRectangle(0, topFieldHeight, size, newheight);
                }
                else //изображение узкое и высокое
                {
                    double coef = (double)curimg.Height / (double)size;
                    int newwidth = (int)(curimg.Width / coef);
                    int leftFieldWidth = (size - newwidth) / 2;
                    newRect = new DrawingRectangle(leftFieldWidth, 0, newwidth, size);
                }

                using (Bitmap sourceBitmap = new Bitmap(curimg, curimg.Width, curimg.Height))
                {
                    using (Graphics g = Graphics.FromImage(targetBitmap))
                    {
                        SolidBrush sb = new SolidBrush(DrawingColor.White);
                        g.FillRectangle(sb, 0, 0, size, size);
                        g.DrawImage(sourceBitmap, newRect, curRect, GraphicsUnit.Pixel);
                    }
                }
                finalimgpath = jpgpath.Replace(".jpg", "_" + size.ToString() + ".jpg");

                ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, (long)quality);
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = myEncoderParameter;

                targetBitmap.Save(finalimgpath, myImageCodecInfo, myEncoderParameters);
            }
            return finalimgpath;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
