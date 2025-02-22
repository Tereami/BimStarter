using System.Collections.Generic;

namespace LinkWriter
{
    public class Save
    {
        public HashSet<string> SheetParameters = new HashSet<string>();
        public HashSet<string> TitleblockParameters = new HashSet<string>();
        public HashSet<string> TypeParameters = new HashSet<string>();
        public HashSet<string> ProjectParameters = new HashSet<string>();


        public void AddValues(FormSelectParameterValues form)
        {
            SheetParameters = GetParamNames(form.ValuesSheets);
            TitleblockParameters = GetParamNames(form.ValuesTitleblocks);
            TypeParameters = GetParamNames(form.ValuesTitleblockType);
            ProjectParameters = GetParamNames(form.ValuesProjectInfo);
        }

        private HashSet<string> GetParamNames(Dictionary<string, List<(string, string)>> values)
        {
            HashSet<string> names = new HashSet<string>();
            foreach (var kvp in values)
            {
                foreach (var parameter in kvp.Value)
                {
                    string paramName = parameter.Item1;
                    names.Add(paramName);
                }
            }
            return names;
        }

        private HashSet<string> GetParamNames(List<(string, string)> values)
        {
            HashSet<string> names = new HashSet<string>();
            foreach (var parameter in values)
            {
                string paramName = parameter.Item1;
                names.Add(paramName);
            }
            return names;
        }
    }
}
