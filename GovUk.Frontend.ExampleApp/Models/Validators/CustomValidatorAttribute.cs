using System.ComponentModel.DataAnnotations;
using System;

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

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            var prop1 = int.Parse(type.GetProperty(Property1).GetValue(instance)?.ToString());
            var prop2 = int.Parse(type.GetProperty(Property2).GetValue(instance)?.ToString());

            if (value != null && int.Parse(value.ToString()) == prop1 + prop2)
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
