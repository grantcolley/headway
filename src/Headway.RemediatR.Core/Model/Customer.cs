using Headway.RemediatR.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Headway.RemediatR.Core.Model
{
    public class Customer
    {
        public Customer()
        {
            Products = new List<Product>();
        }

        public int CustomerId { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public List<Product>? Products { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(5, ErrorMessage = "Title cannot exceed 5 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(50, ErrorMessage = "Firstname cannot exceed 50 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string? Surname { get; set; }

        [StringLength(15, ErrorMessage = "Telephone cannot exceed 50 characters")]
        public string? Telephone { get; set; }

        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string? Email { get; set; }

        [StringLength(50, ErrorMessage = "Address Line 1 cannot exceed 50 characters")]
        public string? Address1 { get; set; }

        [StringLength(50, ErrorMessage = "Address Line 2 cannot exceed 50 characters")]
        public string? Address2 { get; set; }

        [StringLength(50, ErrorMessage = "Address Line 3 cannot exceed 50 characters")]
        public string? Address3 { get; set; }

        [StringLength(50, ErrorMessage = "Address Line 4 cannot exceed 50 characters")]
        public string? Address4 { get; set; }

        [StringLength(50, ErrorMessage = "Address Line 5 cannot exceed 50 characters")]
        public string? Address5 { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }

        [StringLength(10, ErrorMessage = "Post Code cannot exceed 10 characters")]
        public string? PostCode { get; set; }

        [MaxLength(8)]
        [RegularExpression(@"^(\d){2}-(\d){2}-(\d){2}$", ErrorMessage = "Sort Code must be 3 sets of 2 digits, separated by hyphens e.g. 12-34-56")]
        public string? SortCode { get; set; }

        [MaxLength(8)]
        [RegularExpression(@"^(\d){8}$", ErrorMessage = "AccountNumber must be 8 digits")]
        public string? AccountNumber { get; set; }
    }
}
