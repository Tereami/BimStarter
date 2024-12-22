using System.Collections.Generic;
using System.Linq;

namespace Tools.Extensions.Shortcuts
{
    public class ShortcutItem
    {
        public string CommandName { get; set; }
        public string CommandId { get; set; }
        public List<string> Shortcuts { get; set; }

        public string Paths { get; set; }

        public ShortcutItem(string xmlRow)
        {
            xmlRow = xmlRow.Replace("\" ", "®");
            xmlRow = xmlRow.Replace("<ShortcutItem ", "");
            xmlRow = xmlRow.Replace("/>", "");

            string[] arr1 = xmlRow.Split('®');

            char[] unwantedChars = { '\\', ' ', '"', '\t' };

            Dictionary<string, string> values = new Dictionary<string, string>();
            foreach (string elem in arr1)
            {
                if (!elem.Contains("=")) continue;

                string[] arr2 = elem.Split('=');

                string name = arr2[0].Trim(unwantedChars);
                string value = arr2[1].Trim(unwantedChars);

                values.Add(name, value);
            }

            CommandName = values[nameof(CommandName)];
            CommandId = values[nameof(CommandId)];

            if (values.ContainsKey("Shortcuts"))
                Shortcuts = values["Shortcuts"].Split('#').ToList();
            else
                Shortcuts = null;

            if (values.ContainsKey("Paths"))
                Paths = values["Paths"];
            else
                Paths = null;
        }

        public ShortcutItem(string commandName, string commandId, IEnumerable<string> shortcuts, string paths)
        {
            CommandName = commandName;
            CommandId = commandId;
            Shortcuts = shortcuts.ToList();
            Paths = paths;
        }

        public bool CheckContainsShortcuts(IEnumerable<string> requiredShortcuts)
        {
            if (this.Shortcuts == null || this.Shortcuts.Count == 0) return false;

            foreach (string shortcut in requiredShortcuts)
            {
                if (!this.Shortcuts.Contains(shortcut))
                    return false;
            }
            return true;
        }

        public void SetShortCuts(IEnumerable<string> addShortcuts)
        {
            if (Shortcuts == null)
            {
                Shortcuts = addShortcuts.ToList();
                return;
            }
            foreach (string shortcut in addShortcuts)
            {
                if (!this.Shortcuts.Contains(shortcut))
                {
                    this.Shortcuts.Add(shortcut);
                }
            }
        }

        public string ToXmlString()
        {
            string line = $"<ShortcutItem CommandName=\"{CommandName}\" CommandId=\"{CommandId}\"";

            if (Shortcuts != null && Shortcuts.Count > 0)
            {
                string shortcutsJoined = string.Join("#", Shortcuts);
                line += $" Shortcuts=\"{shortcutsJoined}\"";
            }
            if (Paths != null && Paths.Length > 0)
            {
                line += $" Paths=\"{Paths}\"";
            }
            line += @" />";

            return line;
        }
    }
}
