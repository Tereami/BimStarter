using System.ComponentModel;

namespace PartsParametrisation
{
    public class PartSettings
    {
        public BindingList<PartParameter> Parameters { get; set; } = new BindingList<PartParameter>();

        public PartSettings()
        {

        }

        public void GetDefault()
        {
            PartParameter paramLength = new PartParameter
            {
                HostParameterName = "Рзм.Длина",
                PartParameterName = "Рзм.Длина"
            };

            PartParameter typeConstr = new PartParameter
            {
                HostParameterName = "Орг.ТипКонструкции",
                PartParameterName = "Орг.ТипКонструкцииЗависимый"
            };

            Parameters = new BindingList<PartParameter> { paramLength, typeConstr };
        }
    }
}
