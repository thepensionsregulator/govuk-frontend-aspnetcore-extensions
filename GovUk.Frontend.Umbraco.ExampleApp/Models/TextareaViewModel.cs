using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class TextareaViewModel
    {
        public Textarea? Page { get; set; }
        public string? Field1 { get; set; }

        [Required(ErrorMessage = nameof(Field2))]
        [MinLength(10, ErrorMessage = nameof(Field2))]
        [RegularExpression(@"[0-9]+", ErrorMessage = nameof(Field2))]
        public string? Field2 { get; set; }

        [MaxLength(100, ErrorMessage = nameof(Field3))]
        public string? Field3 { get; set; }
    }
}
