using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
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
                NewTabText = NewTabText
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
