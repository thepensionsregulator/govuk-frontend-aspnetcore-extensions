using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    [HtmlTargetElement("govuk-client-side-validation")]
    [RestrictChildren("govuk-input", "govuk-radios", "govuk-select", "govuk-character-count", "govuk-checkboxes", "govuk-fieldset", "govuk-textarea", "govuk-date-input", "govuk-file-upload")]
    public class GovUkClientSideValidationTagHelper : TagHelper
    {
        private readonly IClientSideValidationHtmlEnhancer _htmlModifier;

        public GovUkClientSideValidationTagHelper(IClientSideValidationHtmlEnhancer htmlModifier)
        {
            _htmlModifier = htmlModifier ?? throw new ArgumentNullException(nameof(htmlModifier));
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
        /// Custom error message to show if the bound property has a <see cref="PhoneAttribute"/>
        /// </summary>
        [HtmlAttributeName("error-message-phone")]
        public string? ErrorMessagePhone { get; set; }

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

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Grab the HTML that would've been rendered by the child tag helper.
            var html = (await output.GetChildContentAsync()).GetContent();
            output.SuppressOutput();

            html = _htmlModifier.EnhanceHtml(
                html,
                ViewContext!,
                ErrorMessageRequired,
                ErrorMessageRegex,
                ErrorMessageEmail,
                ErrorMessagePhone,
                ErrorMessageLength,
                ErrorMessageMinLength,
                ErrorMessageMaxLength,
                ErrorMessageRange,
                ErrorMessageCompare
                );

            // Output the child HTML with any modifications made
            output.Content.AppendHtml(html);
        }
    }
}
