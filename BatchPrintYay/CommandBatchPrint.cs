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
#region usings
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tools.LinksManager;
#endregion

namespace BatchPrintYay
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class CommandBatchPrint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("BatchPrint"));
            Trace.WriteLine("Print started");
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Trace.WriteLine($"Assembly version: {version}");

            App.assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            Document mainDoc = commandData.Application.ActiveUIDocument.Document;

            Tools.SettingsSaver.Saver<YayPrintSettings> settingsSaver = new Tools.SettingsSaver.Saver<YayPrintSettings>();
            YayPrintSettings printSettings = settingsSaver.Activate("BatchPrintYay");
            if (printSettings.excludeColors == null || printSettings.excludeColors.Count == 0)
            {
                printSettings.excludeColors = YayPrintSettings.GetStandardExcludeColors();
            }
            printSettings.dwgProfiles = DwgSupport.GetAllDwgExportSettingsNames(mainDoc);


            List<MyRevitDocument> allDocuments = new List<MyRevitDocument>();

            MyRevitMainDocument myMainDocument = new MyRevitMainDocument(mainDoc, true, commandData.Application.ActiveUIDocument.Selection);
            if (myMainDocument.SelectedSheetsCount == 0)
            {
                message = MyStrings.MessageNoSelectedSheets;
                Trace.WriteLine("Print has been stopped, no selected sheets");
                return Result.Failed;
            }
            allDocuments.Add(myMainDocument);

            List<MyRevitLinkDocument> linkDocuments = myMainDocument.GetLinkDocuments();
            allDocuments.AddRange(linkDocuments);



            FormPrint form = new FormPrint(allDocuments, printSettings);

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Trace.WriteLine("Cancelled");
                return Result.Cancelled;
            }
            Trace.WriteLine("Click OK, print start");
            printSettings = form.printSettings;

            string printerName = printSettings.printerName;
            List<MyRevitDocument> printableDocuments = form.PrintableDocuments;


            string outputFolderCommon = printSettings.outputFolder;


            //Дополнительные возможности работают только с PDFCreator
            if (printerName != "PDFCreator")
            {
                if (printSettings.colorsType == ColorType.MonochromeWithExcludes || printSettings.mergePdfs || printSettings.useOrientation)
                {
                    string errmsg = MyStrings.MessagePrinterNotPdfCreator;
                    TaskDialog.Show("Warning", errmsg);
                    printSettings.mergePdfs = false;
                    printSettings.excludeColors = new List<PdfColor>();
                    printSettings.useOrientation = false;
                    Trace.WriteLine("Settings is not compatible with " + printerName);
                }
            }
            else
            {
                if (!printSettings.useOrientation)
                {
                    SupportRegistry.SetOrientationForPdfCreator(OrientationType.Automatic);
                    Trace.WriteLine("Sheet orientation is Automatic");
                }
            }
            bool printToFile = form.printToFile;
            string outputFolder = "";
            if (printToFile)
            {
                outputFolder = PrintSupport.CreateFolderToPrint(mainDoc, printerName, outputFolderCommon);
                Trace.WriteLine("Folder for print is created: " + outputFolder);
            }

            //печатаю листы из каждого выбранного revit-файла
            List<MySheet> printedSheets = new List<MySheet>();
            foreach (MyRevitDocument myDoc in printableDocuments)
            {
                Trace.WriteLine("Print from file: " + myDoc.Name);

                //проверяю, текущий это документ или полученный через ссылку
                if (myDoc is MyRevitLinkDocument)
                {
                    MyRevitLinkDocument linkDocument = myDoc as MyRevitLinkDocument;
                    linkDocument.OpenLinkDocument(commandData, false);
                }

                Document openedDoc = myDoc.Doc;
                List<MySheet> mSheets = myDoc.Sheets;
                Trace.WriteLine("Sheets found in this file: " + mSheets.Count.ToString());

                PrintManager pManager = openedDoc.PrintManager;
                Trace.WriteLine("Current selected printer: " + pManager.PrinterName);
                Trace.WriteLine("Try to set printer: " + printerName);
                pManager.SelectNewPrintDriver(printerName);
                pManager = openedDoc.PrintManager;
                pManager.PrintRange = PrintRange.Current;
                pManager.Apply();
                Trace.WriteLine("Print manager setting applied successfully");


                //получаю имя формата и проверяю, настроены ли размеры бумаги в Сервере печати
                bool checkingResult = PrintSupport.PrintFormatsCheckIn(myDoc, printerName, out message, ref mSheets);
                if (!checkingResult)
                {
                    Trace.WriteLine("Formats checking failed: " + message);
                    return Result.Failed;
                }
                Trace.WriteLine("Format checking success, go to print");

                //если включен экспорт dwg - нахожу параметры экспорта по имени 
                DWGExportOptions dwgOptions = null;
                if (printSettings.exportToDwg)
                {
                    List<ExportDWGSettings> curDwgSettings = DwgSupport.GetAllDwgExportSettingsNames(openedDoc)
                        .Where(i => i.Name == printSettings.selectedDwgExportProfileName)
                        .ToList();
                    if (curDwgSettings.Count == 0)
                    {
                        TaskDialog.Show("Error", MyStrings.MessageInFile + openedDoc.Title
                            + MyStrings.MessageDwgNotFound + printSettings.selectedDwgExportProfileName);
                        dwgOptions = DwgSupport.GetAllDwgExportSettingsNames(openedDoc).First().GetDWGExportOptions();
                    }
                    else
                    {
                        dwgOptions = curDwgSettings.First().GetDWGExportOptions();
                    }
                }

                SubcategoriesGetter nonPrintedSubcats = new SubcategoriesGetter(printSettings.nonPrintedLinesPrefix, myDoc.Doc);
                nonPrintedSubcats.Hide();

                //печатаю каждый лист
                foreach (MySheet msheet in mSheets)
                {
                    Trace.WriteLine("\nSheet is printing: " + msheet.sheet.Name);
                    if (printSettings.refreshSchedules)
                    {
                        SchedulesRefresher.Start(openedDoc, msheet.sheet);
                        Trace.WriteLine("Schedules is refreshed succesfully");
                    }

                    msheet.CheckIsColored(printSettings.alwaysColorParamName);

                    using (Transaction t = new Transaction(openedDoc))
                    {
                        t.Start(MyStrings.TransactionPrintProfiles);

                        string fileName0 = "";
                        if (printSettings.mergePdfs)
                        {
                            string guid = Guid.NewGuid().ToString();
                            fileName0 = msheet.sheet.SheetNumber + "_" + guid + ".pdf";
                        }
                        else
                        {
                            fileName0 = msheet.NameByConstructor(printSettings.nameConstructor);
                        }
                        string fileName = SheetSupport.ClearIllegalCharacters(fileName0);
                        if (fileName.Length > 128)
                        {
                            Trace.WriteLine("Sheet name length is longer than 128 symbols, will be cut");
                            string cutname = fileName.Substring(0, 63);
                            cutname += fileName.Substring(fileName.Length - 64);
                            fileName = cutname;
                        }

                        if (printerName == "PDFCreator" && printSettings.useOrientation)
                        {
                            if (msheet.IsVertical)
                            {
                                SupportRegistry.SetOrientationForPdfCreator(OrientationType.Portrait);
                                Trace.WriteLine("Portrait orientation is set forcibly");
                            }
                            if (!msheet.IsVertical)
                            {
                                SupportRegistry.SetOrientationForPdfCreator(OrientationType.Landscape);
                                Trace.WriteLine("Landscape orientation is set forcibly");
                            }
                        }

                        for (int i = 0; i < msheet.titleBlocks.Count; i++)
                        {
                            MySheet printedSheet = msheet;
                            string tempFilename = "";
                            if (msheet.titleBlocks.Count > 1)
                            {
                                Trace.WriteLine("More than 1 titleblock on 1 sheet, print number " + i.ToString());
                                tempFilename = fileName.Replace(".pdf", "_" + i.ToString() + ".pdf");
                                printedSheet = new MySheet(msheet);
                                printedSheet.SheetSubNumber = i;
                            }
                            else
                            {
                                Trace.WriteLine($"Single titleblock on sheet, Id {msheet.titleBlocks.First().GetElementId()}");
                                tempFilename = fileName;
                            }

                            string fullFilename = System.IO.Path.Combine(outputFolder, tempFilename);
                            Trace.WriteLine("File full name: " + fullFilename);

                            if (fullFilename.Length > 256)
                            {
                                throw new Exception(MyStrings.ExceptionFilenameTooLong + fullFilename);
                            }
                            printedSheet.PdfFileName = fullFilename;

                            //смещаю область для печати многолистовых спецификаций
                            double offsetX = -i * printedSheet.widthMm / 25.4; //смещение задается в дюймах!
                            Trace.WriteLine("Offset X: " + offsetX.ToString("F3"));

                            PrintSetting ps = PrintSupport.CreatePrintSetting(openedDoc, pManager, printedSheet, printSettings, offsetX, 0);

                            pManager.PrintSetup.CurrentPrintSetting = ps;
                            Trace.WriteLine("Print settings are applied, " + ps.Name);


                            PrintSupport.PrintView(printedSheet.sheet, pManager, ps, tempFilename);
                            Trace.WriteLine("Sheet is send to the printer");

                            printedSheets.Add(printedSheet);
                        }

                        if (printerName == "PDFCreator" && printSettings.useOrientation)
                        {
                            System.Threading.Thread.Sleep(5000);
                        }

                        t.RollBack();
                    }

                    //если включен dwg - то ещё экспортирую этот лист
                    if (printSettings.exportToDwg)
                    {
                        List<ElementId> sheetsIds = new List<ElementId> { msheet.sheet.Id };
                        string sheetname = msheet.NameByConstructor(printSettings.dwgNameConstructor);
                        openedDoc.Export(outputFolder, sheetname, sheetsIds, dwgOptions);
                    }
                }

                nonPrintedSubcats.Show();

                if (myDoc is MyRevitLinkDocument)
                {
                    MyRevitLinkDocument mylinkdoc = myDoc as MyRevitLinkDocument;
                    mylinkdoc.CloseDocument(false);
                }
            }
            int printedSheetsCount = printedSheets.Count;
            printedSheets = printedSheets
                .OrderBy(i => i.SheetNumberInt)
                .ThenBy(i => i.SheetSubNumber)
                .ToList();


            //если требуется постобработка файлов - ждем, пока они напечатаются
            if (printSettings.colorsType == ColorType.MonochromeWithExcludes || printSettings.mergePdfs)
            {
                Trace.WriteLine("Postprocessing is switched on; wait all sheets will be printed. Expected files count " + printedSheetsCount.ToString());
                int watchTimer = 0;
                while (printToFile)
                {
                    int filescount = System.IO.Directory.GetFiles(outputFolder, "*.pdf").Length;
                    Trace.WriteLine("Iteracion " + watchTimer + ", files are printed " + filescount);
                    if (filescount >= printedSheetsCount)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(500);
                    watchTimer++;


                    if (watchTimer > 100)
                    {
                        BalloonTip.Show("Warning", MyStrings.MessagePrintTooMuchTime);
                        Trace.WriteLine("Unable to wait for the end of printing");
                        return Result.Failed;
                    }
                }
            }


            List<string> pdfFileNames = printedSheets.Select(i => i.PdfFileName).ToList();
            Trace.WriteLine("PDF files should be printed");
            foreach (string pdfname in pdfFileNames)
            {
                Trace.WriteLine("  " + pdfname);
            }
            Trace.WriteLine("PDF files are printed in fact");
            foreach (string pdfnameOut in System.IO.Directory.GetFiles(outputFolder, "*.pdf"))
            {
                Trace.WriteLine("  " + pdfnameOut);
            }

            //преобразую файл в черно-белый при необходимости
            if (printSettings.colorsType == ColorType.MonochromeWithExcludes)
            {
                Trace.WriteLine("Convert pdf to black-white");
                foreach (MySheet msheet in printedSheets)
                {
                    if (msheet.ForceColored)
                    {
                        Trace.WriteLine("Sheet is marked as always colored: " + msheet.sheet.Name);
                        continue;
                    }

                    string file = msheet.PdfFileName;
                    string outFile = file.Replace(".pdf", "_OUT.pdf");
                    Trace.WriteLine("File will be converted from " + file + " to " + outFile);

                    pdf.PdfWorker.SetExcludeColors(printSettings.excludeColors);
                    pdf.PdfWorker.ConvertToGrayScale(file, outFile);

                    System.IO.File.Delete(file);
                    System.IO.File.Move(outFile, file);
                    Trace.WriteLine("Sheet os converted successfully");
                }
            }


            //объединяю файлы при необходимости
            if (printSettings.mergePdfs)
            {
                Trace.WriteLine(" ");
                Trace.WriteLine("\nMerge pdf files");
                System.Threading.Thread.Sleep(500);
                string combinedFile = System.IO.Path.Combine(outputFolder, mainDoc.Title + ".pdf");

                BatchPrintYay.pdf.PdfWorker.CombineMultiplyPDFs(pdfFileNames, combinedFile);

                foreach (string file in pdfFileNames)
                {
                    System.IO.File.Delete(file);
                    Trace.WriteLine("File has been deleted " + file);
                }
                Trace.WriteLine("Merge success");
            }

            if (printToFile)
            {
                if (System.IO.Directory.Exists(outputFolder))
                {
                    System.Diagnostics.Process.Start("explorer.exe", outputFolder);
                }
                else
                {
                    throw new Exception($"No folder {outputFolder}");
                }

                Trace.WriteLine("Folder is opened " + outputFolder);
            }

            settingsSaver.Save(printSettings);
            string msg = MyStrings.MessageSheetsPrinted + printedSheetsCount;
            BalloonTip.Show(MyStrings.MessageFinishPrintTItle, msg);
            Trace.WriteLine("Print has been finished successfully, sheets have been printed: " + printedSheetsCount);
            return Result.Succeeded;
        }
    }
}
