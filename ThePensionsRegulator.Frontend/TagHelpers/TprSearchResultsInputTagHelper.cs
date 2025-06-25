using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprSearchResultsTagHelper.TagName)]
    public class TprSearchResultsInputTagHelper : TagHelper
    {
        internal const string TagName = "tpr-search-results-input";
        private readonly ITprHtmlGenerator _htmlGenerator;

        private int _headingLevel = 2;
        /// <summary>
        /// The heading level.
        /// </summary>
        /// <remarks>
        /// Must be between <c>1</c> and <c>6</c> (inclusive). The default is <c>2</c>.
        /// </remarks>
        [HtmlAttributeName("heading-level")]
        public int HeadingLevel
        {
            get => _headingLevel;
            set
            {
                if (value < ComponentGenerator.SearchResultsMinHeadingLevel ||
                    value > ComponentGenerator.SearchResultsMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {ComponentGenerator.SearchResultsMinHeadingLevel} and {ComponentGenerator.SearchResultsMaxHeadingLevel}.");
                }

                _headingLevel = value;
            }
        }

        /// <summary>
        /// The govuk heading class to apply. Default is <c>govuk-heading-l</c>
        /// </summary>
        /// <remarks>
        /// Must be between <c>1</c> and <c>6</c> (inclusive). .
        /// </remarks>
        private string _headingClass = "govuk-heading-l";
        [HtmlAttributeName("govuk-heading-class")]
        public string HeadingClass
        {
            get => _headingClass;
            set
            {
                if (!ComponentGenerator.AllHeadingClasses.Contains(value))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingClass)} must be one of govuk-heading-xl, govuk-heading-l, govuk-heading-m or govuk-heading-s");
                }

                _headingClass = value;
            }
        }

        internal TprSearchResultsInputTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprSearchResultsInputTagHelper() : this(null) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inner = await output.GetChildContentAsync();
            var content = inner.GetContent();

            var result = _htmlGenerator.GenerateTprSearchResultsInput(_headingLevel, _headingClass, content);

            output.TagName = null;

            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
