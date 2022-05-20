using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class TextareaViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MinLength(10, ErrorMessage = "Must be 10 or more characters")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Field must be only numbers")]
        public string Field1 { get; set; }
    }
}
