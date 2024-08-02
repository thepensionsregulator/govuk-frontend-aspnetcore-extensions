using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators;
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
        private readonly IEnumerable<IFileTypeValidator> _fileTypeValidators;

        public AllowedFileTypesAttribute(IEnumerable<IFileTypeValidator> fileTypeValidators, Type[] validTypes)
        {
            _fileTypeValidators = fileTypeValidators.Where(x => validTypes.Any(y => y == x.GetType()));
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success!;
            }

            if (value is not IFormFile file)
            {
                throw new InvalidOperationException($"Target property for {nameof(AllowedFileExtensionsAttribute)} must be {nameof(IFormFile)}");
            }

            foreach (var v in _fileTypeValidators)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var isMatch = v.IsMatch(memoryStream, file.FileName);
                    if (isMatch)
                    {
                        return ValidationResult.Success!;
                    }
                }
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}