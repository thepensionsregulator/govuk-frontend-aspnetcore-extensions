using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.ExampleApp.Models
{
    public class CustomValidationViewModel
    {
        [Required(ErrorMessage ="Cannot be null")]
        [Range(10, 20, ErrorMessage = "Must be between 10 and 20")]
        public int Field1 { get; set; }

        [Required(ErrorMessage = "Cannot be null")]
        [Range(1, 10, ErrorMessage = "Must be between 1 and 10")]
        public int Field2 { get; set; }


        [CustomValidator("Field1", "Field2", ErrorMessage = "This field must be the sum of box 1 and box 2")]
        public string Field3 { get; set; }
    }

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

            if (value!=null && int.Parse(value.ToString()) == prop1+prop2)
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
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
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
