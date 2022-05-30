namespace Headway.RemediatR.Model
{
    public class CustomerProduct
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }
        public Product? Product { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Value { get; set; }
    }
}
