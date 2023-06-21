﻿using Headway.Core.Attributes;
using Headway.Core.Model;
using RemediatR.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemediatR.Core.Model
{
    [DynamicModel]
    public class Product : ModelBase
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Range(3, 360)]
        public int? Duration { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal? Rate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Value { get; set; }
    }
}
