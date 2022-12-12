using GovUk.Frontend.Umbraco.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Services
{
    public static class GovUkFieldsetErrorFinder
    {
        /// <summary>
        /// Return fieldset-level errors that match a ModelState error, if the 'fieldsetErrors' setting is enabled for a fieldset block
        /// </summary>
        /// <param name="fieldsetBlock">A block that potentially represents a fieldset</param>
        /// <param name="modelState">ModelState containing errors for the current request</param>
        /// <returns></returns>
        public static IEnumerable<BlockListItem> FindFieldsetErrors(OverridableBlockListItem fieldsetBlock, ModelStateDictionary modelState)
        {
            if (fieldsetBlock?.Content?.ContentType?.Alias != ElementTypeAliases.Fieldset) { return Array.Empty<BlockListItem>(); }

            bool.TryParse(fieldsetBlock.Settings.GetProperty(PropertyAliases.FieldsetErrorsEnabled)?.GetValue()?.ToString(), out var fieldsetErrorsEnabled);
            var blocksWithinFieldset = fieldsetBlock.Content.Value<OverridableBlockListModel>(PropertyAliases.FieldsetBlocks);
            if (fieldsetErrorsEnabled && blocksWithinFieldset != null)
            {
                var invalidFields = modelState.Where(x => x.Value?.ValidationState == ModelValidationState.Invalid && !string.IsNullOrEmpty(x.Key)).Select(x => x.Key);
                return blocksWithinFieldset.Where(x => x.Content.ContentType.Alias == ElementTypeAliases.ErrorMessage
                                                       && !string.IsNullOrEmpty(x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString())
                                                       && invalidFields.Contains(x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString()));
            }
            return Array.Empty<BlockListItem>();
        }
    }
}
