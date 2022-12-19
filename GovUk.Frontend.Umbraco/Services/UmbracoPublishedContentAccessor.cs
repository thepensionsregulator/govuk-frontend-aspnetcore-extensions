using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco.Services
{
    public class UmbracoPublishedContentAccessor : IUmbracoPublishedContentAccessor
    {
        private IUmbracoContextAccessor _umbracoContextAccessor;
        private IUmbracoContext _umbracoContext;

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
        }

        public IPublishedContent PublishedContent => _umbracoContext.PublishedRequest.PublishedContent;
    }
}
