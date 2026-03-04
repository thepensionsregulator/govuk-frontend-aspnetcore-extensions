using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    public interface IValidatorAttributeAdapterFactory
    {
        public IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer? localizer);
        public bool CanAdapt(ValidationAttribute attribute);
    }
}
