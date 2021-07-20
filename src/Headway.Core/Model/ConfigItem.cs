namespace Headway.Core.Model
{
    public class ConfigItem
    {
        public int ConfigItemId { get; set; }
        public string PropertyName { get; set; }
        public string Label { get; set; }
        public bool? IsIdentity { get; set; }
        public bool? IsTitle { get; set; }
        public int Order { get; set; }
        public string Component { get; set; }

    }
}
