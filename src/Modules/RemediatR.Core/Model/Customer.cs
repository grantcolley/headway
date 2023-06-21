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
}
