using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;


namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    internal class TprGlobalNavigationTests
    {
        public delegate void TryGetUmbracoContextCallback(out IUmbracoContext context);

        private Mock<IPublishedContent> _rootNode;
        private int _rootId;
        private TprGlobalNavigationService _sut;
        private string _expectedLinkText;
        private string _expectedLinkDestination;
        private List<Mock<IPublishedContent>> _childMocks;

        [SetUp]
        public void SetUp()
        {
            _rootId = 1;
            _rootNode = new Mock<IPublishedContent>();
            _rootNode.Setup(x => x.Id).Returns(_rootId);
            _rootNode.Setup(x => x.Name).Returns("Home");

            var fakeChecker = new FakeContentVisibilityChecker(true);

            var mockUrlProvider = new Mock<IContentUrlProvider>();

            var childContent = new Mock<IPublishedContent>();
            _childMocks = new List<Mock<IPublishedContent>>
            {
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
            };

            int index = 0;
            foreach (var child in _childMocks)
            {
                index++;

                child.Setup(x => x.Name).Returns($"Test content {index}");
                mockUrlProvider.Setup(x => x.GetUrl(It.IsAny<IPublishedContent>())).Returns($"/test{index}");
            }

            var children = _childMocks.Select(m => m.Object).ToList();

            _rootNode.Setup(x => x.Children).Returns(children);

            var mockNestedContent = new List<Mock<IPublishedContent>>
            {
                new Mock<IPublishedContent>(),
                new Mock<IPublishedContent>(),
            };

            var nestedContent = mockNestedContent.Select(m => m.Object).ToList();
            _childMocks[0].Setup(c => c.Children).Returns(nestedContent);

            var mockContentAccessor = new Mock<IUmbracoContextAccessor>();

            var mockContext = new Mock<IUmbracoContext>();

            mockContentAccessor.Setup(x => x.TryGetUmbracoContext(out It.Ref<IUmbracoContext>.IsAny)).Callback(new TryGetUmbracoContextCallback((out IUmbracoContext context) =>
            {
                context = mockContext.Object;
            }
            )).Returns(true);

            mockContext.Setup(x => x.Content.GetById(It.IsAny<int>())).Returns(_rootNode.Object);

            _sut = new TprGlobalNavigationService(mockContentAccessor.Object, mockUrlProvider.Object, fakeChecker);

            _expectedLinkText = "Test content 4";
            _expectedLinkDestination = "/test4";

        }

        [Test]
        public void GetMenuItems_ShouldReturnExpectedMenuItems()
        {
            //Arrange
            SetUp();

            //Act
            var result = _sut.GetMenuItems(_rootId);

            //Assert 
            Assert.That(result?.Count, Is.EqualTo(_rootNode.Object.Children.Count()));
            Assert.That(result[3].LinkText, Is.EqualTo(_expectedLinkText));
            Assert.That(result[3].LinkDestination, Is.EqualTo(_expectedLinkDestination));

            Assert.That(result[0]?.HeaderMenuChildItems?.Count, Is.EqualTo(_childMocks[0].Object.Children.Count()));
        }

        [Test]
        public void AddParentMenuItem_ShouldAdd_A_MenuItem_InCorrectLocation()
        {
            //Arrange
            SetUp();
            var newParentItem = new TprHeaderMenuParentItem();

            //Act
            var result = _sut.AddParentMenuItem(_rootId, 1, newParentItem);

            //Assert
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void AddChildMenuItem_ShouldAdd_A_MenuItem_InCorrectLocation()
        {
            //Arrange
            SetUp();
            var newChildItem = new TprHeaderMenuChildItem();

            //Act
            var result = _sut.AddChildMenuItem(_rootId, 1, 0, newChildItem);

            //Assert
            Assert.That(result[1]?.HeaderMenuChildItems?.Count, Is.EqualTo(1));
            Assert.That(result[3]?.HeaderMenuChildItems?.Count, Is.EqualTo(0));
        }
    }
}
