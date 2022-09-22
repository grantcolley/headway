using Headway.Core.Attributes;

namespace RemediatR.Core.Model
{
    [DynamicModel]
    public class RedressCase
    {
        public int? RedressId { get; set; }
        public string? CustomerName { get; set; }
        public string? ProgramName { get; set; }
        public string? ProductName { get; set; }
        public string? Status { get; set; }
    }
}