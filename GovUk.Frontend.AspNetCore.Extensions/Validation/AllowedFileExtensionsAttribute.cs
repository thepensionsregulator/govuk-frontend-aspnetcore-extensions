using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly Type[] _validTypes;
        private readonly IEnumerable<IFileTypeValidator> _fileTypeValidators;

        public AllowedFileExtensionsAttribute(IEnumerable<IFileTypeValidator> fileTypeValidators, Type[] validTypes)
        {
            _validTypes = validTypes;
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
            //var extension = Path.GetExtension(file.FileName);
            //if (!_extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            //{
            //    return new ValidationResult(ErrorMessage);
            //}

            //return ValidationResult.Success!;
        }
    }
}