using FluentValidation;
using Headway.Core.Attributes;
using Headway.Core.Model;
using RemediatR.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RemediatR.Core.Model
{
    [DynamicModel]
    public class Customer : ModelBase
    {
        public Customer()
        {
            Products = new List<Product>();
        }

        public int CustomerId { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public List<Product> Products { get; set; }

        [Required]
        [StringLength(5)]
        public string? Title { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? Surname { get; set; }

        [StringLength(15)]
        public string? Telephone { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }

        [MaxLength(8)]
        [RegularExpression(@"^(\d){6}$")]
        public string? SortCode { get; set; }

        [MaxLength(8)]
        [RegularExpression(@"^(\d){8}$")]
        public string? AccountNumber { get; set; }

        [StringLength(50)]
        public string? Address1 { get; set; }

        [StringLength(50)]
        public string? Address2 { get; set; }

        [StringLength(50)]
        public string? Address3 { get; set; }

        [StringLength(50)]
        public string? Address4 { get; set; }

        [StringLength(50)]
        public string? Address5 { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        [StringLength(10)]
        public string? PostCode { get; set; }


        [NotMapped]
        [JsonIgnore]
        public string Fullname 
        { 
            get
            {
                var title = string.IsNullOrWhiteSpace(Title) ? string.Empty : Title + " ";
                var firstName = string.IsNullOrWhiteSpace(FirstName) ? string.Empty : FirstName + " ";
                var surname = string.IsNullOrWhiteSpace(Surname) ? string.Empty : Surname;
                return $"{title}{firstName}{surname}".Trim();
            }
        }
    }

    public class CustomerValidation : AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(1, 5).WithMessage("Title cannot exceed 5 characters");

            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("Firstname is required")
                .Length(1, 50).WithMessage("Firstname cannot exceed 50 characters");

            RuleFor(v => v.Surname)
                .NotEmpty().WithMessage("Surname is required")
                .Length(1, 50).WithMessage("Surname cannot exceed 50 characters");

            RuleFor(v => v.Telephone)
                .Length(1, 15).WithMessage("Telephone cannot exceed 50 characters");

            RuleFor(v => v.Email)
                .Length(1, 50).WithMessage("Email cannot exceed 50 characters");

            RuleFor(v => v.SortCode)
                .Length(6).WithMessage("Sort Code must be 6 digits e.g. 123456");

            RuleFor(v => v.AccountNumber)
                .Length(8).WithMessage("Account number must be 8 digits e.g. 12345678");

            RuleFor(v => v.Address1)
                .Length(1, 50).WithMessage("Address1 cannot exceed 50 characters");

            RuleFor(v => v.Address2)
                .Length(1, 50).WithMessage("Address2 cannot exceed 50 characters");

            RuleFor(v => v.Address3)
                .Length(1, 50).WithMessage("Address3 cannot exceed 50 characters");

            RuleFor(v => v.Address4)
                .Length(1, 50).WithMessage("Address4 cannot exceed 50 characters");

            RuleFor(v => v.Address5)
                .Length(1, 50).WithMessage("Address5 cannot exceed 50 characters");

            RuleFor(v => v.Country)
                .Length(1, 50).WithMessage("Country cannot exceed 50 characters");

            RuleFor(v => v.PostCode)
                .Length(1, 10).WithMessage("Post Code cannot exceed 10 characters");

            RuleForEach(v => v.Products).SetValidator(new ProductValidator());
        }
    }
}
