using System.Collections.Generic;
using System.Linq;

namespace LinkWriter
{
    public class Save
    {
        public List<string> SheetParameters = new List<string>();
        public List<string> TitleblockParameters = new List<string>();
        public List<string> TypeParameters = new List<string>();
        public List<string> ProjectParameters = new List<string>();


        public void SetSelectedParams(WriteLinkSettings sets)
        {
            SheetParameters = sets.SheetParams.Select(i => i.ParameterName).ToList();
            TitleblockParameters = sets.TitleblockParams.Select(i => i.ParameterName).ToList();
            TypeParameters = sets.TypeParams.Select(i => i.ParameterName).ToList();
            ProjectParameters = sets.ProjectParams.Select(i => i.ParameterName).ToList();
        }
    }
}
