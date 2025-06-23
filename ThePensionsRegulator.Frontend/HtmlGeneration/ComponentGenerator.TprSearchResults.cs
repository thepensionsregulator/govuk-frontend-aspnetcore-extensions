using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string SearchElement = "aside";

        public virtual TagBuilder GenerateTprSearchResults(string popularContentApiUrl, string searchContentApiUrl, string contentByIdUrl)
        {
            var section = new TagBuilder(SearchElement);
            section.AddCssClass("tpr-search-results");
            section.Attributes.Add("data-popular-content-url", popularContentApiUrl);
            section.Attributes.Add("data-search-content-url", searchContentApiUrl);
            section.Attributes.Add("data-content-by-id-url", contentByIdUrl);
            return section;
        }
    }
}
