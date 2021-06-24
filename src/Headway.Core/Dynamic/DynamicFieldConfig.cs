namespace Headway.Core.Dynamic
{
    public class DynamicFieldConfig
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public bool IsTitleField { get; set; }
        public string PropertyName { get; set; }
        public string DynamicComponentTypeName { get; set; }
    }
}