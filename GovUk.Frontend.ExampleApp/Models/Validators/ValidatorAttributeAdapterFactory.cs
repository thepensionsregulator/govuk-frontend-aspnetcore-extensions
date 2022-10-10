namespace GovUk.Frontend.ExampleApp.Models.Validators
{
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;
    using System.ComponentModel.DataAnnotations;

    public abstract class ValidatorAttributeAdapterFactory<T> : IValidatorAttributeAdapterFactory where T : ValidationAttribute
    {
        public bool CanAdapt(ValidationAttribute attribute)
        {
            if (attribute is T)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer localizer);
    }
}
