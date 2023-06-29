using GovUk.Frontend.ExampleApp.Models.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class CustomValidationViewModel
    {

        [Required(ErrorMessage = "This field is required")]
        [Range(10, 20, ErrorMessage = "Must be between 10 and 20")]
        public int Field1 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Range(1, 10, ErrorMessage = "Must be between 1 and 10")]
        public int Field2 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [CustomValidator("Field1", "Field2", ErrorMessage = "custom_sum_error")]
        public string? Field3 { get; set; }
    }
}
