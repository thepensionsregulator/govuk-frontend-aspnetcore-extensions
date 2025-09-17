using Moq;
using NUnit.Framework.Internal;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;


namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    internal class TprGlobalNavigationTests
    {
        public delegate void TryGetUmbracoContextCallback(out IUmbracoContext context);

        private Mock<IPublishedContent> _rootNode;
        private string _guidString;
        private Guid _rootKey;
        private TprGlobalNavigationService _sut;
        private List<Mock<IPublishedContent>> _content;
        private List<Mock<IPublishedContent>> _test;

        [SetUp]
        public void SetUp()
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

            mockContentAccessor.Setup(x => x.TryGetUmbracoContext(out It.Ref<IUmbracoContext>.IsAny)).Callback(new TryGetUmbracoContextCallback((out IUmbracoContext context) =>
            {
                context = mockContext.Object;
            }
            )).Returns(true);

            mockContext.Setup(x => x.Content.GetById(It.IsAny<Guid>())).Returns(_rootNode.Object);

            var fakeChecker = new FakeContentVisibilityChecker(true);

            _sut = new TprGlobalNavigationService(mockContentAccessor.Object, mockUrlProvider.Object, fakeChecker);
        }

        [Test]

        public void GetMenuItems_ShouldReturnExpectedMenuItems()
        {

            //Act
            var result = _sut.GetMenuItems(_rootKey);

            //Assert 
            Assert.Multiple(() =>
            {
                Assert.That(result?.Count, Is.EqualTo(_rootNode.Object.Children.Count()));

                int index = 0;

                foreach (var child in _rootNode.Object.Children)
                {
                    Assert.That(result?[index].LinkText, Is.EqualTo($"Test content {index}"));
                    Assert.That(result?[index].LinkUrl, Is.EqualTo($"/{index}"));
                    index++;
                }

                Assert.That(result?[0].HeaderMenuChildItems?[0].LinkUrl, Is.EqualTo("/0.1"));
                Assert.That(result?[0].HeaderMenuChildItems?[0].LinkText, Is.EqualTo("Test"));
                Assert.That(result?[0].HeaderMenuChildItems?[1].LinkUrl, Is.EqualTo("/0.2"));
                Assert.That(result?[0]?.HeaderMenuChildItems?.Count, Is.EqualTo(_content[0].Object.Children.Count()));
            });
           
        }

        [Test]
        public void AddParentMenuItem_ShouldAdd_A_MenuItem_InCorrectLocation()
        {
            //Arrange
            var newParentItem = new TprHeaderMenuParentItem("", "");

            //Act
            var result = _sut.AddParentMenuItem(_rootKey, 1, newParentItem);

            //Assert
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void AddChildMenuItem_ShouldAdd_A_MenuItem_InCorrectLocation()
        {
            //Arrange
            var newChildItem = new TprHeaderMenuChildItem("", "");

            //Act
            var result = _sut.AddChildMenuItem(_rootKey, 1, 0, newChildItem);

            //Assert
            Assert.That(result[1]?.HeaderMenuChildItems?.Count, Is.EqualTo(1));
            Assert.That(result[3]?.HeaderMenuChildItems?.Count, Is.EqualTo(0));
        }
    }
}
