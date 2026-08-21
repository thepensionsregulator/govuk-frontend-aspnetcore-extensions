using FileSignatures.Formats;
using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class FileUploadViewModel
    {
        public FileUpload? Page { get; set; }

        [Required(ErrorMessage = nameof(ImageFile))]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = nameof(CsvFile))]
        public IFormFile? CsvFile { get; set; }

        [MaxFileSize(5_000_000, ErrorMessage = nameof(FileWithMaximumSize))]
        public IFormFile? FileWithMaximumSize { get; set; }

        [AllowedFileTypes([typeof(Excel)], ErrorMessage = nameof(ExcelFile))]
        public IFormFile? ExcelFile { get; set; }

        [AllowedFileTypes([typeof(Pdf)], ErrorMessage = nameof(PdfFile))]
        public IFormFile? PdfFile { get; set; }

        public IEnumerable<IFormFile> MultipleFiles { get; set; } = [];
    }
}