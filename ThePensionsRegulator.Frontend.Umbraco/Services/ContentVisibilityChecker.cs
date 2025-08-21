using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    interface IContentVisibilityChecker
    {
        bool IsVisible(IPublishedContent content);
    }

    internal class ContentVisibilityChecker : IContentVisibilityChecker
    {
        public bool IsVisible(IPublishedContent content)
        {
            return content.IsVisible();
        }

    }
}
