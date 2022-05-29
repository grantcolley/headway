namespace Headway.RemediatR.Model
{
    public class Redress
    {
        public Redress()
        {
            Products = new List<Product>();
        }

        public int RedressId { get; set; }
        public Customer? Customer { get; set; }
        public Program? Program { get; set; }
        public List<Product> Products { get; set; }
    }
}