using System.ComponentModel.DataAnnotations;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupViewModel
    {
        public AddressLookup? Page { get; set; }

        [Required(ErrorMessage = nameof(AddressLine1))]
        [MaxLength(500, ErrorMessage = nameof(AddressLine1))]
        public string? AddressLine1 { get; set; }

        [MaxLength(500, ErrorMessage = nameof(AddressLine2))]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = nameof(Town))]
        [MaxLength(500, ErrorMessage = nameof(Town))]
        public string? Town { get; set; }

        [MaxLength(500, ErrorMessage = nameof(Town))]
        public string? County { get; set; }

        [Required(ErrorMessage = nameof(Country))]
        public string? Country { get; set; }

        [MaxLength(20, ErrorMessage = nameof(Postcode))]
        public string? Postcode { get; set; }
    }
}
