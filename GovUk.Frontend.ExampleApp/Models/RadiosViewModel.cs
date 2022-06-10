using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class RadiosViewModel
    {
        [Required(ErrorMessage = "This radio button field is required")]
        public string Field1 { get; set; }

        [RegularExpression("[0-9]+", ErrorMessage = "The related question must be numbers")]
        public string Field2 { get; set; }
    }
}