using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinkWriter
{
    public class Save
    {
        public List<string> SheetParameters = new List<string>();
        public List<string> TitleblockParameters = new List<string>();
        public List<string> TypeParameters = new List<string>();
        public List<string> ProjectParameters = new List<string>();

        public BindingList<CustomParameter> CustomParameters = new BindingList<CustomParameter>();

        public void SetSelectedParams(WriteLinkSettings sets)
        {
            SheetParameters = sets.SheetParams.Select(i => i.ParameterName).ToList();
            TitleblockParameters = sets.TitleblockParams.Select(i => i.ParameterName).ToList();
            TypeParameters = sets.TypeParams.Select(i => i.ParameterName).ToList();
            ProjectParameters = sets.ProjectParams.Select(i => i.ParameterName).ToList();
        }

        public void SetCustomParams(FormSelectParameterValues form)
        {
            CustomParameters = new BindingList<CustomParameter>();
            var firstLinkValues = form.ValuesCustomParameters.Values.FirstOrDefault();
            if (firstLinkValues == null || firstLinkValues.Count == 0) return;

            foreach (var kvp in firstLinkValues)
            {
                CustomParameter cp = new CustomParameter
                {
                    Name = kvp.Name,
                    Value = kvp.Value,
                    revitCategories = kvp.Categories,
                };
                CustomParameters.Add(cp);
            }
        }
    }
}
