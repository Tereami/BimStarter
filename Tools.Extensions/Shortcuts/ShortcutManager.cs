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

            shortcuts = new List<ShortcutItem>();

            string[] lines = File.ReadAllLines(xmlPath);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.Contains("CommandName")) continue;

                ShortcutItem shortcut = new ShortcutItem(line);

                shortcuts.Add(shortcut);
            }
        }

        public bool SetShortcuts(List<ShortcutItem> shortcutsToAdd)
        {
            string[] lines = File.ReadAllLines(xmlPath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (shortcutsToAdd.Count == 0) break;
                string line = lines[i];
                if (!line.Contains("CommandName")) continue;

                ShortcutItem curShortcut = new ShortcutItem(line);

                ShortcutItem checkRequired = shortcutsToAdd.FirstOrDefault(s => s.CommandId == curShortcut.CommandId);
                if (checkRequired == null) continue;

                curShortcut.SetShortCuts(checkRequired.Shortcuts);
                shortcutsToAdd.Remove(checkRequired);

                string newLine = curShortcut.ToXmlString();

                lines[i] = newLine;
            }

            if (shortcutsToAdd.Count > 0)
            {
                string missedCommands = string.Join(", ", shortcutsToAdd.Select(s => s.CommandId));
                throw new Exception($"No commands found! {missedCommands}");
            }

            MakeBackup();

            File.WriteAllLines(xmlPath, lines);

            return true;
        }

        public List<ShortcutItem> GetIncorrectShortcuts(List<ShortcutItem> requiredShortcuts)
        {
            List<ShortcutItem> incorrects = new List<ShortcutItem>();
            foreach (ShortcutItem reqs in requiredShortcuts)
            {
                ShortcutItem cur = this.shortcuts.FirstOrDefault(i => i.CommandId == reqs.CommandId);
                if (cur == null)
                    throw new Exception($"Failed! No command found: {reqs.CommandId}");

                bool check = cur.CheckContainsShortcuts(reqs.Shortcuts);
                if (!check)
                    incorrects.Add(reqs);
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
