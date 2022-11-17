using GovUk.Frontend.Umbraco.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Services
{
    public static class GovUkFieldsetClassBuilder
    {
        /// <summary>
        /// Return classes for a fieldset-level error, if one exists
        /// </summary>
        /// <param name="fieldsetBlock">A block that potentially represents a fieldset</param>
        /// <param name="modelState">ModelState containing errors for the current request</param>
        /// <param name="legendIsPageHeadingMustMatchThis"><c>true</c> if classes are for when the <c>legendIsPageHeading</c> setting is <c>true</c>; <c>false</c> otherwise</param>
        /// <remarks>
        /// If there is at least one an error message bound to a property, and that property is invalid, render an extra
        /// grid row to surround all the fieldset content so that the error styling can span the whole fieldset
        /// unless legendIsPageHeading = false, in which case the classes are assigned in BlockList.cshtml,
        /// or unless this behaviour has been disabled using the renderErrorClasses setting.
        /// </remarks>
        /// <returns></returns>
        public static string BuildFieldsetErrorClass(OverridableBlockListItem fieldsetBlock, ModelStateDictionary modelState, bool legendIsPageHeadingMustMatchThis)
        {
            if (fieldsetBlock?.Content?.ContentType?.Alias != ElementTypeAliases.Fieldset) { return string.Empty; }

            bool.TryParse(fieldsetBlock.Settings.GetProperty(PropertyAliases.FieldsetRenderErrorClasses)?.GetValue()?.ToString(), out var renderErrorClasses);
            bool.TryParse(fieldsetBlock.Settings.GetProperty(PropertyAliases.FieldsetLegendIsPageHeading)?.GetValue()?.ToString(), out var legendIsPageHeading);
            var blocksWithinFieldset = fieldsetBlock.Content.Value<OverridableBlockListModel>(PropertyAliases.FieldsetBlocks);
            if (legendIsPageHeading == legendIsPageHeadingMustMatchThis && renderErrorClasses && blocksWithinFieldset != null)
            {
                var invalidFields = modelState.Where(x => x.Value?.ValidationState == ModelValidationState.Invalid).Select(x => x.Key);
                renderErrorClasses = blocksWithinFieldset.FindBlock(x => x.Content.ContentType.Alias == ElementTypeAliases.ErrorMessage
                                                                        && !string.IsNullOrEmpty(x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString())
                                                                        && invalidFields.Contains(x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString())) != null;
                if (renderErrorClasses)
                {
                    return "govuk-form-group govuk-form-group--error";
                }
            }
            return string.Empty;
        }
    }
}
