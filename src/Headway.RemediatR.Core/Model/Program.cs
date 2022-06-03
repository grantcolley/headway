using System;

namespace Headway.RemediatR.Core.Model
{
    public class Program
    {
        public int ProgramId { get; set; }
        public string? Name { get; set; }
        public decimal? Compensation { get; set; }
        public decimal? CompensatoryInterest { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
