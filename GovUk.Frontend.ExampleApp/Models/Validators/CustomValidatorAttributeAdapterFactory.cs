namespace GovUk.Frontend.ExampleApp.Models.Validators
{
    using GovUk.Frontend.AspNetCore.Extensions.Validation;
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;
    using System.ComponentModel.DataAnnotations;

    public class CustomValidatorAttributeAdapterFactory : ValidatorAttributeAdapterFactory<CustomValidatorAttribute>
    {
        public override IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer localizer)
        {
            return new CustomValidatorAttributeAdapter((CustomValidatorAttribute)attribute, localizer);
        }
    }
}