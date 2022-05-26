using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class DateInputViewModel
    {
        [Required(ErrorMessage = "Field 1 is required")]
        public DateTime? Field1 { get; set; }

        [Required(ErrorMessage = "Field 2 is required")]
        public DateTime? Field2 { get; set; }
    }
}