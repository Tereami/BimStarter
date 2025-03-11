namespace LinkWriter.Values
{
    public class NameAndValue
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public NameAndValue(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
