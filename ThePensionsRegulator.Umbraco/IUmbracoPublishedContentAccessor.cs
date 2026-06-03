using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco
{
    public interface IUmbracoPublishedContentAccessor
    {
        /// <summary>
        /// The published content node matching the current request
        /// </summary>
        IPublishedContent PublishedContent { get; }
    }
}
