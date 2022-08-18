using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class TextInputViewModel
    {
        public TextInput Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        [StringLength(20, MinimumLength = 5, ErrorMessage = nameof(Field1))]
        [RegularExpression(@"[0-9]+", ErrorMessage = nameof(Field1))]
        [Compare(nameof(Field2), ErrorMessage = nameof(Field1))]
        public string Field1 { get; set; }

        public string Field2 { get; set; }

        [EmailAddress(ErrorMessage = nameof(Field3))]
        [MinLength(5, ErrorMessage = nameof(Field3))]
        [MaxLength(100, ErrorMessage = nameof(Field3))]
        public string Field3 { get; set; }

        [Phone(ErrorMessage = nameof(Field4))]
        public string Field4 { get; set; }

        [Range(5, 50, ErrorMessage = nameof(Field5))]
        public int Field5 { get; set; }

        public string Field6 { get; set; }
    }
}
