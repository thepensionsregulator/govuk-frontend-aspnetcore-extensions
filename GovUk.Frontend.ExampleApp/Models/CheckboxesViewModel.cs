using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class CheckboxesViewModel
    {
        [Required(ErrorMessage = "Please select at least one checkbox")]
        public List<int>? Field1 { get; set; }

        [RegularExpression("[0-9]+", ErrorMessage = "The related question must be numbers")]
        public string? Field2 { get; set; }

        [Required(ErrorMessage = "You must select the single checkbox")]
        public string? Field3 { get; set; }
    }
}