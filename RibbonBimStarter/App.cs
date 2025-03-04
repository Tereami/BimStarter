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
#region Usings
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
#endregion

[assembly: System.Reflection.AssemblyVersion("1.0.*")]
namespace RibbonBimStarter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class App : IExternalApplication
    {
        public static string assemblyPath;
        public static string assemblyFolder;
        public static string ribbonPath;

        public static SettingsStorage settings;
        public static WebConnection connect;

        public static Guid paneGuid = new Guid("8d8207a6-c925-4e93-bb14-487912fb24b5");
        private FamilyLibraryDockablePane pane = null;

        public static string revitVersion = "2020";

        public static LanguageType curUiLanguage = LanguageType.English_USA;

        public Result OnStartup(UIControlledApplication application)
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Logger("Ribbon"));
            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Debug.WriteLine($"Revit started. Plugin version: {assemblyVersion}");

            assemblyPath = typeof(App).Assembly.Location;
            assemblyFolder = Path.GetDirectoryName(assemblyPath);
            string appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //ribbonPath = Path.Combine(assemblyFolder, "RibbonBimStarterData");
            ribbonPath = Path.Combine(appdataFolder, @"Autodesk\Revit\Addins\20xx\BimStarter");
            revitVersion = application.ControlledApplication.VersionNumber;
            curUiLanguage = application.ControlledApplication.Language;

            Debug.WriteLine("Ribbon path: " + ribbonPath);

            string tabName = "BIM-STARTER";
            try { application.CreateRibbonTab(tabName); }
            catch { Debug.WriteLine("Unable to create tab name " + tabName); }

            try
            {
                CreateAboutRibbon(application, tabName);
                CreateViewRibbon(application, tabName);
                CreateRebarRibbon(application, tabName);
                CreateTableRibbon(application, tabName);
                CreateModelingRibbon(application, tabName);
                CreateParametrisationRibbon(application, tabName);
                CreateInstrumentsRibbon(application, tabName);
                CreateMasterRibbon(application, tabName);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ribbon Sample", ex.Message);
                Debug.WriteLine("Exception: " + ex.Message);
                return Result.Failed;
            }
            RegisterDockablepage(application);

            Debug.WriteLine("Ribbon is created succesfully");


            settings = SettingsStorage.LoadSettings();
            if (settings == null)
            {
                TaskDialog.Show("Ошибка", "Не удалось запустить Bim-Starter! Не найден файл настроек");
                return Result.Failed;
            }

            //setting SSL certificate
            SslConfig.Load();
            Debug.WriteLine($"Current certificate: {ServicePointManager.SecurityProtocol}");

            connect = new WebConnection(App.settings.Email, App.settings.Password, App.settings.Website);


            //INSTALL TEMPLATE
            int revitVersionInt = Convert.ToInt32(revitVersion);
            if (revitVersionInt < 2020)
            {
                Debug.WriteLine($"No template for version {revitVersionInt}");
                return Result.Succeeded;
            }

            string[] args = Environment.GetCommandLineArgs();
            if (args.Contains("/viewer"))
            {
                Debug.WriteLine("Revit is in Viewer mode");
                return Result.Succeeded;
            }

            if (settings.ShowStartupWindow == false)
                return Result.Succeeded;

            TemplateInstaller ti = new TemplateInstaller(revitVersionInt, ribbonPath);
            bool configOk = ti.IsConfigFileOk();
            TemplateCheckingResult templateOk = ti.IsTemplateOk();
            if (configOk && templateOk == TemplateCheckingResult.Exists)
                return Result.Succeeded;

            FormInstallTemplate formTemplate = new FormInstallTemplate();
            System.Windows.Forms.DialogResult result = formTemplate.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.No)
            {
                settings.ShowStartupWindow = false;
                settings.Save(settings);
                Debug.WriteLine($"Template cancelled, startup window is disabled");
            }
            else if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (templateOk == TemplateCheckingResult.No)
                {
                    throw new Exception($"WEANDREVIT TEMPLATE NOT FOUND IN FOLDER {ti.TemplateFolder}");
                }

                if (!configOk)
                {
                    ti.AddTemplatePathToConfig();
                }
                ti.AddSharedParamsFilePath(application.ControlledApplication);

                if (templateOk == TemplateCheckingResult.OlderVersionExists)
                {
                    application.ControlledApplication.ApplicationInitialized += ControlledApplication_ApplicationInitialized;
                }
                else
                {
                    TaskDialog.Show("Weandrevit", "Конфигурация завершена. Перезапустите Revit");
                }
            }

            return Result.Succeeded;
        }

        private void ControlledApplication_ApplicationInitialized(object sender, Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
        {
            try
            {
                string templateToOpen = TemplateInstaller.TemplateOlderVersionPath;
                Debug.WriteLine($"Try to open document: {templateToOpen}");
                var app = sender as Autodesk.Revit.ApplicationServices.Application;
                UIApplication uiapp = new UIApplication(app);
                UIDocument uidoc = uiapp.OpenAndActivateDocument(templateToOpen);
                Document doc = uidoc.Document;

                //Autodesk.Revit.DB.Document doc = app.OpenDocumentFile(templateToOpen);

                string templateToSave = TemplateInstaller.TemplateFileToSave;
                Debug.WriteLine($"Try to save document to file: {templateToSave}");
                doc.SaveAs(templateToSave);
                Debug.WriteLine("Document saved");
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return;
            }
            TaskDialog.Show("Weandrevit", "Шаблон установлен. Revit будет перезапущен");
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Environment.Exit(-1);
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


        private void CreateAboutRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("AboutPanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "О программе" : "About";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);
            panel.AddItem(CreateButtonData("AboutBimstarter", "CommandAbout"));
            //panel.AddItem(CreateButtonData("AskBimQuestion", "CommandAskBimQuestion"));
            Debug.WriteLine("AboutPanel is created");
        }


        private void CreateRebarRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("RebarPanel started...");
            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "Армирование" : "Reinforcement";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);

            panel.AddItem(CreateButtonData("RebarTools", "CommandRebarVisibility"));

            panel.AddSeparator();

            panel.AddItem(CreateButtonData("RevitAreaReinforcement", "CommandCreateAreaRebar"));
            panel.AddItem(CreateButtonData("RevitAreaReinforcement", "CommandCreateFloorRebar"));

            panel.AddStackedItems(
                CreateButtonData("RevitAreaReinforcement", "CommandRestoreRebarArea"),
                CreateButtonData("RebarTools", "CommandRebarPresentation"),
                CreateButtonData("RebarTools", "CommandExplode")
                );

            panel.AddSlideOut();
            panel.AddItem(CreateButtonData("RebarTools", "CommandAreaMark"));
            Debug.WriteLine("RebarPanel is created");
        }



        private void CreateTableRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("TablePanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "Таблицы" : "Schedules";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);

            panel.AddItem(CreateButtonData("RebarSketch", "CommandCreatePictures"));

            panel.AddItem(CreateButtonData("SchedulesTools", "CommandCollapseRebarSchedule"));

            panel.AddStackedItems(
                CreateButtonData("BatchPrintYay", "CommandRefreshSchedules"),
                CreateButtonData("SchedulesTools", "CommandCreateTable"),
                CreateButtonData("Autonumber", "Command"));

            panel.AddSlideOut();
            panel.AddItem(CreateButtonData("RebarSketch", "CommandFormGenerator"));
            panel.AddItem(CreateButtonData("SchedulesTools", "CommandRevisionClouds"));
            Debug.WriteLine("TablePanel is created");
        }

        private void CreateViewRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("ViewPanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "Виды и листы" : "Views and sheets";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);

            PushButtonData pbdPrint = CreateButtonData("BatchPrintYay", "CommandBatchPrint");
            panel.AddItem(pbdPrint);

            PushButtonData pbdColorize = CreateButtonData("RevitViewFilters", "CommandViewColoring");
            panel.AddItem(pbdColorize);

            PushButtonData pbdOverrides = CreateButtonData("RevitGraphicsOverride", "Command");
            PushButtonData pbdOpenSheets = CreateButtonData("ViewsSheetsTools", "CommandOpenSheets");
            PushButtonData pbdViewNumbers = CreateButtonData("ViewsSheetsTools", "CommandSetNumber");
            panel.AddStackedItems(pbdOverrides, pbdOpenSheets, pbdViewNumbers);

            panel.AddSlideOut();
            panel.AddItem(CreateButtonData("RevitViewFilters", "CommandWallHatch"));
            panel.AddItem(CreateButtonData("RevitViewFilters", "CommandCreate"));
            panel.AddItem(CreateButtonData("RevitViewFilters", "CommandBatchDelete"));
            panel.AddItem(CreateButtonData("ViewsSheetsTools", "CommandViewTemplate"));

            Debug.WriteLine("ViewPanel is created");
        }

        private void CreateModelingRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("ModelingPanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "Моделирование" : "Modeling";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);


            SplitButton splitJoin = panel
                .AddItem(new SplitButtonData("JoingeometrySplitButton", "Geometry"))
                as SplitButton;

            splitJoin.AddPushButton(CreateButtonData("AutoJoinCut", "CommandAutoJoin"));
            splitJoin.AddPushButton(CreateButtonData("AutoJoinCut", "CommandJoinByOrder"));
            splitJoin.AddPushButton(CreateButtonData("AutoJoinCut", "CommandBatchUnjoin"));
            splitJoin.AddPushButton(CreateButtonData("AutoJoinCut", "CommandAutoCut"));
            splitJoin.AddPushButton(CreateButtonData("AutoJoinCut", "CommandCreateCope"));


            SplitButtonData sbdPiles = new SplitButtonData("Piles", "Piles");


            IList<RibbonItem> stacked1 = panel.AddStackedItems(
                CreateButtonData("GroupedAssembly", "Command"),
                sbdPiles,
                CreateButtonData("ParameterWriter", "CommandPropertiesCopy")
            );

            SplitButton splitPiles = stacked1[1] as SplitButton;
            splitPiles.AddPushButton(CreateButtonData("PilesCoords", "PilesNumberingCommand"));
            splitPiles.AddPushButton(CreateButtonData("PilesCoords", "PileCutCommand"));
            splitPiles.AddPushButton(CreateButtonData("PilesCoords", "PilesElevationCommand"));
            splitPiles.AddPushButton(CreateButtonData("PilesCoords", "PilesCalculateRangeCommand"));
            splitPiles.AddSeparator();
            splitPiles.AddPushButton(CreateButtonData("PilesCoords", "SettingsCommand"));
            Debug.WriteLine("ModelingPanel is created");
        }

        private void CreateParametrisationRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("ParametrisationPanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "Параметризация" : "Parametrisation";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);

            SplitButton splitHolesElev = panel
                .AddItem(new SplitButtonData("HolesElevSplitButton", "Openings"))
                as SplitButton;
            splitHolesElev.AddPushButton(CreateButtonData("RevitElementsElevation", "Command"));

            splitHolesElev.AddSeparator();
            splitHolesElev.AddPushButton(CreateButtonData("RevitElementsElevation", "CommandConfig"));

#if R2017 || R2018
            IList<RibbonItem> stacked = panel.AddStackedItems(
                CreateButtonData("ArchParametrisation", "CmdArchParametrisation"),
                CreateButtonData("ColumnsParametrisation", "Command")
            );
#else
            IList<RibbonItem> stacked = panel.AddStackedItems(
                CreateButtonData("ArchParametrisation", "CmdArchParametrisation"),
                CreateButtonData("RevitPlatesWeight", "Command"),
                CreateButtonData("ColumnsParametrisation", "Command")
            );
#endif

            Debug.WriteLine("ParametrisationPanel is created");
        }

        private void CreateInstrumentsRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("InstrumentsPanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "Инструменты" : "Instruments";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);

            panel.AddItem(CreateButtonData("SelectHost", "Command"));
            panel.AddItem(CreateButtonData("RevitWorksets", "Command"));

            /*string libraryButtonTitle = App.curUiLanguage == LanguageType.Russian ? "Семейства" : "Families";
            PushButtonData pbdFamiliesLibrary = new PushButtonData("ShowFamiliesCatalog", libraryButtonTitle, assemblyPath, "RibbonBimStarter.CommandShowPane");
            pbdFamiliesLibrary.ToolTip = App.curUiLanguage == LanguageType.Russian ? "Открыть палитру библиотеки семейств" : "Open family library palette";
            string famLibIconsPath = Path.Combine(Path.GetDirectoryName(assemblyPath), "FamilyLibrary_data");
            pbdFamiliesLibrary.LargeImage = new BitmapImage(new Uri(Path.Combine(famLibIconsPath, "FamilyLibrary_large.png")));
            pbdFamiliesLibrary.Image = new BitmapImage(new Uri(Path.Combine(famLibIconsPath, "FamilyLibrary_small.png")));*/

            SplitButton splitFamilies = panel
                .AddItem(new SplitButtonData("FamiliesSplitButton", "Families"))
                as SplitButton;
            splitFamilies.AddPushButton(CreateButtonData("RibbonBimStarter", "CommandShowPane"));
            splitFamilies.AddPushButton(CreateButtonData("RibbonBimStarter", "CommandCheckFamilies"));
            splitFamilies.AddPushButton(CreateButtonData("RibbonBimStarter", "CommandShowFamilyInfo"));


            Debug.WriteLine("InstrumentsPanel is created");
        }

        private void CreateMasterRibbon(UIControlledApplication uiApp, string tabName)
        {
            Debug.WriteLine("MasterPanel started...");

            string panelTitle = App.curUiLanguage == LanguageType.Russian ? "BIM-мастер" : "BIM-master";
            RibbonPanel panel = uiApp.CreateRibbonPanel(tabName, panelTitle);

            SplitButtonData sbdAddParams = new SplitButtonData("FamilyParametersSplitButton", "Add parameters");
            PushButtonData fixSlowFileData = CreateButtonData("FixSlowFile", "Command");
            SplitButtonData sbdParametrization = new SplitButtonData("ModelParametrizationSplitButton", "Parametrisation");
            IList<RibbonItem> stacked = panel.AddStackedItems(sbdAddParams, fixSlowFileData, sbdParametrization);

            SplitButton splitFamParam = stacked[0] as SplitButton;
            splitFamParam.AddPushButton(CreateButtonData("ClearUnusedGUIDs", "CommandAddParameters"));
            splitFamParam.AddPushButton(CreateButtonData("ClearUnusedGUIDs", "CommandAddParamsByAnalog"));

            SplitButton splitParametrization = stacked[2] as SplitButton;
            splitParametrization.AddPushButton(CreateButtonData("ParameterWriter", "Command"));
            splitParametrization.AddPushButton(CreateButtonData("ParameterWriter", "CommandWriteView"));
            splitParametrization.AddPushButton(CreateButtonData("LinkWriter", "CommandWriteLink"));
            splitParametrization.AddPushButton(CreateButtonData("RebarParametrisation", "Command"));
            splitParametrization.AddPushButton(CreateButtonData("PartsParametrisation", "Command"));

            splitParametrization.AddPushButton(CreateButtonData("IngradParametrisation", "Command"));


            panel.AddSlideOut();
            panel.AddItem(CreateButtonData("ClearUnusedGUIDs", "CommandClear"));
            panel.AddItem(CreateButtonData("RibbonBimStarter", "CommandUploadFamily"));


            /* string checkProjectButtonTitle = App.curUiLanguage == LanguageType.Russian ? "Проверить проект" : "Check project";
            PushButtonData pbdCheckFamilies = new PushButtonData("CheckFamilies", checkProjectButtonTitle, assemblyPath, "RibbonBimStarter.CommandCheckFamilies");
            pbdCheckFamilies.ToolTip = App.curUiLanguage == LanguageType.Russian 
                ? "Проверить проект на наличие устаревших, дублированных, сторонних и неверно названных семейств"
                : "Check project for old, incorrect, extraneous and duplicated families";
            
            pbdCheckFamilies.LargeImage = new BitmapImage(new Uri(Path.Combine(famLibIconsPath, "FamilyCheck_large.png")));
            pbdCheckFamilies.Image = new BitmapImage(new Uri(Path.Combine(famLibIconsPath, "FamilyCheck_small.png")));

            string uploadFamilyButtonTitle = App.curUiLanguage == LanguageType.Russian ? "Загрузить в базу" : "Load to the library";
            PushButtonData pbdUploadFamily = new PushButtonData("UploadFamily", uploadFamilyButtonTitle, assemblyPath, "RibbonBimStarter.CommandUploadFamily");
            pbdUploadFamily.ToolTip = App.curUiLanguage == LanguageType.Russian ? "Загрузить семейство в библиотеку" : "Load a family to the library";
            pbdUploadFamily.LargeImage = new BitmapImage(new Uri(Path.Combine(famLibIconsPath, "FamilyUpload_large.png")));
            pbdUploadFamily.Image = new BitmapImage(new Uri(Path.Combine(famLibIconsPath, "FamilyUpload_small.png")));
            */

            //PushButtonData pbdFamilySettings = new PushButtonData("FamilySettings", "Настройки", assemblyPath, "RibbonBimStarter.CommandFamilySettings");
            //pbdFamilySettings.ToolTip = "Настройки библиотеки семейств";

            //SplitButton splitFamilies = stacked2[1] as SplitButton;
            //splitFamilies.AddPushButton(pbdFamiliesLibrary);
            //splitFamilies.AddPushButton(pbdCheckFamilies);
            //splitFamilies.AddPushButton(pbdUploadFamily);
            //splitFamilies.AddPushButton(pbdFamilySettings);


            Debug.WriteLine("MasterPanel is created");
        }


        public PushButtonData CreateButtonData(string assemblyName, string className)
        {
            string dllPath = Path.Combine(ribbonPath, assemblyName + ".dll");
            if (!File.Exists(dllPath))
            {
                dllPath = Path.Combine(ribbonPath, assemblyName + "_" + revitVersion + ".dll");
                if (!File.Exists(dllPath))
                {
                    throw new Exception("File not found " + dllPath.Replace(@"\", @" \ "));
                }
            }
            string fullClassname = assemblyName + "." + className;
            string dataPath = Path.Combine(ribbonPath, assemblyName + "_data");
            string largeIcon = Path.Combine(dataPath, className + "_large.png");
            string smallIcon = Path.Combine(dataPath, className + "_small.png");

            string langTitle = Enum.GetName(typeof(LanguageType), App.curUiLanguage);
            string textPath = Path.Combine(dataPath, className + "." + langTitle + ".txt");
            if (!File.Exists(textPath))
                textPath = Path.Combine(dataPath, className + ".txt");


            string[] text = File.ReadAllLines(textPath);
            string title = text[0];
            if (title.Contains("/"))
                title = title.Replace("/", "\n");

            string tooltip = text[1];
            string url = text[2];

            PushButtonData data = new PushButtonData(fullClassname, title, dllPath, fullClassname);

            data.LargeImage = new BitmapImage(new Uri(largeIcon, UriKind.Absolute));
            data.Image = new BitmapImage(new Uri(smallIcon, UriKind.Absolute));

            data.ToolTip = text[1];

            ContextualHelp chelp = new ContextualHelp(ContextualHelpType.Url, url);
            data.SetContextualHelp(chelp);

            return data;
        }

        private void RegisterDockablepage(UIControlledApplication uiapp)
        {
            Debug.WriteLine("Start register dockable pane");
            EventLoadFamily eventLoad = new EventLoadFamily();
            ExternalEvent exEvent = ExternalEvent.Create(eventLoad);
            DockablePaneProviderData providerData = new DockablePaneProviderData();
            FamilyLibraryDockablePane famPane = new FamilyLibraryDockablePane(exEvent, eventLoad);
            this.pane = famPane;

            providerData.FrameworkElement = famPane;
            DockablePaneState state = new DockablePaneState();
            state.DockPosition = DockPosition.Tabbed;
            state.TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser;
            providerData.InitialState = state;

            DockablePaneId paneId = new DockablePaneId(paneGuid);

            string paneTitle = App.curUiLanguage == LanguageType.Russian ? "Библиотека семейств" : "Family library";

            uiapp.RegisterDockablePane(paneId, paneTitle, famPane);
            //uiapp.ViewActivated += new EventHandler<ViewActivatedEventArgs>(App_ViewActivated);
            Debug.WriteLine("Dockable pane is registered");
        }

        private void App_ViewActivated(object sender, ViewActivatedEventArgs args)
        {
            //some actions 
        }
    }
}
