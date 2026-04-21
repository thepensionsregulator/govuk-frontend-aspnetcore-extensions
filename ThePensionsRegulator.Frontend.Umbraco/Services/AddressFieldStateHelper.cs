using System.Linq;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThePensionsRegulator.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    internal class AddressFieldStateHelper : IAddressFieldStateHelper
    {
        public AddressFieldState GetFieldState(
            string? modelPropertyName,
            string? umbracoLabel,
            string defaultLabel,
            ModelStateDictionary modelState)
        {
            var label = !string.IsNullOrEmpty(umbracoLabel) ? umbracoLabel : defaultLabel;

            ModelStateEntry? modelStateEntry = null;
            if (!string.IsNullOrEmpty(modelPropertyName))
            {
                modelState.TryGetValue(modelPropertyName, out modelStateEntry);
            }

            var isInvalid = modelStateEntry is not null
                && modelStateEntry.ValidationState == ModelValidationState.Invalid
                && modelStateEntry.Errors.Any();

            var hasErrorMessage = isInvalid
                && modelStateEntry!.Errors.Any(x => x.ErrorMessage != ValidationConstants.FIELDSET_ERROR);

            var errorMessage = hasErrorMessage ? string.Join(". ", modelStateEntry!.Errors.Where(x => x.ErrorMessage != ValidationConstants.FIELDSET_ERROR).Select(x => x.ErrorMessage)) : null;

            return new AddressFieldState(
                ModelPropertyName: modelPropertyName ?? string.Empty,
                Label: label,
                InvalidAriaLabel: isInvalid.ToString().ToLowerInvariant(),
                HasErrorMessage: hasErrorMessage,
                AttemptedValue: modelStateEntry?.AttemptedValue,
                ErrorMessage: errorMessage
            );
        }
    }
}
