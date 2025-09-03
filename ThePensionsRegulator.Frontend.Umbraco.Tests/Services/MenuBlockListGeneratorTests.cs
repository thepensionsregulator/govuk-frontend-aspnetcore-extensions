using Moq;
using Newtonsoft.Json.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class MenuBlockListGeneratorTests
    {
        private Mock<IContentService> _mockContentService;
        private Mock<IContentTypeService> _mockContentTypeService;
        private Mock<IContent> _mockSettingsNode;
        private Mock<IContent> _mockHomeNode;
        private Mock<IContentType> _mockMainMenuType;
        private Mock<IContentType> _mockChildMenuType;
        private Mock<ITprGlobalNavigationSerivce> _mockNavigationSerivce;
        private TprHeaderMenuBlockListGenerator _sut;
        private const int homeNodeId = 1234;
        private const int settingsNodeId = 567;
        private const string TestPropertyAlias = "tprHeaderMenu";
        private string? _capturedJson;
        private List<TprHeaderMenuParentItem> _parentItems;

        [SetUp]
        public void SetUp()
        {
            _mockContentService = new Mock<IContentService>();
            _mockContentTypeService = new Mock<IContentTypeService>();

            _mockMainMenuType = new Mock<IContentType>();
            _mockMainMenuType.Setup(x => x.Alias).Returns("tprHeaderMenuParentItems");
            _mockMainMenuType.Setup(x => x.Key).Returns(Guid.NewGuid());


            _mockChildMenuType = new Mock<IContentType>();
            _mockChildMenuType.Setup(x => x.Alias).Returns("tprHeaderMenuChildItem");
            _mockChildMenuType.Setup(x => x.Key).Returns(Guid.NewGuid());

            _mockSettingsNode = new Mock<IContent>();
            _mockSettingsNode.Setup(x => x.Id).Returns(settingsNodeId);
            _mockSettingsNode.Setup(x => x.ContentType.Alias).Returns(TestPropertyAlias);

            _mockHomeNode = new Mock<IContent>();
            _mockHomeNode.Setup(x => x.Id).Returns(homeNodeId);

            var contentTypes = new List<IContentType> { _mockMainMenuType.Object, _mockChildMenuType.Object };
            _mockContentTypeService.Setup(x => x.GetAll()).Returns(contentTypes);

            _mockContentService.Setup(x => x.GetById(settingsNodeId)).Returns(_mockSettingsNode.Object);

            _parentItems = new List<TprHeaderMenuParentItem>  {
                new TprHeaderMenuParentItem("link one", "/1", new List<TprHeaderMenuChildItem>
                {
                    new TprHeaderMenuChildItem("child link one", "/1.1"),
                    new TprHeaderMenuChildItem("child link two", "1.2")
                }),
                new TprHeaderMenuParentItem("link two", "/2")

            };

            _mockNavigationSerivce = new Mock<ITprGlobalNavigationSerivce>();
            _mockNavigationSerivce.Setup(x => x.GetMenuItems(It.IsAny<int>())).Returns(_parentItems);

            _sut = new TprHeaderMenuBlockListGenerator(_mockContentService.Object, _mockContentTypeService.Object, _mockNavigationSerivce.Object);

            _capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => _capturedJson = value.ToString());

            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);
        }

        [Test]
        public void GenerateTprHeaderMenuBlockList_WithValidSettingsNode_SetsValueAndSaves()
        {
            
            //Assert
            _mockContentService.Verify(x => x.GetById(settingsNodeId), Times.Once);
            _mockContentService.Verify(x => x.SaveAndPublish(_mockSettingsNode.Object, It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void CreateBlockList_WithInvaldSettingsNode_ThrowsException()
        {
            //Arrange
            var invalidSettingsNodeId = 321;
            var expectedExceptionMessage = "Settings node Id cannot be null";

            //Assert
            var ex = Assert.Throws<ArgumentException>(() => _sut.GenerateTprHeaderMenuBlockList(homeNodeId, invalidSettingsNodeId));
            Assert.That(ex.Message, Is.EqualTo(expectedExceptionMessage));         
        }

        [Test]
        public void GenerateTprHeaderMenuBlockList_CreatesCorrectJsonStructure()
        {

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(_capturedJson, Is.Not.Null);

                var jsonObject = JObject.Parse(_capturedJson);
                Assert.That(jsonObject.ContainsKey("layout"), Is.True);
                Assert.That(jsonObject.ContainsKey("contentData"), Is.True);
                Assert.That(jsonObject.ContainsKey("settingsData"), Is.True);

                var layout = jsonObject["layout"];
                Assert.Multiple(() =>
                {
                    Assert.That(layout.HasValues, Is.True);
                    Assert.That(layout["Umbraco.BlockList"] != null, Is.True);
                });

                var contentData = jsonObject["contentData"] as JArray;
                Assert.That(contentData, Is.Not.Null);
                Assert.That(contentData.Any(), Is.True);
            });
        }

        [Test]
        public void GenerateTprHeaderMenuBlockList_CreatesCorrectParentMenuItemStructure()
        {

            //Assert
            var jsonObject = JObject.Parse(_capturedJson);
            var contentData = jsonObject["contentData"] as JArray;
            var firstItem = contentData?[0] as JObject;

            Assert.Multiple(() =>
            {
                Assert.That(firstItem?.ContainsKey("contentTypeKey"), Is.True);
                Assert.That(firstItem?.ContainsKey("linkText"), Is.True);
                Assert.That(firstItem?.ContainsKey("linkUrl"), Is.True);
                Assert.That(firstItem?.ContainsKey("udi"), Is.True);

                var udi = firstItem?["udi"].ToString();
                Assert.That(udi?.StartsWith("umb://element/"), Is.True);
            });

        }

        [Test]
        public void GenerateChildMenuBlockList_CreatesNestedChildMenuItems()
        {

            //Assert
            var jsonObject = JObject.Parse(_capturedJson);
            var contentData = jsonObject["contentData"] as JArray;
            var childItemBlock = contentData?[1] as JObject;

            Assert.Multiple(() =>
            {

                Assert.That(childItemBlock?.ContainsKey("tprHeaderMenuChildItems"), Is.True);

                var childItems = childItemBlock["tprHeaderMenuChildItems"] as JObject;
                Assert.That(childItems, Is.Not.Null);

                Assert.That(childItems.ContainsKey("layout"), Is.True);
                Assert.That(childItems.ContainsKey("contentData"), Is.True);
                Assert.That(childItems.ContainsKey("settingsData"), Is.True);

                var childContentData = childItems["contentData"] as JArray;
                Assert.That(childContentData, Is.Not.Null);
                Assert.That(childContentData.Count, Is.EqualTo(2));

                var firstChild = childContentData[0] as JObject;
                Assert.That(firstChild?.ContainsKey("contentTypeKey"), Is.True);
                Assert.That(firstChild?.ContainsKey("linkText"), Is.True);
                Assert.That(firstChild?.ContainsKey("linkUrl"), Is.True);
                Assert.That(firstChild?.ContainsKey("udi"), Is.True);
            });
        }

        [Test]
        public void GenerateChildMenuBlockList_HandlesEmptyUrls()
        {
            //Arrange
           

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert

        }

        [Test]
        public void GenerateChildMenuBlockList_CreatesValidUdiReference()
        {
            //Assert
            var jsonObject = JObject.Parse(_capturedJson);
            var layout = jsonObject?["layout"]["Umbraco.BlockList"] as JArray;
            var contentData = jsonObject["contentData"] as JArray;

            for (int i = 0; i < layout?.Count; i = i + 2)
            {
                var layoutUdi = layout[i]["contentUdi"].ToString();
                var contentUdi = contentData[i]["udi"].ToString();

                Assert.That(contentUdi, Is.EqualTo(layoutUdi));
            }
        }
    }
}
