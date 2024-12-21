using System.ComponentModel;

namespace PartsParametrisation
{
    public class PartParameter
    {
        [DisplayName("Host parameter")]
        public string HostParameterName { get; set; }

        [DisplayName("Part parameter")]
        public string PartParameterName { get; set; }
    }
}
