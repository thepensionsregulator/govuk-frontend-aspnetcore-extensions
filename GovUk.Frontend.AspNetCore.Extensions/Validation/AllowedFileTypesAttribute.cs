using FileSignatures;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class AllowedFileTypesAttribute : ValidationAttribute
    {
        private readonly IFileFormatInspector _fileInspector;
        private readonly IEnumerable<FileFormat> _expectedFormats;

        public AllowedFileTypesAttribute(Type[] validTypes)
        {
            _expectedFormats = new List<FileFormat>();
            var availableFormats = FileFormatLocator.GetFormats();
            var expectedFormats = new List<FileFormat>();
            foreach (var t in validTypes)
            {
                var formatFound = false;
                foreach (var f in availableFormats)
                {
                    if (t == f.GetType())
                    {
                        expectedFormats.Add(f);
                        formatFound = true;
                        break;
                    }
                }
                if (!formatFound)
                {
                    throw new ArgumentException($"{t} is not defined");
                }
            }

            if (expectedFormats.Any())
            {
                _expectedFormats = expectedFormats.Distinct();
            }
            _fileInspector = new FileFormatInspector(_expectedFormats);
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success!;
            }

            if (value is not IFormFile file)
            {
                throw new InvalidOperationException($"Target property for {nameof(AllowedFileTypesAttribute)} must be {nameof(IFormFile)}");
            }
            var ext = Path.GetExtension(file.FileName);

            var extensionsMatch = true;
            foreach (var e in _expectedFormats)
            {
                var extension = e.Extension.StartsWith(".") ? e.Extension : "." + e.Extension;
                extensionsMatch = extension.Equals(ext, StringComparison.InvariantCultureIgnoreCase);
                if (extensionsMatch)
                { break; }
            }

            if (!extensionsMatch)
            {
                return new ValidationResult(ErrorMessage);
            }

            var fileValidationResult = ValidateFileSignature(file);
            if (_expectedFormats.Any() && fileValidationResult is null)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!;
        }

        private FileFormat? ValidateFileSignature(IFormFile file)
        {
            FileFormat? fileFormatMatch = null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileFormatMatch = _fileInspector.DetermineFileFormat(memoryStream);
            }

            return fileFormatMatch;
        }
    }
}