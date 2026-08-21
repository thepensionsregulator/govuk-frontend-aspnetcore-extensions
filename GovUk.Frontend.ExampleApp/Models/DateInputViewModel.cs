using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class DateInputViewModel
    {
        [Required(ErrorMessage = "Field 1 is required")]
        [DateRange("2020-01-01", "2029-12-31", ErrorMessage = "Field 1 must be a date in the 2020s")]
        public DateOnly? Field1 { get; set; }

        public DateOnly? Field2 { get; set; }
        public DateOnly? Field3 { get; set; }

        public DateOnly? Field4 { get; set; }
    }
}