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
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Collections.Generic;
using System.Linq;

namespace BatchPrintYay
{
    public static class PrinterUtility
    {
        public static PaperSize GetPaperSize(string printerName, double widthMM, double heigthMM)
        {
            Trace.WriteLine("   trying to get sheet size: width " + widthMM.ToString("F3") + "mm, height " + heigthMM.ToString("F3"));

            int widthInches = (int)Math.Round(100 * widthMM / 25.4);
            int heigthInches = (int)Math.Round(100 * heigthMM / 25.4);

            Trace.WriteLine("   in inches: " + widthInches + " х " + heigthInches);
            PrinterSettings prntSettings = new PrinterSettings();
            prntSettings.PrinterName = printerName;
            PrinterSettings.PaperSizeCollection sizes = prntSettings.PaperSizes;

            foreach (PaperSize size in sizes)
            {
                int curWidth = size.Width;
                int curHeigth = size.Height;
                Trace.WriteLine("   check paper size " + size.PaperName + " in inches " + curWidth.ToString() + " x " + curHeigth.ToString());

                bool check1 = IntEquals(widthInches, curWidth, 5);
                bool check2 = IntEquals(heigthInches, curHeigth, 5);

                if (check1 && check2)
                {
                    Trace.WriteLine("    A sheet size is found, width equals width, height equals height");
                    return size;
                }
                else
                {
                    bool check3 = IntEquals(widthInches, curHeigth, 5);
                    bool check4 = IntEquals(heigthInches, curWidth, 5);
                    if (check3 && check4)
                    {
                        Trace.WriteLine("    Width equals height, height equals width - is compatible too");
                        return size;
                    }
                    else
                    {
                        Trace.WriteLine("    Sheet size is not compatible");
                    }

                }
            }

            Trace.WriteLine("    Sheet size is found");
            return null;
        }

        private static bool IntEquals(int i1, int i2, int delta)
        {
            int c = Math.Abs(i1 - i2);
            if (c <= delta) return true;
            return false;
        }

        public static void AddFormatToAnyPdfPrinter(string formName, double widthCm, double heightCm)
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (printer == "PDFCreator")
                {
                    PrinterUtility.AddFormat(printer, formName, widthCm, heightCm);
                    return;
                }
                else if(printer.Contains("PDF24"))
                {
                    PrinterUtility.AddFormat(printer, formName, widthCm, heightCm);
                    return;
                }
                else if (printer == "Adobe PDF")
                {
                    PrinterUtility.AddFormat(printer, formName, widthCm, heightCm);
                    return;
                }
            }
            PrinterUtility.AddFormat(formName, widthCm, heightCm);
        }

        public static void AddFormat(string formName, double widthCm, double heightCm)
        {
            AddFormat(GetDefaultPrinter(), formName, widthCm, heightCm);
        }


        public static void AddFormat(string printerName, string formName, double widthCm, double heightCm)
        {
            //форматы надо всегда добавлять в вертикальной ориентации
            double heightCmForAdd = heightCm > widthCm ? heightCm : widthCm;
            double widthCmForAdd = heightCm > widthCm ? widthCm : heightCm;

            WinApi.PrinterDefaults defaults = new WinApi.PrinterDefaults();
            defaults.pDatatype = null;
            defaults.pDevMode = IntPtr.Zero;
            // установить права доступа к принтеру
            defaults.DesiredAccess = WinApi.PRINTER_ACCESS_ADMINISTER | WinApi.PRINTER_ACCESS_USE;

            IntPtr hPrinter = IntPtr.Zero;

            // Открыть принтер и получить handle на принтер
            if (WinApi.OpenPrinter(printerName, out hPrinter, ref defaults))
            {
                try
                {
                    // если формат с таким именем существует, то удалить этот формат 
                    WinApi.DeleteForm(hPrinter, formName);
                    // создать и инициализировать структуру FORM_INFO_1 
                    WinApi.FormInfo1 formInfo = new WinApi.FormInfo1();
                    formInfo.Flags = 0;
                    formInfo.pName = formName;

                    // для перевода в сантиметры умножить на 10000
                    formInfo.Size.width = (int)(widthCmForAdd * 10000.0);
                    formInfo.Size.height = (int)(heightCmForAdd * 10000.0);
                    formInfo.ImageableArea.left = 0;
                    formInfo.ImageableArea.right = formInfo.Size.width;
                    formInfo.ImageableArea.top = 0;
                    formInfo.ImageableArea.bottom = formInfo.Size.height;

                    //  форма добавляется в список доступных принтеру форм
                    // попытаться добавить форму, в случае неудачи вброс исключения
                    if (!WinApi.AddForm(hPrinter, 1, ref formInfo))
                    {
                        StringBuilder strBuilder = new StringBuilder();
                        strBuilder.AppendFormat("Failed to add the custom paper size {0} to the printer {1}, System error number: {2}",
                            formName, printerName, WinApi.GetLastError());
                        throw new ApplicationException(strBuilder.ToString());
                    }
                }
                finally
                {
                    // закрыть принтер
                    WinApi.ClosePrinter(hPrinter);
                }
            }
            else // если не удалось открыть принтер WinApi.OpenPrinter
            {    // то вброс исключения  
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendFormat("Failed to open the {0} printer, System error number: {1}",
                    printerName, WinApi.GetLastError());
                throw new ApplicationException(strBuilder.ToString());
            }
        }


        public static bool CheckFormatExists(string printerName, string formName)
        {
            PrinterSettings prntSettings = new PrinterSettings();
            prntSettings.PrinterName = printerName;

            foreach (PaperSize ps in prntSettings.PaperSizes)
            {
                if (ps.PaperName == formName)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetDefaultPrinter()
        {
            PrintDocument pd = new PrintDocument();
            string defaultPrinterName = pd.PrinterSettings.PrinterName;
            return defaultPrinterName;
        }
    }
}