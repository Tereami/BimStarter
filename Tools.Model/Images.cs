using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tools.Model
{
    public static class Images
    {
        public static ImageType GetImageTypeByName(Document doc, string name)
        {
            Trace.WriteLine("Try to get image by name: " + name);
            List<ImageType> images = new FilteredElementCollector(doc)
                .OfClass(typeof(ImageType))
                .Cast<ImageType>()
                .Where(i => i.Name.Equals(name))
                .ToList();
            Trace.WriteLine("Try to find image by name: " + name + ", found: " + images.Count.ToString());
            if (images.Count == 0)
            {
                List<ImageType> errImgs = new FilteredElementCollector(doc)
                    .OfClass(typeof(ImageType))
                    .Cast<ImageType>()
                    .Where(i => i.Name.Equals("Ошибка.png"))
                    .ToList();
                if (errImgs.Count == 0)
                {
                    Trace.WriteLine("Unable to find image Ошибка.png");
                    throw new Exception("Unable to find image Ошибка.png");
                }

                ImageType errImg = errImgs.First();
                return errImg;
            }
            ImageType it = images.First();
            Trace.WriteLine($"Image is found, id: {it.Id}");
            return it;
        }
    }
}
