using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend
{
    public interface ITprHtmlGenerator
    {
        TagBuilder GenerateTprBackToTop(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprBackToMenu(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprHeaderBar(TprHeaderBar tprHeaderBar);
        TagBuilder GenerateTprFooterBar(TprFooterBar tprFooterBar);
        TagBuilder GenerateTprContextBar(TprContextBar tprContextBar);
        TagBuilder GenerateTprFeaturedImage(AttributeDictionary? attributes, string? imageUrl, string? imageAlt, IHtmlContent? htmlContent, bool horizontal, bool decorativeImage);
        TagBuilder GenerateTprRelatedLinks(TprRelatedLinks tprRelatedLinks);
        TagBuilder GenerateTprSectionCards(TprSectionCards tprSectionCards);
        TagBuilder GenerateTprAblePlayer(TprYouTubeVideo video);
        TagBuilder GenerateTprYouTubeNoCookiesEmbeddedPlayer(TprYouTubeVideo video);
        TagBuilder GenerateTprTimeline(AttributeDictionary? attributes, IEnumerable<TprTimelineItem> items, bool hideTail, int headingLevel, string ariaTitle);
        TagBuilder GenerateTprSearchResults(string popularContentUrl, string searchContentUrl, string contentByIdUrl);
        TagBuilder GenerateTprSearchResultsFooterLinks(TprSearchFooterLinks tprSearchFooterLinks);
        TagBuilder GenerateTprSearchResultsInput(int headingLevel, string headingClass, string? label = null);
        TagBuilder GenerateTprAddressLookup(bool isLegendPageHeading, AttributeDictionary? legendAttributes, IHtmlContent? legend, IHtmlContent? childContent, string? fieldsetDescribedBy, AddressLookupRole role, string? sameAsPrimaryCheckboxLabel);
    }
}
