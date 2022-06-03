using System;

namespace Headway.RemediatR.Core.Model
{
    public class RefundCalculation
    {
        public decimal? BasicRefundAmount { get; set; }
        public decimal? CompensatoryAmount { get; set; }
        public decimal? CompensatoryInterestAmount { get; set; }
        public decimal? TotalCompensatoryAmount { get; set; }
        public decimal? TotalRefundAmount { get; set; }
        public DateTime? CalculatedDate { get; set; }
        public string? CalculatedBy { get; set; }
    }
}