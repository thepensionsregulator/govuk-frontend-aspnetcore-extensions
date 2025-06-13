using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string SearchElement = "aside";

        public virtual TagBuilder GenerateTprSearch(string faqApiUrl)
        {
            var section = new TagBuilder(SearchElement);
            section.AddCssClass("tpr-search");
            section.Attributes.Add("id", "tpr-search");
            section.Attributes.Add("data-api-url", faqApiUrl);
            return section;
        }
    }
}
