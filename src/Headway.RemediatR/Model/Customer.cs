namespace Headway.RemediatR.Model
{
    public class Customer
    {
        public Customer()
        {
            Products = new List<CustomerProduct>();
        }

        public int CustomerId { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public string? Address5 { get; set; }
        public string? Country { get; set; }
        public string? PostCode { get; set; }
        public List<CustomerProduct> Products { get; set; }
    }
}
