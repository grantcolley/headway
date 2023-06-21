using Headway.Core.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemediatR.Core.Model
{
    public class RefundCalculation : ModelBase
    {
        public int RefundCalculationId { get; set; }

        [MaxLength(50)]
        public string? CalculatedBy { get; set; }

        public DateTime? CalculatedDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BasicRefundAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CompensatoryAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CompensatoryInterestAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalCompensatoryAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalRefundAmount { get; set; }

        [MaxLength(50)]
        public string? VerifiedBy { get; set; }

        public DateTime? VerifiedDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? VerifiedBasicRefundAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? VerifiedCompensatoryAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? VerifiedCompensatoryInterestAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? VerifiedTotalCompensatoryAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? VerifiedTotalRefundAmount { get; set; }
    }
}