namespace GovUk.Frontend.ExampleApp.Models.Validators
{
    using Microsoft.AspNetCore.Mvc.DataAnnotations;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.Extensions.Localization;
    using System.ComponentModel.DataAnnotations;

    public interface IValidatorAttributeAdapterFactory
    {
        public IAttributeAdapter Create(ValidationAttribute attribute, IStringLocalizer localizer);
        public bool CanAdapt(ValidationAttribute attribute);
    }
}
