namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.Extensions.Localization;
    using System.ComponentModel.DataAnnotations;

    public interface IValidatorAttributeAdapterFactory
    {
        public IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer? localizer);
        public bool CanAdapt(ValidationAttribute attribute);
    }
}
