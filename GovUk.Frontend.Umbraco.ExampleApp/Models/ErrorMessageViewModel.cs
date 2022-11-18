using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class ErrorMessageViewModel
    {
        public ErrorMessage? Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        public int? Field1 { get; set; }

        [Required(ErrorMessage = nameof(Field2))]
        public int? Field2 { get; set; }

        public int? Field3 { get; set; }

        [Range(100, 100, ErrorMessage = nameof(AddsUpTo100))]
        public int? AddsUpTo100 { get => Field1 + Field2; }

        [Range(50, 50, ErrorMessage = nameof(AddsUpTo50))]
        public int? AddsUpTo50 { get => Field1 + Field2; }
    }
}
