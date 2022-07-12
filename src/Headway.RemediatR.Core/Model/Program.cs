using Headway.RemediatR.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.RemediatR.Core.Model
{
    public class Program
    {
        public int ProgramId { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Compensation { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? CompensatoryInterest { get; set; }
    }
}
