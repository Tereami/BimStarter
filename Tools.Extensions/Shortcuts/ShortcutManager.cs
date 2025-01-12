using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tools.Extensions.Shortcuts
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

            if (!File.Exists(xmlPath))
                throw new Exception($"File not found: {xmlPath}");

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
            if (sourceShortcut == null) throw new Exception($"Failed to find a command {shortcutToAdd.CommandId}");

            sourceShortcut.AddShortCuts(shortcutToAdd.Shortcuts);
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
                    throw new Exception($"Failed! No command found: {req.CommandId}");

                bool check = cur.CheckContainsRequiredShortcuts(req);
                if (!check)
                    incorrects.Add(req);
            }

            return incorrects;
        }

        public void MakeBackup()
        {
            string curDateTime = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            xmlFileBackup = xmlPath.Replace(".xml", " - " + curDateTime + ".xml");
            System.IO.File.Copy(xmlPath, xmlFileBackup);
        }
    }
}
