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
        private Mock<ITprGlobalNavigationService> _mockNavigationSerivce;
        private TprHeaderMenuBlockListGenerator _sut;
        private  Guid homeNodeId = Guid.NewGuid();
        private  Guid settingsNodeId = Guid.NewGuid();
        private const string TestPropertyAlias = "tprHeaderMenu";
        private string? _capturedJson;
        private List<TprHeaderMenuParentItem> _parentItems;

        public MenuBlockListGeneratorTests()
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
            _mockSettingsNode.Setup(x => x.Key).Returns(settingsNodeId);
            _mockSettingsNode.Setup(x => x.ContentType.Alias).Returns(TestPropertyAlias);

            _mockHomeNode = new Mock<IContent>();
            _mockHomeNode.Setup(x => x.Key).Returns(homeNodeId);

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

            _mockNavigationSerivce = new Mock<ITprGlobalNavigationService>();
            _mockNavigationSerivce.Setup(x => x.GetMenuItems(It.IsAny<Guid>())).Returns(_parentItems);

            _sut = new TprHeaderMenuBlockListGenerator(_mockContentService.Object, _mockContentTypeService.Object, _mockNavigationSerivce.Object);

            _capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => _capturedJson = value.ToString());

            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_WithValidSettingsNode_SetsValueAndSaves()
        {
            
            //Assert
            _mockContentService.Verify(x => x.GetById(settingsNodeId), Times.Once);
            _mockContentService.Verify(x => x.SaveAndPublish(_mockSettingsNode.Object, It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_CreatesCorrectJsonStructure()
        {

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(_capturedJson);

                var jsonObject = JObject.Parse(_capturedJson);
                Assert.True(jsonObject.ContainsKey("layout"));
                Assert.True(jsonObject.ContainsKey("contentData"));
                Assert.True(jsonObject.ContainsKey("settingsData"));

                var layout = jsonObject["layout"];
                Assert.Multiple(() =>
                {
                    Assert.True(layout.HasValues);
                    Assert.True(layout["Umbraco.BlockList"] != null);
                });

                var contentData = jsonObject["contentData"] as JArray;
                Assert.NotNull(contentData);
                Assert.True(contentData.Any());
            });
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_CreatesCorrectParentMenuItemStructure()
        {

            //Assert
            var jsonObject = JObject.Parse(_capturedJson);
            var contentData = jsonObject["contentData"] as JArray;
            var firstItem = contentData?[0] as JObject;

            Assert.Multiple(() =>
            {
                Assert.True(firstItem?.ContainsKey("contentTypeKey"));
                Assert.True(firstItem?.ContainsKey("linkText"));
                Assert.True(firstItem?.ContainsKey("linkUrl"));
                Assert.True(firstItem?.ContainsKey("udi"));

                var udi = firstItem?["udi"].ToString();
                Assert.True(udi?.StartsWith("umb://element/"));
            });

        }

        [Fact]
        public void GenerateChildMenuBlockList_CreatesNestedChildMenuItems()
        {

            //Assert
            var jsonObject = JObject.Parse(_capturedJson);
            var contentData = jsonObject["contentData"] as JArray;
            var childItemBlock = contentData?[1] as JObject;

            Assert.Multiple(() =>
            {

                Assert.True(childItemBlock?.ContainsKey("tprHeaderMenuChildItems"));

                var childItems = childItemBlock["tprHeaderMenuChildItems"] as JObject;
                Assert.NotNull(childItems);

                Assert.True(childItems.ContainsKey("layout"));
                Assert.True(childItems.ContainsKey("contentData"));
                Assert.True(childItems.ContainsKey("settingsData"));

                var childContentData = childItems["contentData"] as JArray;
                Assert.NotNull(childContentData);
                Assert.Equal(2, childContentData.Count);

                var firstChild = childContentData[0] as JObject;
                Assert.True(firstChild?.ContainsKey("contentTypeKey"));
                Assert.True(firstChild?.ContainsKey("linkText"));
                Assert.True(firstChild?.ContainsKey("linkUrl"));
                Assert.True(firstChild?.ContainsKey("udi"));
            });
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_DoesNotOverwrite_ExistingItems()
        {
            //Arrange
           

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert

        }

        [Fact]
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

                Assert.Equal(layoutUdi, contentUdi);
            }
        }
    }
}
