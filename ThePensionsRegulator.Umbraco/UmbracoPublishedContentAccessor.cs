using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Umbraco
{
    /// <summary>
    /// Accessor for the published content node matching the current request
    /// </summary>
    public class UmbracoPublishedContentAccessor : IUmbracoPublishedContentAccessor
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IUmbracoContext _umbracoContext;
        private readonly IPublishedContent _publishedContent;

        public UmbracoPublishedContentAccessor(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                throw new InvalidOperationException("Unable to get Umbraco context");
            }

            if (umbracoContext.PublishedRequest?.PublishedContent == null)
            {
                throw new InvalidOperationException("Unable to get Umbraco published content");
            }
            _umbracoContext = umbracoContext;
            _publishedContent = _umbracoContext.PublishedRequest.PublishedContent;
        }

        /// <inheritdoc />
        public IPublishedContent PublishedContent => _publishedContent;
    }
}
