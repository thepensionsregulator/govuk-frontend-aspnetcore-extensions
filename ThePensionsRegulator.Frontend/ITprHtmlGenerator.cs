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
        TagBuilder GenerateTprSectionCards(TprSectionCards tprSectionCards);
        TagBuilder GenerateTprAblePlayer(string id,string title,string videoId,bool autoplay,bool playsinline,string preload, string transcriptUrl, string transcriptTitle, string transcriptTarget);
        TagBuilder GenerateTprYoutubeNoCookiesEmbeddedPlayer(string id,string title, string videoId,bool autoplay,bool playsinline,string preload, string transcriptUrl, string transcriptTitle, string transcriptTarget);
        TagBuilder GenerateTprTimeline(AttributeDictionary? attributes, IEnumerable<TprTimelineItem> items, string dateSize);

    }
}
