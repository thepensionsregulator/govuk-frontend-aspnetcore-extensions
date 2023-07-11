using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomValidatorAttribute : ValidationAttribute
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }

        public CustomValidatorAttribute(string property1, string property2)
        {
            Property1 = property1;
            Property2 = property2;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            var valueToValidate = value != null ? value.ToString() : null;
            var integerToValidate = !string.IsNullOrEmpty(valueToValidate) ? int.Parse(valueToValidate) : (int?)null;
            var value1 = type.GetProperty(Property1)?.GetValue(instance)?.ToString();
            var value2 = type.GetProperty(Property2)?.GetValue(instance)?.ToString();
            var prop1 = !string.IsNullOrEmpty(value1) ? int.Parse(value1) : (int?)null;
            var prop2 = !string.IsNullOrEmpty(value2) ? int.Parse(value2) : (int?)null;

            if (integerToValidate.HasValue && integerToValidate == prop1 + prop2)
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
