namespace Headway.RemediatR.Core.Model
{
    public class RedressProduct
    {
        public int RedressProductId { get; set; }
        public Redress Redress { get; set; }
        public Product Product { get; set; }
    }
}
