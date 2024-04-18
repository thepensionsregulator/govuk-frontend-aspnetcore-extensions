using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;


namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                throw new InvalidOperationException($"Target property for {nameof(AllowedFileExtensionsAttribute)} must be {nameof(IFormFile)}");
            }

            if (file is not null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success!;
        }

        public string GetErrorMessage() => $"Maximum allowed file size is {_maxFileSize} bytes.";
        
    }
}
