namespace SchedulesTools
{
    public class CollapseScheduleSettings
    {
        public int HeaderRowsCount { get; set; } = 5;

        public string LastColumnSign { get; set; } = "=";

        public override string ToString()
        {
            string str = $"{nameof(HeaderRowsCount)}: {HeaderRowsCount}, {nameof(LastColumnSign)}: {LastColumnSign}";
            return str;
        }
    }
}
