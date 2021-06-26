namespace Headway.Core.Model
{
    public class FieldConfig
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public bool IsIdField { get; set; }
        public bool IsTitleField { get; set; }
        public string PropertyName { get; set; }
        public string DynamicComponentTypeName { get; set; }
    }
}