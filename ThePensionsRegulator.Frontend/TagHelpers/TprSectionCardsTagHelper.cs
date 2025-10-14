using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprSectionCardTagHelper.TagName)]
    [OutputElementHint("nav")]

    public class TprSectionCardsTagHelper : TagHelper
    {
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal const string TagName = "tpr-section-cards";

        internal const string NewTabTextAttributeName = "new-tab-text";

        private string? _newTabText = null;

        [HtmlAttributeName(NewTabTextAttributeName)]
        public string? NewTabText
        {
            get => _newTabText;
            set => _newTabText = value;
        }

        private int _sectionCardsTitleHeadingLevel = 2;
        /// <summary>
        /// The heading level for each card title.
        /// </summary>
        /// <remarks>
        /// Must be between <c>2</c> and <c>5</c> (inclusive). The default is <c>2</c>.
        /// </remarks>
        [HtmlAttributeName("card-titles-heading-level")]
        public int HeadingLevel
        {
            get => _sectionCardsTitleHeadingLevel;
            set
            {
                if (value < ComponentGenerator.SectionCardTitlesMinHeadingLevel ||
                    value > ComponentGenerator.SectionCardTitlesMaxHeadingLevel)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingLevel)} must be between {ComponentGenerator.SectionCardTitlesMinHeadingLevel} and {ComponentGenerator.SectionCardTitlesMaxHeadingLevel}.");
                }

                _sectionCardsTitleHeadingLevel = value;
            }
        }

        /// <summary>
        /// The govuk heading class to apply to each title. Default is <c>govuk-heading-m</c>.
        /// </summary>
        /// <remarks>
        /// Must be one of <c>govuk-heading-xl</c>, <c>govuk-heading-l</c>, <c>govuk-heading-m</c> or <c>govuk-heading-s</c>.
        /// </remarks>
        private string _sectionCardsTitleHeadingClass = "govuk-heading-m";
        [HtmlAttributeName("card-titles-heading-class")]
        public string HeadingClass
        {
            get => _sectionCardsTitleHeadingClass;
            set
            {
                if (!ComponentGenerator.AllHeadingClasses.Contains(value))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        $"{nameof(HeadingClass)} must be one of govuk-heading-xl, govuk-heading-l, govuk-heading-m or govuk-heading-s");
                }

                _sectionCardsTitleHeadingClass = value;
            }
        }

        public TprSectionCardsTagHelper()
          : this(htmlGenerator: null)
        {
        }

        internal TprSectionCardsTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardsContext = new TprSectionCardsContext();

            using (context.SetScopedContextItem(cardsContext))
            {
                await output.GetChildContentAsync();
            }

            var sectionCards = new TprSectionCards
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Cards = cardsContext.Cards.Select(c => new TprSectionCard
                {
                    CardAttributes = c.CardAttributes,
                    TitleAttributes = c.TitleAttributes,
                    Title = c.Title,
                    TitleUrl = c.TitleUrl,
                    TitleTarget = c.TitleTarget,
                    TitleAllowHtml = c.TitleAllowHtml,
                    ContentAttributes = c.ContentAttributes,
                    Content = c.Content,
                    ContentAllowHtml = c.ContentAllowHtml,
                }).ToList(),
                NewTabText = NewTabText,
                sectionCardsTitleHeadingLevel = _sectionCardsTitleHeadingLevel,
                sectionCardsTitleHeadingClass = _sectionCardsTitleHeadingClass
            };

            var tagBuilder = _htmlGenerator.GenerateTprSectionCards(sectionCards);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
