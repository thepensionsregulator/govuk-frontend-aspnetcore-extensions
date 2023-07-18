using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests.CustomValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomTestValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value != null && value.ToString() == "FISH")
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
