using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace GovUk.Frontend.ExampleApp.Models.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private string Property1 { get; set; }
        private string Property2 { get; set; }

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
        public void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            
            MergeAttribute(context.Attributes, "data-val", "true");            
            MergeAttribute(context.Attributes, "data-val-custom", errorMessage);
            MergeAttribute(context.Attributes, "data-val-custom-property1", Property1);
            MergeAttribute(context.Attributes, "data-val-custom-property2", Property2);

        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }

    }
}
