using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class CheckboxesViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public List<int> Field1 { get; set; } = new();

        [RegularExpression("[0-9]+", ErrorMessage = "The related question must be numbers")]
        public string Field2 { get; set; }
    }
}