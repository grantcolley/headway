using System;

namespace Headway.RemediatR.Core.Model
{
    public class RefundCalculation
    {
        public decimal? BasicRefundAmount { get; set; }
        public decimal? CompensatoryAmount { get; set; }
        public decimal? TotalRefundAmount { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? SubmittedBy { get; set; }
    }
}