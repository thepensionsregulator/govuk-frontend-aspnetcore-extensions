using GovUk.Frontend.AspNetCore.Extensions.Validation;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    [HtmlTargetElement("govuk-input-wrapper")]
    [RestrictChildren("govuk-input")]
    public class TextInputWrapperTagHelper : TagHelper
    {
        private static IStringLocalizerFactory? _factory;
        private static IStringLocalizer? _localizer = null;

        public TextInputWrapperTagHelper(IStringLocalizerFactory? factory = null)
        {
            _factory = factory;
        }

        /// <summary>
        /// Gets the <see cref="ViewContext"/> of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        [DisallowNull]
        public ViewContext? ViewContext { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="RequiredAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-required")]
        public string? ErrorMessageRequired { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="RegularExpressionAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-regex")]
        public string? ErrorMessageRegex { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="EmailAddressAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-email")]
        public string? ErrorMessageEmail { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="StringLengthAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-length")]
        public string? ErrorMessageLength { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="MinLengthAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-minlength")]
        public string? ErrorMessageMinLength { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="MaxLengthAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-maxlength")]
        public string? ErrorMessageMaxLength { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="RangeAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-range")]
        public string? ErrorMessageRange { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="CompareAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-compare")]
        public string? ErrorMessageCompare { get; set; }

        /// <summary>
        /// Custom error message to show if the bound property has a <see cref="CreditCardAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-credit-card")]
        public string? ErrorMessageCreditCard { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Grab the HTML that would've been rendered by the child tag helper.
            var html = new HtmlDocument();
            html.LoadHtml((await output.GetChildContentAsync()).GetContent());
            output.SuppressOutput();

            // Get the input element that should always be there if a <govuk-input> child exists.
            var input = html.DocumentNode.SelectSingleNode("//input");
            if (input != null)
            {
                // Get the output of the <govuk-input-error-message> grandchild tag helper, if present.
                var errorMessage = html.DocumentNode.SelectSingleNode("//*[@class='govuk-error-message']");

                // If <govuk-input-error-message> is present, the <govuk-input> tag helper always adds error classes because it assumes
                // we put it there because we already detected an error on the server. But we might want it there at all times to enable 
                // client-side validation, so remove those classes if there is not actually an error.
                bool hasError = ErrorMessageTagHelperHasRenderedAnError(errorMessage);
                if (!hasError)
                {
                    RemoveErrorClasses(html, input);
                }

                // Add the data-val-* attributes for ASP.NET / jQuery validation to pick up
                AddClientSideValidationAttributes(ViewContext!, input.Attributes, errorMessage?.Attributes,
                    ErrorMessageRequired,
                    ErrorMessageRegex,
                    ErrorMessageEmail,
                    ErrorMessageLength,
                    ErrorMessageMinLength,
                    ErrorMessageMaxLength,
                    ErrorMessageRange,
                    ErrorMessageCompare,
                    ErrorMessageCreditCard);
            }

            // Output the child HTML with any modifications made
            output.Content.AppendHtml(html.DocumentNode.OuterHtml);
        }

        private static void RemoveErrorClasses(HtmlDocument html, HtmlNode input)
        {
            input.RemoveClass("govuk-input--error");
            var errorContainer = html.DocumentNode.SelectSingleNode("//*[contains(@class,'govuk-form-group--error')]");
            if (errorContainer != null) { errorContainer.RemoveClass("govuk-form-group--error"); }
        }

        private static bool ErrorMessageTagHelperHasRenderedAnError(HtmlNode? errorMessage)
        {
            if (errorMessage == null) { return false; }
            foreach (var node in errorMessage.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element && !node.HasClass("govuk-visually-hidden") ||
                    node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerText))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddClientSideValidationAttributes(ViewContext viewContext,
            HtmlAttributeCollection targetElementAttributes,
            HtmlAttributeCollection? errorMessageAttributes,
            string? errorMessageRequired,
            string? errorMessageRegex,
            string? errorMessageEmail,
            string? errorMessageLength,
            string? errorMessageMinLength,
            string? errorMessageMaxLength,
            string? errorMessageRange,
            string? errorMessageCompare,
            string? errorMessageCreditCard)
        {
            if (viewContext == null || !viewContext.ClientValidationEnabled) { return; }

            var modelPropertyName = targetElementAttributes["name"]?.Value;
            if (string.IsNullOrEmpty(modelPropertyName)) { return; }

            targetElementAttributes.Add("data-val", "true");

            if (errorMessageAttributes != null)
            {
                var targetElementId = targetElementAttributes["id"]?.Value;
                errorMessageAttributes.Add("data-valmsg-for", targetElementId);
                errorMessageAttributes.Add("data-valmsg-replace", "true");
                errorMessageAttributes.Add("id", targetElementId + "-error");
            }

            var modelProperty = GetAttributesForModelProperty(viewContext, modelPropertyName);

            if (modelProperty != null)
            {
                // Compare
                var compareAttr = modelProperty.GetCustomAttributes<CompareAttribute>().FirstOrDefault();
                if (compareAttr != null)
                {
                    targetElementAttributes.Add("data-val-equalto", SelectBestErrorMessage(errorMessageCompare, compareAttr.ErrorMessage));
                    targetElementAttributes.Add("data-val-equalto-other", compareAttr.OtherProperty);
                }

                // Credit Card
                var creditCardAttr = modelProperty.GetCustomAttributes<CreditCardAttribute>().FirstOrDefault();
                if (creditCardAttr != null)
                {
                    targetElementAttributes.Add("data-val-creditcard", SelectBestErrorMessage(errorMessageCreditCard, creditCardAttr.ErrorMessage));
                }

                // Email Address
                var emailAttr = modelProperty.GetCustomAttributes<EmailAddressAttribute>().FirstOrDefault();
                if (emailAttr != null)
                {
                    targetElementAttributes.Add("data-val-email", SelectBestErrorMessage(errorMessageEmail, emailAttr.ErrorMessage));
                    targetElementAttributes["type"].Value = "email";
                }

                // Max Length
                var maxLengthAttr = modelProperty.GetCustomAttributes<MaxLengthAttribute>().FirstOrDefault();
                if (maxLengthAttr != null)
                {
                    targetElementAttributes.Add("data-val-maxlength", SelectBestErrorMessage(errorMessageMaxLength, maxLengthAttr.ErrorMessage));
                    targetElementAttributes.Add("data-val-maxlength-max", maxLengthAttr.Length.ToString());
                    targetElementAttributes.Add("maxlength", maxLengthAttr.Length.ToString());
                }

                // Min Length
                var minLengthAttr = modelProperty.GetCustomAttributes<MinLengthAttribute>().FirstOrDefault();
                if (minLengthAttr != null)
                {
                    targetElementAttributes.Add("data-val-minlength", SelectBestErrorMessage(errorMessageMinLength, minLengthAttr.ErrorMessage));
                    targetElementAttributes.Add("data-val-minlength-min", minLengthAttr.Length.ToString());
                }

                // Range
                var rangeAttr = modelProperty.GetCustomAttributes<RangeAttribute>().FirstOrDefault();
                if (rangeAttr != null)
                {
                    targetElementAttributes.Add("data-val-range", SelectBestErrorMessage(errorMessageRange, rangeAttr.ErrorMessage));
                    targetElementAttributes.Add("data-val-range-max", rangeAttr.Maximum.ToString());
                    targetElementAttributes.Add("data-val-range-min", rangeAttr.Minimum.ToString());
                    targetElementAttributes["type"].Value = "number";
                }

                // Regex
                var regexAttr = modelProperty.GetCustomAttributes<RegularExpressionAttribute>().FirstOrDefault();
                if (regexAttr != null)
                {
                    targetElementAttributes.Add("data-val-regex", SelectBestErrorMessage(errorMessageRegex, regexAttr.ErrorMessage));
                    targetElementAttributes.Add("data-val-regex-pattern", regexAttr.Pattern);
                    targetElementAttributes.Add("pattern", regexAttr.Pattern);
                }

                // Required
                var reqdAttr = modelProperty.GetCustomAttributes<RequiredAttribute>().FirstOrDefault();
                if (reqdAttr != null)
                {
                    targetElementAttributes.Add("required", "required");
                    targetElementAttributes.Add("data-val-required", SelectBestErrorMessage(errorMessageRequired, reqdAttr.ErrorMessage));
                }

                // String Length
                var strLenAttr = modelProperty.GetCustomAttributes(typeof(StringLengthAttribute), true).Cast<StringLengthAttribute>().FirstOrDefault();
                if (strLenAttr != null)
                {
                    targetElementAttributes.Add("data-val-length", SelectBestErrorMessage(errorMessageLength, strLenAttr.ErrorMessage));
                    targetElementAttributes.Add("data-val-length-max", strLenAttr.MaximumLength.ToString());
                    targetElementAttributes.Add("data-val-length-min", strLenAttr.MinimumLength.ToString());
                    targetElementAttributes.Add("maxlength", strLenAttr.MaximumLength.ToString());
                }
            }

        }

        private static string? SelectBestErrorMessage(string? fromTagHelperAttribute, string? fromDataAnnotationsAttribute)
        {
            return string.IsNullOrEmpty(fromTagHelperAttribute) ?
                        (_localizer != null && !string.IsNullOrEmpty(fromDataAnnotationsAttribute) ? _localizer.GetString(fromDataAnnotationsAttribute!) : fromDataAnnotationsAttribute) :
                        fromTagHelperAttribute;
        }

        private static PropertyInfo GetAttributesForModelProperty(ViewContext viewContext, string modelPropertyName)
        {
            var actionMethod = (viewContext.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
            var modelType = (actionMethod?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
            if (modelType == null) { modelType = viewContext.ViewData?.ModelMetadata?.ModelType; }
            if (modelType == null) { throw new InvalidOperationException($"Unable to detect the model type for the page to support client-side validation. To specify the model type you can decorate {(actionMethod != null ? actionMethod.DeclaringType?.FullName + "." + actionMethod.Name : "your controller action")} with a {nameof(ModelTypeAttribute)} identifying the type of your view model."); }

            var modelProperty = modelType?.GetProperty(modelPropertyName);
            if (modelProperty == null) { throw new InvalidOperationException($"To support client-side validation add a property named {modelPropertyName} to type {modelType!.FullName}, or decorate your controller action with {nameof(ModelTypeAttribute)} to specify a different model type."); }

            if (modelType != null && _factory != null)
            {
                _localizer = _factory.Create(modelType);
            }

            return modelProperty;
        }
    }
}
