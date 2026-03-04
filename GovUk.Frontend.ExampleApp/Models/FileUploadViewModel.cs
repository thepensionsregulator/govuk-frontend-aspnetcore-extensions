using FileSignatures.Formats;
using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class FileUploadViewModel
    {
        [Required(ErrorMessage = "Select an image")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Select a CSV spreadsheet")]
        public IFormFile? CsvFile { get; set; }

        [MaxFileSize(5_000_000, ErrorMessage = "The file must be smaller than 5MB")]
        public IFormFile? FileWithMaximumSize { get; set; }

        [AllowedFileTypes([typeof(Excel)], ErrorMessage = "The selected file must be an Excel file")]
        public IFormFile? ExcelFile { get; set; }

        [AllowedFileTypes([typeof(Pdf)], ErrorMessage = "The selected file must be a PDF")]
        public IFormFile? PdfFile { get; set; }

        public IEnumerable<IFormFile> MultipleFiles { get; set; } = [];
    }
}
