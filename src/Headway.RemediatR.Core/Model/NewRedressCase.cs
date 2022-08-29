using Headway.Core.Attributes;

namespace Headway.RemediatR.Core.Model
{
    [DynamicModel]
    public class NewRedressCase
    {
        public int? RedressId { get; set; }
        public string? ProgramName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public string? RateType { get; set; }
        public string? RepaymentType { get; set; }
        public string? Status { get; set; }
    }
}