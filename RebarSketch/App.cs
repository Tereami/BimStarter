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
Zuev Aleksandr, 2024, all rights reserved.*/
#endregion
#region Usings
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.IO;
#endregion

[assembly: System.Reflection.AssemblyVersion("1.0.*")]

namespace RebarSketch
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public static string rebarSketchPath = "";
        public static string libraryPath = "";
        public static string configFilePath = "";

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = typeof(App).Assembly.Location;

            string tabName = "TEST";
            try { application.CreateRibbonTab(tabName); }
            catch { }

            string projectName = nameof(RebarSketch);
            RibbonPanel panel = application.CreateRibbonPanel(tabName, $"{projectName} panel");
            string commandName = nameof(CommandCreatePictures);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            commandName = nameof(CommandFormGenerator);
            _ = panel.AddItem(new PushButtonData(
                commandName,
                commandName,
                assemblyPath,
                $"{projectName}.{commandName}")
                ) as PushButton;

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public static void ActivatePaths()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Tools.Logger.Logger("RebarSketch"));

            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            string appdataFolder =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string bimstarterRootFolder =
                System.IO.Path.Combine(appdataFolder, "bim-starter");
            if (!System.IO.Directory.Exists(bimstarterRootFolder))
            {
                Trace.WriteLine("Create folder: " + bimstarterRootFolder);
                System.IO.Directory.CreateDirectory(bimstarterRootFolder);
            }
            configFilePath = Path.Combine(bimstarterRootFolder, "config.ini");

            string bimstarterStoragePath = string.Empty;
            if (File.Exists(configFilePath))
            {
                Trace.WriteLine("Read file: " + configFilePath);
                string[] lines = File.ReadAllLines(configFilePath);
                if (lines.Length > 0)
                {
                    bimstarterStoragePath = lines[0];
                    Trace.WriteLine($"Storage path: {bimstarterStoragePath}");
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(configFilePath);
                        Trace.WriteLine($"File is deleted: {configFilePath}");
                    }
                    catch
                    {
                        Trace.WriteLine($"Invalid file: {configFilePath}");
                        throw new Exception($"Invalid file: {configFilePath}");
                    }
                }
            }

            if (bimstarterStoragePath == string.Empty)
            {
                Trace.WriteLine("First start, show dialog window and select config folder");
                string configDefaultFolder = Path.Combine(appdataFolder, @"Autodesk\Revit\Addins\20xx\BimStarter");
                FormSelectPath form = new FormSelectPath(configFilePath, configDefaultFolder);
                if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                if (form.UseServerPath)
                {
                    bimstarterStoragePath = form.ServerPath;
                }
                else
                {
                    bimstarterStoragePath = configDefaultFolder;
                }
                Trace.WriteLine("Selected user path: " + bimstarterStoragePath);
                File.WriteAllText(configFilePath, bimstarterStoragePath);
                Trace.WriteLine("Success write to file: " + configFilePath);
            }

            rebarSketchPath = Path.Combine(bimstarterStoragePath, "RebarSketch");
            libraryPath = Path.Combine(rebarSketchPath, "library");
            Trace.WriteLine("Library path: " + libraryPath);
            if (!Directory.Exists(libraryPath))
            {
                Trace.WriteLine("Library isnt found");
                TaskDialog.Show("Rebar Sketch", "Library directory isnt found: " + libraryPath);
            }
        }
    }
}