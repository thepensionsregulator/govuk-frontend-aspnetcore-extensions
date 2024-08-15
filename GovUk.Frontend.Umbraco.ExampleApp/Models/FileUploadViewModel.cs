using FileSignatures.Formats;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
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

        [MaxFileSize(5_000_000, ErrorMessage = nameof(FileWithMaximumSize))]
        public IFormFile? FileWithMaximumSize { get; set; }

        public IFormFile? FileWithSpecificExtensions { get; set; }

        [AllowedFileTypes([typeof(Excel)], ErrorMessage = nameof(FileOfSpecificTypes))]
        public IFormFile? FileOfSpecificTypes { get; set; }

        [AllowedFileTypes([typeof(Pdf)], ErrorMessage = nameof(FileOfPdfType))]
        public IFormFile? FileOfPdfType { get; set; }
    }
}