using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class SelectViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string? Field1 { get; set; }
    }
}
