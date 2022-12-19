using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPublishedContentAccessor
    {
        IPublishedContent PublishedContent { get; }
    }
}
