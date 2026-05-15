using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Tools.Shortcuts
{
    public class ShortcutManager
    {
        public List<ShortcutItem> shortcuts = new List<ShortcutItem>();

        public string xmlPath;
        public string xmlFileBackup;

        public ShortcutManager(int RevitVersion)
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string revitFolder = $"Autodesk Revit {RevitVersion}";

            xmlPath = System.IO.Path.Combine(appdataPath, "Autodesk", "Revit", revitFolder, "KeyboardShortcuts.xml");

            if (File.Exists(xmlPath))
            {
                LoadData(xmlPath);
            }
        }

        public ShortcutManager(string xmlFilePath)
        {
            xmlPath = xmlFilePath;
            LoadData(xmlFilePath);
        }

        private void LoadData(string xmlPath)
        {
            string[] lines = File.ReadAllLines(xmlPath);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.Contains("CommandName")) continue;

                ShortcutItem shortcut = new ShortcutItem(line);

                shortcuts.Add(shortcut);
            }
        }


        public void AddShortcut(ShortcutItem shortcutToAdd)
        {
            foreach (ShortcutItem shortcutItem in shortcuts)
            {
                shortcutItem.RemoveShortCut(shortcutToAdd);
            }

            ShortcutItem sourceShortcut = shortcuts.FirstOrDefault(s => s.CommandId == shortcutToAdd.CommandId);
            if (sourceShortcut == null)
            {
                shortcutToAdd.IsModified = true;
                shortcuts.Add(shortcutToAdd);
            }
            else
            {
                sourceShortcut.AddShortCuts(shortcutToAdd.Shortcuts);
            }
        }


        

        public bool Save()
        {
            MakeBackup();

            List<string> lines = new List<string>();

            List<ShortcutItem> modified = shortcuts.Where(i => i.IsModified).ToList();

            lines.Add("<Shortcuts>");
            for (int i = 0; i < shortcuts.Count; i++)
            {
                ShortcutItem si = shortcuts[i];
                string line = si.ToXmlString();
                lines.Add(line);
            }
            lines.Add("</Shortcuts>");

            File.WriteAllLines(xmlPath, lines);

            return true;
        }

        public List<ShortcutItem> GetIncorrectShortcuts(List<ShortcutItem> requiredShortcuts)
        {
            List<ShortcutItem> incorrects = new List<ShortcutItem>();
            foreach (ShortcutItem req in requiredShortcuts)
            {
                ShortcutItem cur = this.shortcuts.FirstOrDefault(i => i.CommandId == req.CommandId);
                if (cur == null)
                {
                    incorrects.Add(req);
                    continue;
                }

                bool check = cur.CheckContainsRequiredShortcuts(req);
                if (!check)
                    incorrects.Add(req);
            }

            return incorrects;
        }

        public static void RestoreIncorrectShortcuts(int revitVersion, List<ShortcutItem> requiredShortcuts)
        {
            string shortcutsHelpUrl = "https://weandrevit.ru/gorjachie-klavishi-dlja-russkogo-jazyka";
            string bimStarterFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Autodesk\Revit\Addins\20xx\BimStarter");
            string weandrevitXmlPath = System.IO.Path.Combine(bimStarterFolder, "Template", "KeyboardShortcuts.xml");
            string shortcutsReadmeFile = System.IO.Path.Combine(bimStarterFolder, "Template", "KeyboardShortcuts ПАМЯТКА.docx");
            string restoreRebarXmlFolder = System.IO.Path.Combine(bimStarterFolder, "RevitAreaReinforcement_data", "KeyboardShortcuts", revitVersion.ToString());


            FormAddShortcutsSelect form1 = new FormAddShortcutsSelect();
            form1.ShowDialog();
            if (form1.DialogResult != DialogResult.Yes && form1.DialogResult != DialogResult.No)
                return;

            if (form1.DialogResult == DialogResult.Yes)
            {
                //подключение горячих клавиш Weadnrevit
                FormAddShortcutsDefault formHotkeysDefault = new FormAddShortcutsDefault(weandrevitXmlPath, shortcutsReadmeFile, shortcutsHelpUrl);
                formHotkeysDefault.ShowDialog();
            }
            else if (form1.DialogResult == DialogResult.No)
            {
                //подключение горячих клавиш только для ремонта арматуры
                FormAddShortcutsCustom formHotkeysCustom = new FormAddShortcutsCustom(restoreRebarXmlFolder, shortcutsHelpUrl);
                if (formHotkeysCustom.ShowDialog() == DialogResult.Yes)
                {
                    string userXmlFilePath = formHotkeysCustom.userXmlPath;
                    ShortcutManager userShortcutsManager = new ShortcutManager(userXmlFilePath);
                    foreach (ShortcutItem reqShortcut in requiredShortcuts)
                    {
                        userShortcutsManager.AddShortcut(reqShortcut);
                    }
                    if (!userShortcutsManager.Save())
                    {
                        MessageBox.Show("Failed to save the required shortcuts");
                        return;
                    }
                    FormAddShortcutsCustom2 formHotkeysCustom2 = new FormAddShortcutsCustom2(userXmlFilePath);
                    formHotkeysCustom2.ShowDialog();
                }
            }
        }

        public void MakeBackup()
        {
            string curDateTime = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            xmlFileBackup = xmlPath.Replace(".xml", " - " + curDateTime + ".xml");
            System.IO.File.Copy(xmlPath, xmlFileBackup);
        }
    }
}
