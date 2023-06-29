using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class FileUploadViewModel
    {
        [Required(ErrorMessage = "File is required")]
        public IFormFile? File { get; set; }
    }
}
