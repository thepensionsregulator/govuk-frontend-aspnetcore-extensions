using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;

namespace GovUk.Frontend.ExampleApp.Models.Validators
{
    public class CustomValidatorAttributeAdapter : AttributeAdapterBase<CustomValidatorAttribute>
    {
        public CustomValidatorAttributeAdapter(CustomValidatorAttribute attribute, IStringLocalizer? stringLocalizer) : base(attribute, stringLocalizer) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-custom", errorMessage);
            MergeAttribute(context.Attributes, "data-val-custom-property1", Attribute.Property1);
            MergeAttribute(context.Attributes, "data-val-custom-property2", Attribute.Property2);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
