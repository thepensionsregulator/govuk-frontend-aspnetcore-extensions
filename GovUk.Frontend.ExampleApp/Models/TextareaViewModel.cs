using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class TextareaViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Field must be only numbers")]
        [MinLength(10, ErrorMessage = "Must be 10 or more characters")]        
        public string Field1 { get; set; }

        [MaxLength(100, ErrorMessage = "Must be 100 characters maximum")]
        public string Field2 { get; set; }
    }
}
