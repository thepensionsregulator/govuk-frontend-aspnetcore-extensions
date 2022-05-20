using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class CheckboxesViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Field1 { get; set; }
    }
}