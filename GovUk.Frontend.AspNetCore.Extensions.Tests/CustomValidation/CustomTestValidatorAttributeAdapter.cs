using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests.CustomValidation
{
    public class CustomTestValidatorAttributeAdapter : AttributeAdapterBase<CustomTestValidatorAttribute>
    {
        public CustomTestValidatorAttributeAdapter(CustomTestValidatorAttribute attribute, IStringLocalizer stringLocalizer) : base(attribute, stringLocalizer) { }

        public override void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = GetErrorMessage(context);
            MergeAttribute(context.Attributes, "data-val-custom", errorMessage);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return base.GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }

}
