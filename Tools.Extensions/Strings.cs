using System.Collections.Generic;
using System.Diagnostics;

namespace Tools.Extensions
{
    public static class Strings
    {
        public static Dictionary<string, string> GetMarksRange(Dictionary<string, List<int>> keysAndMarks)
        {
            Dictionary<string, string> keysAndRange = new Dictionary<string, string>();

            foreach (var kvp in keysAndMarks)
            {
                string key = kvp.Key;
                List<int> marks = kvp.Value;
                string range = GetMarksRange(marks);

                keysAndRange.Add(key, range);
            }
            Trace.WriteLine("Mark ranges are created by keys: " + keysAndRange.Count.ToString());
            return keysAndRange;
        }

        public static string GetMarksRange(List<int> marks)
        {
            if (marks.Count == 1) return marks[0].ToString();

            string range = marks[0].ToString();
            if (marks[1] == marks[0] + 1) range += "-";
            if (marks[1] != marks[0] + 1) range += ", ";

            for (int i = 1; i < marks.Count - 1; i++)
            {
                int curMark = marks[i];
                if (marks[i + 1] != curMark + 1)
                {
                    range += curMark + ", ";
                    continue;
                }
                else if (marks[i - 1] != curMark - 1)
                {
                    range += curMark + "-";
                    continue;
                }
            }
            range += marks[marks.Count - 1];
            Trace.WriteLine("Marks range: " + range);
            return range;
        }
    }
}
