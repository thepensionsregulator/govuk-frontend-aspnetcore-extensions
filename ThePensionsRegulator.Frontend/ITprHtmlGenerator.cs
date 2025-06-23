using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend
{
    public interface ITprHtmlGenerator
    {
        TagBuilder GenerateTprBackToTop(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprBackToMenu(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprHeaderBar(TprHeaderBar tprHeaderBar);
        TagBuilder GenerateTprFooterBar(TprFooterBar tprFooterBar);
        TagBuilder GenerateTprContextBar(TprContextBar tprContextBar);
        TagBuilder GenerateTprRelatedLinks(TprRelatedLinks tprRelatedLinks);
        TagBuilder GenerateTprSectionCards(TprSectionCards tprSectionCards);
        TagBuilder GenerateTprAblePlayer(TprYouTubeVideo video);
        TagBuilder GenerateTprYouTubeNoCookiesEmbeddedPlayer(TprYouTubeVideo video);
        TagBuilder GenerateTprTimeline(AttributeDictionary? attributes, IEnumerable<TprTimelineItem> items, bool hideTail, int headingLevel, string ariaTitle);
        TagBuilder GenerateTprMobileMenu(TprMobileMenu tprMobileMenu);

    }
}
