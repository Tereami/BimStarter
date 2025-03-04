using Autodesk.Revit.ApplicationServices;
using System;
using System.Diagnostics;
using System.IO;

namespace RibbonBimStarter
{
    public enum TemplateCheckingResult { Exists, OlderVersionExists, No }
    public class TemplateInstaller
    {
        public int RevitVersion { get; set; }
        public string ConfigFilePath { get; set; }

        public string TemplateFolder { get; set; }

        public string DesiredTemplateFileName;

        public string TemplateFilePath { get; set; }


        public static string TemplateOlderVersionPath;
        public static string TemplateFileToSave;


        public TemplateInstaller(int revitVersion, string rbsfolder)
        {
            RevitVersion = revitVersion;
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configFolder = Path.Combine(appdata, "Autodesk", "Revit", $"Autodesk Revit {RevitVersion}");
            ConfigFilePath = Path.Combine(configFolder, "Revit.ini");
            Debug.WriteLine($"Revit.ini file: {ConfigFilePath}");

            if (!File.Exists(ConfigFilePath))
                throw new Exception($"NO FILE {ConfigFilePath}");

            TemplateFolder = Path.Combine(rbsfolder, "Template");
            if (!Directory.Exists(TemplateFolder))
                throw new Exception($"NO FOLDER: {TemplateFolder}");

            DesiredTemplateFileName = $"Weandrevit_{RevitVersion}.rte";
        }

        public bool IsConfigFileOk()
        {
            Debug.WriteLine($"Read file: ConfigFilePath");
            string[] content = File.ReadAllLines(ConfigFilePath);
            for (int i = 0; i < content.Length; i++)
            {
                string line = content[i];
                if (!line.StartsWith("DefaultTemplate")) continue;
                if (line.Contains("eandrevit"))
                {
                    Debug.WriteLine($"Revit.ini file contains Weandrevit");
                    return true;
                }
            }
            Debug.WriteLine($"Revit.ini file does not contain Weandrevit!");
            return false;
        }

        public void AddTemplatePathToConfig()
        {
            string templateTitle = "Weandrevit Template";

            string textToAdd = $"DefaultTemplate={templateTitle}={TemplateFilePath}, ";

            string content = File.ReadAllText(ConfigFilePath, System.Text.Encoding.Unicode);
            content = content.Replace("DefaultTemplate=", textToAdd);

            File.WriteAllText(ConfigFilePath, content, System.Text.Encoding.Unicode);
            Debug.WriteLine($"Text '{textToAdd}' added in file {ConfigFilePath}");
        }

        public TemplateCheckingResult IsTemplateOk()
        {
            Debug.WriteLine($"Try to find template {RevitVersion} in folder {TemplateFolder}");

            string[] files = Directory.GetFiles(TemplateFolder, "*.rte");
            if (files.Length == 0)
                throw new Exception($"NO TEMPLATES IN FOLDER {TemplateFolder}");

            foreach (string filepath in files)
            {
                Debug.WriteLine($"Check file {filepath}");
                string filename = Path.GetFileName(filepath);
                if (filename == DesiredTemplateFileName)
                {
                    TemplateFilePath = filepath;
                    Debug.WriteLine($"Template file found!");
                    return TemplateCheckingResult.Exists;
                }
            }
            Debug.WriteLine($"No template file for version {RevitVersion}");

            TemplateFilePath = Path.Combine(TemplateFolder, DesiredTemplateFileName);
            TemplateFileToSave = TemplateFilePath;

            for (int i = RevitVersion; i >= 2020; i--)
            {
                Debug.WriteLine($"Try to find older template version {i}");
                string olderFileName = $"Weandrevit_{i}.rte";
                foreach (string filepath in files)
                {
                    if (Path.GetFileName(filepath) == olderFileName)
                    {
                        TemplateOlderVersionPath = filepath;
                        Debug.WriteLine($"Template file older version found!");
                        return TemplateCheckingResult.OlderVersionExists;
                    }
                }
            }
            return TemplateCheckingResult.No;
        }



        public void AddSharedParamsFilePath(ControlledApplication cApp)
        {
            string sharedParamsFilePath = Path.Combine(TemplateFolder, "Weandrevit 2020.txt");
            if (!File.Exists(sharedParamsFilePath))
                throw new Exception($"No file: {sharedParamsFilePath}");
            cApp.SharedParametersFilename = sharedParamsFilePath;
            Debug.WriteLine($"Shared params file is set: {sharedParamsFilePath}");
        }
    }
}
