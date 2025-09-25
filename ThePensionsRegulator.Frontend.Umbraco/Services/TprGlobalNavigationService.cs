using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    internal class TprGlobalNavigationService : ITprGlobalNavigationService
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IPublishedUrlProvider _contentUrlProvider;
        private readonly IContentVisibilityChecker _contentVisibilityChecker;
     
        public TprGlobalNavigationService(IUmbracoContextAccessor umbracoContextAccessor, IPublishedUrlProvider contentUrlProvider, IContentVisibilityChecker contentVisibilityChecker)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _contentUrlProvider = contentUrlProvider ?? throw new ArgumentNullException(nameof(contentUrlProvider));
            _contentVisibilityChecker = contentVisibilityChecker;
        }

        public IList<TprHeaderMenuParentItem> GetMenuItems(Guid rootKey)
        {
            if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                throw new InvalidOperationException("Umbraco context is not available.");
            }

            var rootNode = umbracoContext.Content?.GetById(rootKey);

            if (rootNode == null)
            {
                throw new InvalidOperationException($"No content found with GUID {rootKey}.");
            }

            var menuItems = rootNode.Children
                .Where(x => _contentVisibilityChecker.IsVisible(x))
                .Select(x => new TprHeaderMenuParentItem
                {
                    ContentKey = x.Key,
                    LinkText = x.Name.ToFirstUpper(),
                    LinkUrl = _contentUrlProvider.GetUrl(x),
                    HeaderMenuChildItems = x.Children
                        .Where(c => _contentVisibilityChecker.IsVisible(c))
                        .Select(c => new TprHeaderMenuChildItem
                        {
                            ContentKey = c.Key,
                            LinkText = c.Name.ToFirstUpper(),
                            LinkUrl = _contentUrlProvider.GetUrl(c)
                        })
                        .ToList()
                })
                .ToList();

            return menuItems;
        }

        public IList<TprHeaderMenuParentItem> AddParentMenuItem(Guid rootKey, int index, TprHeaderMenuParentItem parentItem)
        {
            var navigation = GetMenuItems(rootKey);

            if (parentItem != null)
            {
                navigation.Insert(index, parentItem);
            }

            return navigation;
        }

        public IList<TprHeaderMenuParentItem> AddChildMenuItem(Guid rootKey, int index, int hierarchy, TprHeaderMenuChildItem childItem)
        {
            var navigation = GetMenuItems(rootKey);

            if (childItem != null)
            {
                navigation[index]?.HeaderMenuChildItems?.Insert(hierarchy, childItem);
            }

            return navigation;
        }
    }
}
