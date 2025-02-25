using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Org.BouncyCastle.Asn1.Cms;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprSectionCardTagHelper.TagName)]
    [OutputElementHint("ul")]

    public class TprSectionCardsTagHelper : TagHelper
    {
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal const string TagName = "tpr-section-cards";

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
                ContainerAttributes = output.Attributes.ToAttributeDictionary(),
                Cards = cardsContext.Cards.Select(c => new TprSectionCard
                {
                    CardAttributes = c.CardAttributes,
                    TitleAttributes = c.TitleAttributes,
                    Title = c.Title,
                    TitleUrl = c.TitleUrl,
                    TitleAllowHtml = c.TitleAllowHtml,
                    BodyAttributes = c.BodyAttributes,
                    ContentAttributes = c.ContentAttributes,
                    Content = c.Content,
                    ContentAllowHtml = c.ContentAllowHtml,


                }).ToList()
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
