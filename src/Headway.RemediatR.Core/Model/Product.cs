using Headway.RemediatR.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.RemediatR.Core.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Value { get; set; }

        [Range(3, 360, ErrorMessage = "Duration must be between 3 and 360")]
        public int? Duration { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? Rate { get; set; }
    }
}
