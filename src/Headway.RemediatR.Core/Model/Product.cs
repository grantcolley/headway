using System;

namespace Headway.RemediatR.Core.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public ProductType ProductType { get; set; }
        public RateType RateType { get; set; }
        public RepaymentType RepaymentType { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Value { get; set; }
        public int Duration { get; set; }
        public decimal Rate { get; set; }
    }
}
