using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;


namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprGlobalNavigationTests : IDisposable
    {
        private readonly UmbracoTestContext _testContext;
        private Mock<IPublishedContent> _settingsNode;
        private TprGlobalNavigationService _sut;
        private TprHeaderMenuViewModel _menuViewModel;

        public TprGlobalNavigationTests()
        {
            _testContext = new UmbracoTestContext();

            var children = new List<TprHeaderMenuChildItem>
            {
                new TprHeaderMenuChildItem("ChildTest", "/childTest"),
                new TprHeaderMenuChildItem("ChildTest1", "/childTest1"),
            };

            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel([CreateMenuBlock("Test", "/test", children), CreateMenuBlock("Test1", "/test1", null)]);
            _settingsNode = UmbracoContentFactory.CreateContent<IPublishedContent>();
            _settingsNode.SetupUmbracoBlockListPropertyValue(TprElementTypeAliases.HeaderMenu, blockList);

            _sut = new TprGlobalNavigationService(_testContext.PublishedValueFallback.Object);

            _menuViewModel = new TprHeaderMenuViewModel(TprElementTypeAliases.HeaderMenu, TprPropertyAliases.HeaderMenuLinkText, TprPropertyAliases.HeaderMenuLinkUrl, TprElementTypeAliases.HeaderMenuChildItems);
        }

        [Fact]
        public void GetMenuItems_ShouldReturnExpectedMenuItems()
        {
            //Act
            var result = _sut.GetMenuItems(_settingsNode.Object, _menuViewModel);

            var childItems = result[0]?.HeaderMenuChildItems;

            //Assert 
            Assert.Multiple(() =>
            {
                Assert.Equal(2, result.Count);
                Assert.Equal("Test", result[0].LinkText);
                Assert.Equal("/test", result[0].LinkUrl);

                Assert.Equal(2, result[0]?.HeaderMenuChildItems?.Count);
                Assert.Equal("ChildTest", childItems?[0].LinkText);
                Assert.Equal("/childTest", childItems?[0]?.LinkUrl);
                Assert.Equal("ChildTest1", childItems?[1]?.LinkText);
                Assert.Equal("/childTest1", childItems?[1]?.LinkUrl);

                Assert.Equal("Test1", result[1].LinkText);
                Assert.Equal("/test1", result[1].LinkUrl);
                Assert.Equal(0, result[1]?.HeaderMenuChildItems?.Count);
            });
        }

        [Fact]
        public void GetMenuItems_WhenSettingsNodeIsNull_ReturnsEmptyList()
        {
            //Act
            var result = _sut.GetMenuItems(null!, _menuViewModel);

            //Arrange
            Assert.Empty(result);
        }

        [Fact]
        public void GetMenuItems_WhenHeaderMenuBlockListAliasIsNull_ReturnsEmptyList()
        {
            //Act
            var result = _sut.GetMenuItems(_settingsNode.Object, null!);

            //Arrange
            Assert.Empty(result);
        }

        public void Dispose() => _testContext.Dispose();

        private OverridableBlockListItem CreateMenuBlock(string linkText, string linkUrl, List<TprHeaderMenuChildItem>? childItems = null)
        {
            var contentElement = UmbracoBlockListFactory.CreateContentOrSettings();
            contentElement.SetupUmbracoTextboxPropertyValue(TprPropertyAliases.HeaderMenuLinkText, linkText);
            contentElement.SetupUmbracoMultiUrlPickerPropertyValue(TprPropertyAliases.HeaderMenuLinkUrl, new Link { Url = linkUrl });
            if (childItems != null)
            {
                contentElement.SetupUmbracoBlockListPropertyValue(TprElementTypeAliases.HeaderMenuChildItems, UmbracoBlockListFactory.CreateOverridableBlockListModel(CreateChildMenuBlock(childItems)));
            }
            var block = UmbracoBlockListFactory.CreateOverridableBlock(contentElement.Object);

            return block;
        }

        private List<OverridableBlockListItem> CreateChildMenuBlock(List<TprHeaderMenuChildItem> childItems)
        {
            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var child in childItems)
            {
                var contentElement = UmbracoBlockListFactory.CreateContentOrSettings();
                contentElement.SetupUmbracoTextboxPropertyValue(TprPropertyAliases.HeaderMenuLinkText, child.LinkText);
                contentElement.SetupUmbracoMultiUrlPickerPropertyValue(TprPropertyAliases.HeaderMenuLinkUrl, new Link { Url = child.LinkUrl });

                var block = UmbracoBlockListFactory.CreateOverridableBlock(contentElement.Object);

                blockListItems.Add(block);
            }
            return blockListItems;
        }
    }
}

