using Humanizer;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Services
{
    public class DefaultBreadcrumbLinksServiceTests
    {
        [Fact]
        public void Returns_empty_collection_when_a_page_has_no_parent()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);

            var service = new DefaultBreadcrumbLinksService(context.DocumentNavigationQueryService.Object, context.PublishedContentStatusFilteringService.Object);

            var result = service.GetLinks(context.CurrentPage.Object);

            Assert.Null(result.CurrentPage);
            Assert.Empty(result.Links);
        }

        [Fact]
        public void Returns_collection_based_on_ancestors_ordered_by_level()
        {
            var context = new UmbracoTestContext();

            var greatGrandparent = UmbracoContentFactory.CreateContent<IPublishedContent>("example", "great grandparent");
            greatGrandparent.Setup(x => x.Level).Returns(1);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(greatGrandparent.Object, UrlMode.Default, null)).Returns($"/{greatGrandparent.Object.Name.Kebaberize()}");

            var grandparent = UmbracoContentFactory.CreateContent<IPublishedContent>("example", "grandparent");
            grandparent.Setup(x => x.Level).Returns(2);
            grandparent.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [greatGrandparent.Object]);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(grandparent.Object, UrlMode.Default, null)).Returns($"/{grandparent.Object.Name.Kebaberize()}");

            var parent = UmbracoContentFactory.CreateContent<IPublishedContent>("example", "parent");
            parent.Setup(x => x.Level).Returns(3);
            parent.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [grandparent.Object, greatGrandparent.Object]);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(parent.Object, UrlMode.Default, null)).Returns($"/{parent.Object.Name.Kebaberize()}");

            context.CurrentPage.Setup(x => x.Level).Returns(4);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [parent.Object, grandparent.Object, greatGrandparent.Object]);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns($"/{context.CurrentPage.Object.Name.Kebaberize()}");

            var service = new DefaultBreadcrumbLinksService(context.DocumentNavigationQueryService.Object, context.PublishedContentStatusFilteringService.Object);

            var result = service.GetLinks(context.CurrentPage.Object);

            Assert.Equal(context.CurrentPage.Object, result.CurrentPage);
            Assert.Equal(4, result.Links.Count);

            Assert.Equal(greatGrandparent.Object.Name, result.Links[0].Name);
            Assert.Equal(grandparent.Object.Name, result.Links[1].Name);
            Assert.Equal(parent.Object.Name, result.Links[2].Name);
            Assert.Equal(context.CurrentPage.Object.Name, result.Links[3].Name);

            Assert.Equal(greatGrandparent.Object.Url(), result.Links[0].Url);
            Assert.Equal(grandparent.Object.Url(), result.Links[1].Url);
            Assert.Equal(parent.Object.Url(), result.Links[2].Url);
            Assert.Null(result.Links[3].Url);
        }
    }
}
