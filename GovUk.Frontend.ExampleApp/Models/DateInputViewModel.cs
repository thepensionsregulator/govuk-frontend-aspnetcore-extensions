using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class DateInputViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public DateTime? Field1 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime? Field2 { get; set; }
    }
}