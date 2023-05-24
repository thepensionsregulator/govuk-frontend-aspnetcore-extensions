using GovUk.Frontend.AspNetCore.Extensions.Validation;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class TextInputViewModel
    {
        public TextInput? Page { get; set; }

        public string? Field1 { get; set; }

        [Required(ErrorMessage = nameof(Field2))]
        [StringLength(20, MinimumLength = 5, ErrorMessage = nameof(Field2))]
        [RegularExpression(@"[0-9]+", ErrorMessage = nameof(Field2))]
        [Compare(nameof(Field3), ErrorMessage = nameof(Field2))]
        public string? Field2 { get; set; }

        public string? Field3 { get; set; }

        [EmailAddress(ErrorMessage = nameof(Field4))]
        [MinLength(5, ErrorMessage = nameof(Field4))]
        [MaxLength(100, ErrorMessage = nameof(Field4))]
        public string? Field4 { get; set; }

        [Phone(ErrorMessage = nameof(Field4))]
        public string? Field5 { get; set; }

        [Range(5, 50, ErrorMessage = nameof(Field6))]
        public int? Field6 { get; set; }
        public string? Field7 { get; set; }

        [UkPostcode(ErrorMessage = nameof(Field8))]
        public string? Field8 { get; set; }
    }
}
