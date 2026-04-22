using Moq;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class UmbracoSideNavigationLinksServiceTests
    {
        [Fact]
        public void Returns_null_when_root_is_not_visible()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.CurrentPage.SetupUmbracoBooleanPropertyValue("umbracoNaviHide", true);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.Null(result);
        }

        [Fact]
        public void Returns_title_link_based_on_root_node()
        {
            var context = new UmbracoTestContext();

            var root = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Root page");
            root.Setup(x => x.Level).Returns(1);
            root.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(root.Object, UrlMode.Default, null)).Returns("/");

            context.CurrentPage.Setup(x => x.Level).Returns(2);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [root.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Equal("Root page", result.TitleLink.Name);
            Assert.Equal("/", result.TitleLink.Url);
        }

        [Fact]
        public void Title_link_is_current_page_when_current_page_is_root()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/");

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.True(result.TitleLink.IsCurrentPage);
        }

        [Fact]
        public void Title_link_is_not_current_page_when_current_page_is_a_child()
        {
            var context = new UmbracoTestContext();

            var root = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Root page");
            root.Setup(x => x.Level).Returns(1);
            root.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(root.Object, UrlMode.Default, null)).Returns("/");

            context.CurrentPage.Setup(x => x.Level).Returns(2);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [root.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.False(result.TitleLink.IsCurrentPage);
        }

        [Fact]
        public void Returns_empty_navigation_links_when_root_has_no_children()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/");

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Empty(result.NavigationLinks);
        }

        [Fact]
        public void Returns_navigation_links_for_children_of_root()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/");

            var childA = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Child A");
            childA.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(childA.Object, UrlMode.Default, null)).Returns("/child-a");

            var childB = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Child B");
            childB.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(childB.Object, UrlMode.Default, null)).Returns("/child-b");

            context.CurrentPage.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [childA.Object, childB.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Equal(2, result.NavigationLinks.Count);
            Assert.Equal("Child A", result.NavigationLinks[0].Name);
            Assert.Equal("/child-a", result.NavigationLinks[0].Url);
            Assert.Equal("Child B", result.NavigationLinks[1].Name);
            Assert.Equal("/child-b", result.NavigationLinks[1].Url);
        }

        [Fact]
        public void Excludes_hidden_children_from_navigation_links()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/");

            var visibleChild = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Visible");
            visibleChild.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(visibleChild.Object, UrlMode.Default, null)).Returns("/visible");

            var hiddenChild = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Hidden");
            hiddenChild.Setup(x => x.Level).Returns(2);
            hiddenChild.SetupUmbracoBooleanPropertyValue("umbracoNaviHide", true);

            context.CurrentPage.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [visibleChild.Object, hiddenChild.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Single(result.NavigationLinks);
            Assert.Equal("Visible", result.NavigationLinks[0].Name);
        }

        [Fact]
        public void Returns_nested_navigation_links()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/");

            var child = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Child");
            child.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(child.Object, UrlMode.Default, null)).Returns("/child");

            var grandchild = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Grandchild");
            grandchild.Setup(x => x.Level).Returns(3);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(grandchild.Object, UrlMode.Default, null)).Returns("/child/grandchild");

            context.CurrentPage.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [child.Object]);
            child.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [grandchild.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Single(result.NavigationLinks);
            Assert.Equal("Child", result.NavigationLinks[0].Name);
            Assert.Single(result.NavigationLinks[0].Children);
            Assert.Equal("Grandchild", result.NavigationLinks[0].Children[0].Name);
        }

        [Fact]
        public void Marks_current_page_in_navigation_links()
        {
            var context = new UmbracoTestContext();

            var root = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Root");
            root.Setup(x => x.Level).Returns(1);
            root.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(root.Object, UrlMode.Default, null)).Returns("/");

            context.CurrentPage.Setup(x => x.Level).Returns(2);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [root.Object]);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/current");

            root.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [context.CurrentPage.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Single(result.NavigationLinks);
            Assert.True(result.NavigationLinks[0].IsCurrentPage);
        }

        [Fact]
        public void Expands_ancestor_of_current_page_in_navigation_links()
        {
            var context = new UmbracoTestContext();

            var root = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Root");
            root.Setup(x => x.Level).Returns(1);
            root.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(root.Object, UrlMode.Default, null)).Returns("/");

            var parent = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Parent");
            parent.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(parent.Object, UrlMode.Default, null)).Returns("/parent");

            context.CurrentPage.Setup(x => x.Level).Returns(3);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [parent.Object, root.Object]);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/parent/current");

            root.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [parent.Object]);
            parent.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [context.CurrentPage.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Single(result.NavigationLinks);
            Assert.True(result.NavigationLinks[0].IsExpanded);
        }

        [Fact]
        public void Does_not_expand_non_ancestor_of_current_page_in_navigation_links()
        {
            var context = new UmbracoTestContext();

            var root = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Root");
            root.Setup(x => x.Level).Returns(1);
            root.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(root.Object, UrlMode.Default, null)).Returns("/");

            var sibling = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Sibling");
            sibling.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(sibling.Object, UrlMode.Default, null)).Returns("/sibling");

            context.CurrentPage.Setup(x => x.Level).Returns(2);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [root.Object]);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/current");

            root.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [context.CurrentPage.Object, sibling.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Equal(2, result.NavigationLinks.Count);
            Assert.False(result.NavigationLinks[1].IsExpanded);
        }

        [Fact]
        public void Sets_parent_on_child_navigation_links()
        {
            var context = new UmbracoTestContext();
            context.CurrentPage.Setup(x => x.Level).Returns(1);
            context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, []);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(context.CurrentPage.Object, UrlMode.Default, null)).Returns("/");

            var child = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Child");
            child.Setup(x => x.Level).Returns(2);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(child.Object, UrlMode.Default, null)).Returns("/child");

            var grandchild = UmbracoContentFactory.CreateContent<IPublishedContent>("page", "Grandchild");
            grandchild.Setup(x => x.Level).Returns(3);
            context.PublishedUrlProvider.Setup(x => x.GetUrl(grandchild.Object, UrlMode.Default, null)).Returns("/child/grandchild");

            context.CurrentPage.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [child.Object]);
            child.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [grandchild.Object]);

            var contentAccessor = new Mock<IUmbracoPublishedContentAccessor>();
            contentAccessor.Setup(x => x.PublishedContent).Returns(context.CurrentPage.Object);

            var service = CreateService(context, contentAccessor);

            var result = service.GetLinks();

            Assert.NotNull(result);
            Assert.Null(result.NavigationLinks[0].Parent);
            Assert.Same(result.NavigationLinks[0], result.NavigationLinks[0].Children[0].Parent);
        }

        private static UmbracoSideNavigationLinksService CreateService(UmbracoTestContext context, Mock<IUmbracoPublishedContentAccessor> contentAccessor)
        {
            return new UmbracoSideNavigationLinksService(
                contentAccessor.Object,
                context.DocumentNavigationQueryService.Object,
                context.PublishedContentStatusFilteringService.Object,
                context.PublishedUrlProvider.Object,
                context.PublishedValueFallback.Object);
        }
    }
}
