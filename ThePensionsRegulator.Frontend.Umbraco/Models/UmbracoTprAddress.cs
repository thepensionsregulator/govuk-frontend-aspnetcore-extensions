using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class UmbracoTprAddress : TprAddress
    {
        [Required(ErrorMessage = nameof(AddressLine1))]
        [MaxLength(100, ErrorMessage = nameof(AddressLine1))]
        public override string? AddressLine1 { get; set; }

        [MaxLength(100, ErrorMessage = nameof(AddressLine2))]
        public override string? AddressLine2 { get; set; }

        [MaxLength(100, ErrorMessage = nameof(AddressLine3))]
        public override string? AddressLine3 { get; set; }

        [Required(ErrorMessage = nameof(PostTown))]
        [MaxLength(100, ErrorMessage = nameof(PostTown))]
        public override string? PostTown { get; set; }

        [MaxLength(100, ErrorMessage = nameof(PostCounty))]
        public override string? PostCounty { get; set; }

        [Required(ErrorMessage = nameof(PostCode))]
        [MaxLength(10, ErrorMessage = nameof(PostCode))]
        public override string? PostCode { get; set; }

        [Required(ErrorMessage = nameof(CountryId))]
        public override int? CountryId { get; set; }
    }
}
