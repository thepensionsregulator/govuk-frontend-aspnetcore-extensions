using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class FileUploadViewModel
    {
        public FileUpload? Page { get; set; }

        [Required(ErrorMessage = nameof(File1))]
        public IFormFile? File1 { get; set; }

        [Required(ErrorMessage = nameof(File2))]
        public IFormFile? File2 { get; set; }
    }
}
