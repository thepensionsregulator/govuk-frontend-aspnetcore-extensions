using System.ComponentModel.DataAnnotations;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupViewModel
    {
        public AddressLookup? Page { get; set; }

        [Required(ErrorMessage = nameof(AddressLine1))]
        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = nameof(Town))]
        public string? Town { get; set; }

        [Required(ErrorMessage = nameof(County))]
        public string? County { get; set; }

        public string? Country { get; set; }

        [Required(ErrorMessage = nameof(Postcode))]
        [UkPostcode(ErrorMessage = nameof(Postcode))]
        public string? Postcode { get; set; }
    }
}
