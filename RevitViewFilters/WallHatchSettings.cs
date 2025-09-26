namespace RevitViewFilters
{
    public class WallHatchSettings
    {
        public bool UseBaseLevel { get; set; } = false;
        public bool UseHeight { get; set; } = true;

        public bool UseType { get; set; } = false;

        public bool UseThickness { get; set; } = false;

        public string HatchPrefix { get; set; } = "WallHatch_";

        public string ImagePrefix { get; set; } = "ШтриховкаСтены_";
    }
}
