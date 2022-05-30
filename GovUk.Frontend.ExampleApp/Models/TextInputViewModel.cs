using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class TextInputViewModel
    {
        [Required(ErrorMessage = "This text input is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "String length must be between 5 and 20")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Field must be only numbers")]
        [Compare(nameof(Field2), ErrorMessage = "Fields must be the same")]
        public string Field1 { get; set; }

        public string Field2 { get; set; }

        [EmailAddress(ErrorMessage = "Email address must include @")]
        [MinLength(5, ErrorMessage = "Must be 5 or more characters")]
        [MaxLength(100, ErrorMessage = "Must be 100 or fewer characters")]
        public string Field3 { get; set; }

        [Range(5, 50, ErrorMessage = "Must be a number between 5 and 50")]
        public int Field4 { get; set; }
    }
}
