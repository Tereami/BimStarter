using System.Text.RegularExpressions;

namespace Tools.Extensions
{
    public class Paths
    {
        public static string ClearIllegalCharacters(string line)
        {
            if (string.IsNullOrEmpty(line)) return string.Empty;
            string regexSearch = new string(System.IO.Path.GetInvalidFileNameChars())
                + new string(System.IO.Path.GetInvalidPathChars());
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            string line2 = r.Replace(line, "");
            return line2;
        }
    }
}
