using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    internal class TprGlobalNavigationService : ITprGlobalNavigationSerivce
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IContentUrlProvider _contentUrlProvider;
        private readonly IContentVisibilityChecker _contentVisibilityChecker;

        public TprGlobalNavigationService(IUmbracoContextAccessor umbracoContextAccessor, IContentUrlProvider contentUrlProvider, IContentVisibilityChecker contentVisibilityChecker)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _contentUrlProvider = contentUrlProvider ?? throw new ArgumentNullException(nameof(contentUrlProvider));
            _contentVisibilityChecker = contentVisibilityChecker ?? throw new ArgumentNullException(nameof(contentVisibilityChecker));
        }

        public List<TprHeaderMenuParentItem> GetMenuItems(int rootId)
        {
            if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                throw new InvalidOperationException("Umbraco context is not available.");
            }

            var rootNode = umbracoContext.Content?.GetById(rootId);

            if (rootNode == null)
            {
                throw new InvalidOperationException($"No content found with ID {rootId}.");
            }

            var menuItems = rootNode.Children
                .Where(x => _contentVisibilityChecker.IsVisible(x))
                .Select(x => new TprHeaderMenuParentItem
                {
                    LinkText = x.Name.ToFirstUpper(),
                    LinkDestination = _contentUrlProvider.GetUrl(x),
                    HeaderMenuChildItems = x.Children
                        .Where(c => _contentVisibilityChecker.IsVisible(c))
                        .Select(c => new TprHeaderMenuChildItem
                        {
                            LinkText = c.Name.ToFirstUpper(),
                            LinkDestination = _contentUrlProvider.GetUrl(c)
                        })
                        .ToList()
                })
                .ToList();

            return menuItems;
        }

        public List<TprHeaderMenuParentItem> AddParentMenuItem(int rootId, int placement, TprHeaderMenuParentItem parentItem)
        {
            var navigation = GetMenuItems(rootId);

            if (parentItem != null)
            {
                navigation.Insert(placement, parentItem);
            }

            return navigation;
        }

        public List<TprHeaderMenuParentItem> AddChildMenuItem(int rootId, int placement, int hierarchy, TprHeaderMenuChildItem tprMobileMenuChildItem)
        {
            var navigation = GetMenuItems(rootId);

            if (tprMobileMenuChildItem != null)
            {
                navigation[placement]?.HeaderMenuChildItems?.Insert(hierarchy, tprMobileMenuChildItem);
            }

            return navigation;
        }
    }
}
