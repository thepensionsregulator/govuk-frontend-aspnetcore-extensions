using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco.Validation
{
    /// <summary>
    /// During ModelBinding .NET will validate all fields with validation attributes. If showing those fields is dependent
    /// upon a parent option is selected, and that option is not selected or not valid, validation for the dependent fields 
    /// should be marked as Skipped before the model is passed to the controller. 
    /// </summary>
    public class DependentFieldsActionFilter : IActionFilter
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public DependentFieldsActionFilter(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method != HttpMethod.Post.Method) { return; }

            if (_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                if (umbracoContext.PublishedRequest?.PublishedContent is null) { return; }

                var blockModels = umbracoContext.PublishedRequest.PublishedContent.FindOverridableBlockModels();
                ProcessFieldTypeThatSupportsDependentFields(context.ModelState,
                    blockModels,
                    ElementTypeAliases.Radios,
                    PropertyAliases.RadioButtons,
                    ElementTypeAliases.Radio,
                    PropertyAliases.RadioButtonValue,
                    PropertyAliases.RadioConditionalBlocks);

                ProcessFieldTypeThatSupportsDependentFields(context.ModelState,
                    blockModels,
                    ElementTypeAliases.Checkboxes,
                    PropertyAliases.Checkboxes,
                    ElementTypeAliases.Checkbox,
                    PropertyAliases.CheckboxValue,
                    PropertyAliases.CheckboxConditionalBlocks);
            }
        }

        private static void ProcessFieldTypeThatSupportsDependentFields(
            ModelStateDictionary modelState,
            IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels,
            string parentBlockTypeAlias,
            string optionsPropertyAlias,
            string optionBlockTypeAlias,
            string optionValuePropertyAlias,
            string conditionalBlocksPropertyAlias)
        {
            var parentBlocks = blockModels.FindBlocksByContentTypeAlias(parentBlockTypeAlias);
            if (parentBlocks is null || !parentBlocks.Any()) { return; }

            foreach (var parentBlock in parentBlocks)
            {
                // If the parent component (eg 'Radios') is not bound to a property, it's not ready for validation.
                var parentBoundProperty = parentBlock.Settings.Value<string>(PropertyAliases.ModelProperty);
                if (string.IsNullOrEmpty(parentBoundProperty) || !modelState.ContainsKey(parentBoundProperty)) { continue; }

                // Establish whether the parent component (eg 'Radios') is valid.
                // If not, any conditional fields dependent upon any of its options should not be validated.
                ModelValidationState? validationStateOfParent = modelState[parentBoundProperty]!.ValidationState;
                var selectedValueOfParent = modelState[parentBoundProperty]!.AttemptedValue;

                // Get the options, eg individual radio buttons within a 'Radios' component.
                var optionBlocks = parentBlock.Content.Value<OverridableBlockListModel>(optionsPropertyAlias)?.FindBlocksByContentTypeAlias(optionBlockTypeAlias);
                if (optionBlocks is null || !optionBlocks.Any()) { continue; }

                foreach (var optionBlock in optionBlocks)
                {
                    // Estalish whether the option is selected.
                    // If not, any conditional fields dependent upon that option should not be validated.
                    var optionValue = optionBlock.Content.Value<string>(optionValuePropertyAlias);
                    var optionIsSelected = optionValue == selectedValueOfParent;

                    if (validationStateOfParent == ModelValidationState.Invalid || !optionIsSelected)
                    {
                        // If there are any conditional fields dependent upon for this option, skip validation of those fields.
                        var conditionalBlocks = optionBlock.Content.Value<OverridableBlockListModel>(conditionalBlocksPropertyAlias);
                        if (conditionalBlocks is not null && conditionalBlocks.Any())
                        {
                            foreach (var conditionalBlock in conditionalBlocks)
                            {
                                var blockBoundProperty = conditionalBlock.Settings.Value<string>(PropertyAliases.ModelProperty);
                                if (!string.IsNullOrEmpty(blockBoundProperty) && modelState.ContainsKey(blockBoundProperty))
                                {
                                    modelState[blockBoundProperty]!.ValidationState = ModelValidationState.Skipped;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
