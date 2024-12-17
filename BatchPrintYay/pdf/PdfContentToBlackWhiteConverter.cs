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
using System.Threading.Tasks;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
using System.Drawing;
using System.Drawing.Imaging;


namespace BatchPrintYay.pdf
{
    public class PdfContentToBlackWhiteConverter
    {
        #region Private variables

        private static ColorMatrix _colorMatrix = new ColorMatrix(
                            new float[][]
                            {
//                                 new float[] { 1.5f, 1.5f, 1.5f, 0, 0},
//                                 new float[] { 1.5f, 1.5f, 1.5f, 0, 0},
//                                 new float[] { 1.5f, 1.5f, 1.5f, 0, 0},
//                                 new float[] { 0, 0, 0, 1, 0},
//                                 new float[] { -1, -1, -1, 0, 1}

                                new float[] {.3f, .3f, .3f, 0, 0},
                                new float[] {.59f, .59f, .59f, 0, 0},
                                new float[] {.11f, .11f, .11f, 0, 0},
                                new float[] {0, 0, 0, 1, 0},
                                new float[] {0, 0, 0, 0, 1}
                            });

        private PdfContentModifier _modifier = new PdfContentModifier();
        private List<int> _convertedIndirectList = new List<int>();
        public List<Color> _excludeColors = new List<Color>();

        #endregion

        #region Constructor

        public PdfContentToBlackWhiteConverter()
        {
            _modifier.RegisterOperator("G", SetStrokingGray);
            _modifier.RegisterOperator("g", SetNonStrokingGray);
            _modifier.RegisterOperator("RG", SetStrokingRGB);
            _modifier.RegisterOperator("rg", SetNonStrokingRGB);
            _modifier.RegisterOperator("K", SetStrokingCMYK);
            _modifier.RegisterOperator("k", SetNonStrokingCMYK);
            _modifier.RegisterOperator("CS", SetStrokingGeneral);
            _modifier.RegisterOperator("cs", SetNonStrokingGeneral);
            _modifier.RegisterOperator("SC", SetStrokingGeneral);
            _modifier.RegisterOperator("sc", SetNonStrokingGeneral);
            _modifier.RegisterOperator("SCN", SetStrokingGeneral);
            _modifier.RegisterOperator("scn", SetNonStrokingGeneral);
            _modifier.RegisterOperator("Do", Do);
            _modifier.RegisterOperator("l", SetLineTo);
            _modifier.RegisterOperator("re", SetReactangle);
        }

        #endregion

        #region Convert

        public void Convert(PdfReader reader, int pageNumber, List<Color> excludeColors)
        {
            _excludeColors.Clear();
            foreach (Color c in excludeColors)
                _excludeColors.Add(c);

            _modifier.Modify(reader, pageNumber);
        }

        private bool IsKeepColor(BaseColor cr)
        {
            bool bKeep = false;

            foreach (Color c in _excludeColors)
            {
                if (c.ToArgb() == cr.RGB)
                {
                    bKeep = true;
                    break;
                }
            }

            return bKeep;
        }
        private bool IsKeepColor(Color cr)
        {
            int nColor = cr.ToArgb();
            if (nColor == Color.White.ToArgb() || nColor == Color.Black.ToArgb())
                return true;

            byte r = cr.R;
            byte g = cr.G;
            byte b = cr.B;

            if (r == g && g == b)
                return true;

            bool bKeep = false;
            foreach (Color c in _excludeColors)
            {
                if (c.ToArgb() == cr.ToArgb())
                {
                    bKeep = true;
                    break;
                }
            }

            return bKeep;
        }
        #endregion

        #region SetStrokingGray

        private List<PdfObject> SetStrokingGray(PdfLiteral oper, List<PdfObject> operands)
        {
            //             return new List<PdfObject>()
            //                 {
            //                     new PdfNumber(0),
            //                     new PdfLiteral("G")
            //                 };
            return operands;
        }

        #endregion

        #region SetNonStrokingGray

        private List<PdfObject> SetNonStrokingGray(PdfLiteral oper, List<PdfObject> operands)
        {
            //             return new List<PdfObject>()
            //                 {
            //                     new PdfNumber(0),
            //                     new PdfLiteral("g")
            //                 };

            return operands;
        }

        #endregion

        #region SetStrokingRGB

        private List<PdfObject> SetStrokingRGB(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfNumber r = (PdfNumber)operands[0];
            PdfNumber g = (PdfNumber)operands[1];
            PdfNumber b = (PdfNumber)operands[2];


            BaseColor color = new BaseColor(r.FloatValue, g.FloatValue, b.FloatValue);
            if (IsKeepColor(color))
            {
                return operands;
            }
            else
            {
                return new List<PdfObject>()
                {
                     new PdfNumber(0),
                     new PdfNumber(0),
                     new PdfNumber(0),
                     new PdfLiteral("RG")
                };
            }
        }

        #endregion

        #region SetNonStrokingRGB

        private List<PdfObject> SetNonStrokingRGB(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfNumber r = (PdfNumber)operands[0];
            PdfNumber g = (PdfNumber)operands[1];
            PdfNumber b = (PdfNumber)operands[2];
            BaseColor color = new BaseColor(r.FloatValue, g.FloatValue, b.FloatValue);

            if (IsKeepColor(color))
            {
                return operands;
            }
            else
            {
                return new List<PdfObject>()
                {
                    new PdfNumber(0),
                    new PdfNumber(0),
                    new PdfNumber(0),
                    new PdfLiteral("rg")
                };
            }
        }

        #endregion

        #region SetStrokingCMYK

        private List<PdfObject> SetStrokingCMYK(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfNumber c = (PdfNumber)operands[0];
            PdfNumber m = (PdfNumber)operands[1];
            PdfNumber y = (PdfNumber)operands[2];
            PdfNumber k = (PdfNumber)operands[3];
            CMYKColor color = new CMYKColor(c.FloatValue, m.FloatValue, y.FloatValue, k.FloatValue);

            return new List<PdfObject>()
            {
                new PdfNumber(0),
                new PdfNumber(0),
                new PdfNumber(0),
                new PdfLiteral("RG")
            };
        }

        #endregion

        #region SetNonStrokingCMYK

        private List<PdfObject> SetNonStrokingCMYK(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfNumber c = (PdfNumber)operands[0];
            PdfNumber m = (PdfNumber)operands[1];
            PdfNumber y = (PdfNumber)operands[2];
            PdfNumber k = (PdfNumber)operands[3];
            CMYKColor color = new CMYKColor(c.FloatValue, m.FloatValue, y.FloatValue, k.FloatValue);

            return new List<PdfObject>()
                {
                    new PdfNumber(0),
                    new PdfNumber(0),
                    new PdfNumber(0),
                    new PdfLiteral("rg")
                };
        }

        #endregion

        #region SetStrokingColorSpace

        private List<PdfObject> SetStrokingColorSpace(PdfLiteral oper, List<PdfObject> operands)
        {
            return new List<PdfObject>()
            {
                new PdfNumber(0),
                new PdfNumber(0),
                new PdfNumber(0),
                new PdfLiteral("CS")
            };
        }

        #endregion

        #region SetStrokingGeneral

        private List<PdfObject> SetStrokingGeneral(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfLiteral opername = (PdfLiteral)operands[operands.Count - 1];

            return new List<PdfObject>()
            {
                new PdfNumber(0),
                new PdfNumber(0),
                new PdfNumber(0),
                opername
            };
        }

        #endregion

        #region SetNonStrokingGeneral

        private List<PdfObject> SetNonStrokingGeneral(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfLiteral opername = (PdfLiteral)operands[operands.Count - 1];

            return new List<PdfObject>()
            {
                new PdfNumber(0),
                new PdfNumber(0),
                new PdfNumber(0),
                opername
            };
        }

        #endregion

        #region Do

        private List<PdfObject> Do(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfName xobjectName = (PdfName)operands[0];

            //System.Diagnostics.Trace.WriteLine("Opr: " + xobjectName.ToString());

            PdfDictionary xobjects = _modifier.ResourceDictionary.GetAsDict(PdfName.XOBJECT);
            PdfObject po = xobjects.Get(xobjectName);

            int n = (po as PdfIndirectReference).Number;

            if (_convertedIndirectList.Contains(n))
            {
                return operands;
            }
            else
            {
                _convertedIndirectList.Add(n);
            }

            PdfObject xobject = xobjects.GetDirectObject(xobjectName);

            if (xobject.IsStream())
            {
                PdfStream xobjectStream = (PdfStream)xobject;
                PdfName subType = xobjectStream.GetAsName(PdfName.SUBTYPE);

                if (subType == PdfName.FORM)
                {
                    this.Do_Form(xobjectStream);
                }
                else if (subType == PdfName.IMAGE)
                {
                    //this.Do_Image(xobjectStream); //картинки обрабатывать не буду
                }
            }

            return operands;
        }

        #endregion

        #region Do_Form

        private void Do_Form(PdfStream stream)
        {
            PdfDictionary resources = stream.GetAsDict(PdfName.RESOURCES);

            byte[] contentBytes = ContentByteUtils.GetContentBytesFromContentObject(stream);

            contentBytes = _modifier.Modify(contentBytes, resources);

            PRStream prStream = stream as PRStream;

            prStream.SetData(contentBytes);
        }

        #endregion

        /* Doing images isnt needed
        static int bmpfileidx = 0;
        
        private void Do_Image(PdfStream stream)
        {
            byte[] imageBuffer = null;

            PdfName filter = PdfName.NONE;

            if (stream.Contains(PdfName.FILTER))
            {
                filter = stream.GetAsName(PdfName.FILTER);
            }

            int imageWidth = stream.GetAsNumber(PdfName.WIDTH).IntValue;
            int imageHeight = stream.GetAsNumber(PdfName.HEIGHT).IntValue;
            int imageBpp = stream.GetAsNumber(PdfName.BITSPERCOMPONENT).IntValue;

            PRStream prStream = stream as PRStream;

            bool cannotReadImage = false;

            Bitmap image = null;

            try
            {
                PdfImageObject pdfImage = new PdfImageObject(prStream);
                image = pdfImage.GetDrawingImage() as Bitmap;


                string strFile = "C:\\PDF_Print\\pdf" + bmpfileidx.ToString() + ".bmp";
                image.Save(strFile);
                bmpfileidx++;
            }
            catch
            {
                try
                {
                    if (filter == PdfName.FLATEDECODE)
                    {
                        byte[] streamBuffer = PdfReader.GetStreamBytes(prStream);
                        image = this.CreateBitmapFromFlateDecodeImage(streamBuffer, imageWidth, imageHeight, imageBpp);

                        string strFile = "C:\\PDF_Print\\pdf_" + bmpfileidx.ToString() + ".bmp";
                        image.Save(strFile);
                        bmpfileidx++;
                    }
                }
                catch
                {
                    cannotReadImage = true;
                }
            }

            if (!cannotReadImage)
            {
                image = this.ConvertToGrayscale(image);
                //image = this.ConvertToBlackWhite(image);

                using (var ms = new MemoryStream())
                {
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    image.Save(ms, jgpEncoder, myEncoderParameters);

                    imageBuffer = ms.ToArray();
                    //imageBuffer = ms.GetBuffer();

                    iTextSharp.text.Image newImage = iTextSharp.text.Image.GetInstance(imageBuffer);
                    newImage.SimplifyColorspace();

                    //                      PdfImage tempPdfImage = new PdfImage(newImage, newImage.ToString(), null);
                    //                      prStream.Clear();
                    //                      prStream.SetDataRaw(imageBuffer);
                    //                      prStream.Merge(tempPdfImage);

                    //                     imageBuffer = newImage.OriginalData;
                    // 
                    prStream.Clear();
                    prStream.SetData(imageBuffer, false, PRStream.NO_COMPRESSION);
                    prStream.Put(PdfName.TYPE, PdfName.XOBJECT);
                    prStream.Put(PdfName.SUBTYPE, PdfName.IMAGE);
                    prStream.Put(PdfName.FILTER, PdfName.DCTDECODE);
                    prStream.Put(PdfName.WIDTH, new PdfNumber(image.Width));
                    prStream.Put(PdfName.HEIGHT, new PdfNumber(image.Height));
                    prStream.Put(PdfName.BITSPERCOMPONENT, new PdfNumber(8));
                    prStream.Put(PdfName.COLORSPACE, PdfName.DEVICERGB);
                    prStream.Put(PdfName.LENGTH, new PdfNumber(imageBuffer.LongLength));
                }
            }
        } */


        #region SetLineTo

        private List<PdfObject> SetLineTo(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfNumber x = (PdfNumber)operands[0];
            PdfNumber y = (PdfNumber)operands[1];

            return new List<PdfObject>()
            {
                x,
                y,
                new PdfLiteral("l")
            };
        }

        #endregion

        #region SetReactangle

        private List<PdfObject> SetReactangle(PdfLiteral oper, List<PdfObject> operands)
        {
            PdfNumber x = (PdfNumber)operands[0];
            PdfNumber y = (PdfNumber)operands[1];
            PdfNumber w = (PdfNumber)operands[2];
            PdfNumber h = (PdfNumber)operands[3];

            return new List<PdfObject>()
            {
                x,
                y,
                w,
                h,
                new PdfLiteral("re")
            };
        }

        #endregion

        /*
        private Bitmap CreateBitmapFromFlateDecodeImage(byte[] buffer, int width, int height, int bpp)
        {
            PixelFormat pixelFormat;

            switch (bpp)
            {
                case 1:
                    {
                        pixelFormat = PixelFormat.Format1bppIndexed;
                        break;
                    }
                case 8:
                    {
                        pixelFormat = PixelFormat.Format8bppIndexed;
                        break;
                    }
                default:
                    {
                        pixelFormat = PixelFormat.Format24bppRgb;
                        break;
                    }
            }

            using (Bitmap bmp = new Bitmap(width, height, pixelFormat))
            {
                var bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);

                int length = (int)Math.Ceiling(width * bpp / 8.0);

                for (int i = 0; i < height; i++)
                {
                    int offset = i * length;
                    int scanOffset = i * bmpData.Stride;

                    System.Runtime.InteropServices.Marshal.Copy(buffer, offset, new IntPtr(bmpData.Scan0.ToInt32() + scanOffset), length);
                }

                bmp.UnlockBits(bmpData);

                return bmp.Clone() as Bitmap;
            }
        }
        


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }


        
        private Bitmap ConvertToGrayscale(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(_colorMatrix);
            //attributes.SetThreshold(0.85f);
            g.DrawImage(original, new System.Drawing.Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();

            string strFile = "C:\\PDF_Print\\pdfgray_" + bmpfileidx.ToString() + ".bmp";
            newBitmap.Save(strFile);
            bmpfileidx++;
            

            return newBitmap;
        } */

        /*
        private Bitmap ConvertToBlackWhite(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int h = 0; h < original.Height; h++)
            {
                for (int w = 0; w < original.Width; w++)
                {
                    Color pixelColor = original.GetPixel(w, h);
                    if (IsKeepColor(pixelColor))
                        newBitmap.SetPixel(w, h, pixelColor);
                    else
                        newBitmap.SetPixel(w, h, Color.Black);
                }
            }

            string strFile = "C:\\PDF_Print\\pdfbw_" + bmpfileidx.ToString() + ".bmp";
            newBitmap.Save(strFile);
            bmpfileidx++;

            return newBitmap;
        }
        */



        /*
        private BaseColor Convert_CMYK_To_RGB(float c, float m, float y, float k)
        {
            int red = (int)((1 - c) * (1 - k) * 255.0);
            int green = (int)((1 - m) * (1 - k) * 255.0);
            int blue = (int)((1 - y) * (1 - k) * 255.0);

            return new BaseColor(red, green, blue);
        }
        private GrayColor Convert_RGB_To_Grayscale(float r, float g, float b)
        {
            return this.Convert_RGB_To_Grayscale(new BaseColor(r, g, b));
        }
        

        private GrayColor Convert_RGB_To_Grayscale(BaseColor color)
        {
            return new GrayColor((int)((color.R * 0.30f) + (color.G * 0.59) + (color.B * 0.11)));
        }
        */
    }
}
