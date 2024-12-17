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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;



namespace BatchPrintYay.pdf
{
    public static class PdfWorker
    {
        public static List<Color> _excludeColors = new List<Color>();

        public static void SetExcludeColors(List<PdfColor> colors)
        {
            _excludeColors = colors.Select(i => i.GetColor()).ToList();
        }


        public static void ConvertToGrayScale(string pdfPathIn, string pdfPathOut)
        {
            // временные файлы создаются в папке приложения
            PdfContentToBlackWhiteConverter grayscaleConverter = new PdfContentToBlackWhiteConverter();
            PdfReader reader = new PdfReader(pdfPathIn);

            using (FileStream fsOutput = new FileStream(pdfPathOut, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                Document document = new Document(reader.GetPageSizeWithRotation(1));
                PdfWriter writer = PdfWriter.GetInstance(document, fsOutput);

                document.Open();

                PdfContentByte cb = writer.DirectContent;

                int numberOfPages = reader.NumberOfPages;

                for (int pageNumber = 1; pageNumber <= numberOfPages; pageNumber++)
                {
                    iTextSharp.text.Rectangle pageSizeWithRotation = reader.GetPageSizeWithRotation(pageNumber);

                    document.SetPageSize(pageSizeWithRotation);
                    document.NewPage();

                    // >>> CONVERT CURRENT PAGE TO GRAYSCALE
                    grayscaleConverter.Convert(reader, pageNumber, _excludeColors);
                    // <<<<

                    PdfImportedPage page = writer.GetImportedPage(reader, pageNumber);

                    if (pageSizeWithRotation.Rotation == 90 || pageSizeWithRotation.Rotation == 270)
                    {
                        cb.AddTemplate(page, 0, -1f, 1f, 0, 0, pageSizeWithRotation.Height);
                    }
                    else
                    {
                        cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                    }
                }

                document.Close();
                writer.Close();
                reader.Close();
            }
        }


        public static bool CombineMultiplyPDFs(List<string> names, string outFile)
        {
            bool merged = false;

            using (FileStream stream = new FileStream(outFile, FileMode.Create))
            {
                Document doc = new Document();
                PdfCopy writer = new PdfCopy(doc, stream);
                if (writer == null) throw new Exception("Unable to create file: " + outFile);
                PdfReader reader = null;
                try
                {
                    doc.Open();
                    for (int i = 0; i < names.Count; i++)
                    {
                        string fileName = names[i];
                        Trace.WriteLine("File is processed: " + fileName);
                        reader = new PdfReader(fileName);
                        writer.AddDocument(reader);
                        reader.Close();
                        Trace.WriteLine("File merged succefully");
                    }
                }
                catch
                {
                    if (reader != null)
                    {
                        Trace.WriteLine("Unable to merge file");
                        reader.Close();
                    }
                }
                finally
                {
                    if (doc != null) doc.Close();
                }

            }
            Trace.WriteLine("Merged to file: " + outFile);
            return merged;
        }
    }
}
