using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
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
            if (value is null)
            {
                return ValidationResult.Success!;
            }

            if (value is not IFormFile file)
            {
                throw new InvalidOperationException($"Target property for {nameof(MaxFileSizeAttribute)} must be {nameof(IFormFile)}");
            }

            if (file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!;
        }
    }
}