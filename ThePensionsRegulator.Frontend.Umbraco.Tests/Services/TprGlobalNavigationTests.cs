using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;


namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprGlobalNavigationTests
    {
        public delegate void TryGetUmbracoContextCallback(out IUmbracoContext context);

        private Mock<IPublishedContent> _rootNode;
        private string _guidString;
        private Guid _rootKey;
        private TprGlobalNavigationService _sut;
        private List<Mock<IPublishedContent>> _content;

        public TprGlobalNavigationTests()
        {
            _guidString = "36cd375f-4aa3-4e61-9526-8e69642f106a";
            _rootKey = Guid.Parse(_guidString);
            _rootNode = new Mock<IPublishedContent>();
            _rootNode.Setup(x => x.Key).Returns(_rootKey);
            _rootNode.Setup(x => x.Name).Returns("Home");

            var mockUrlProvider = new Mock<IPublishedUrlProvider>();

            _content = new List<Mock<IPublishedContent>>
            {
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
            };

            var children = _content.Select(m => m.Object).ToList();

            _rootNode.Setup(x => x.Children).Returns(children);

            var childContent = new Mock<IPublishedContent>();
            childContent.Setup(x => x.Name).Returns("test");

            var mockNestedContent = new List<Mock<IPublishedContent>>
            {
                childContent,
                childContent,
            };

            var nestedContent = mockNestedContent.Select(m => m.Object).ToList();
            _content[0].Setup(c => c.Children).Returns(nestedContent);

            for (int i = 0; i < _content.Count; i++)
            {
                _content[i].Setup(x => x.Name).Returns($"Test content {i}");

            }
            mockUrlProvider.SetupSequence(x => x.GetUrl(It.IsAny<IPublishedContent>(), It.IsAny<UrlMode>(), It.IsAny<string>(), It.IsAny<Uri>())).Returns("/0")
                .Returns("/0.1")
                .Returns("/0.2")
                .Returns("/1")
                .Returns("/2")
                .Returns("/3");


            var mockContentAccessor = new Mock<IUmbracoContextAccessor>();

            var mockContext = new Mock<IUmbracoContext>();

            mockContext.Setup(x => x!.Content!.GetById(It.IsAny<Guid>())).Returns(_rootNode.Object);

            mockContentAccessor.Setup(x => x.TryGetUmbracoContext(out It.Ref<IUmbracoContext?>.IsAny)).Callback(new TryGetUmbracoContextCallback((out IUmbracoContext context) =>
            {
                context = mockContext.Object;

            }
            )).Returns(true);


            var fakeChecker = new FakeContentVisibilityChecker(true);

            _sut = new TprGlobalNavigationService(mockContentAccessor.Object, mockUrlProvider.Object, fakeChecker);
        }

        [Fact]
        public void GetMenuItems_ShouldReturnExpectedMenuItems()
        {

            //Act
            var result = _sut.GetMenuItems(_rootKey);

            //Assert 
            Assert.Multiple(() =>
            {
                Assert.Equal(result?.Count, _rootNode.Object.Children.Count());

                int index = 0;

                foreach (var child in _rootNode.Object.Children)
                {
                    Assert.Equal(result?[index].LinkText, $"Test content {index}");
                    Assert.Equal(result?[index].LinkUrl, $"/{index}");
                    index++;
                }

                Assert.Equal("/0.1", result?[0].HeaderMenuChildItems?[0].LinkUrl);
                Assert.Equal("Test", result?[0].HeaderMenuChildItems?[0].LinkText);
                Assert.Equal("Test", result?[0].HeaderMenuChildItems?[1].LinkText);
                Assert.Equal("/0.2", result?[0].HeaderMenuChildItems?[1].LinkUrl);
                Assert.Equal(result?[0]?.HeaderMenuChildItems?.Count, _content[0].Object.Children.Count());
            });
        }

        [Fact]
        public void AddParentMenuItem_ShouldAdd_A_MenuItem_InCorrectLocation()
        {
            //Arrange
            var newParentItem = new TprHeaderMenuParentItem("", "");

            //Act
            var result = _sut.AddParentMenuItem(_rootKey, 1, newParentItem);

            //Assert
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void AddChildMenuItem_ShouldAdd_A_MenuItem_InCorrectLocation()
        {
            //Arrange
            var newChildItem = new TprHeaderMenuChildItem("", "");

            //Act
            var result = _sut.AddChildMenuItem(_rootKey, 1, 0, newChildItem);

            //Assert
            Assert.Equal(1, result[1]?.HeaderMenuChildItems?.Count);
            Assert.Equal(0, result[3]?.HeaderMenuChildItems?.Count);
        }
    }
}
