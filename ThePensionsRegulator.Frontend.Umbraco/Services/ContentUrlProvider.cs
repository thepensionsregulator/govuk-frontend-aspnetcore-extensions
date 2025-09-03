using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface IContentUrlProvider
    {
        string GetUrl(IPublishedContent content);
    }
    internal class ContentUrlProvider : IContentUrlProvider
    {
        public string GetUrl(IPublishedContent content)
        {
            return content.Url();
        }
    }
}
