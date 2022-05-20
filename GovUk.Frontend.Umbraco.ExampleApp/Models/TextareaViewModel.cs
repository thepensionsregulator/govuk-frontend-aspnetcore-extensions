using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class TextareaViewModel
    {
        public Textarea Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        [MinLength(10, ErrorMessage = nameof(Field1))]
        [RegularExpression(@"[0-9]+", ErrorMessage = nameof(Field1))]
        public string Field1 { get; set; }
    }
}
