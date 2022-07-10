using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Headway.RemediatR.Core.Model
{
    public class Country
    {
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [StringLength(2, ErrorMessage = "Code must be 2 characters")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Name { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }
    }
}
