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
        //private const string capturedJson = null;

        [SetUp]
        public void SetUp()
        {
            _mockContentService = new Mock<IContentService>();
            _mockContentTypeService = new Mock<IContentTypeService>();

            _mockMainMenuType = new Mock<IContentType>();
            _mockMainMenuType.Setup(x => x.Alias).Returns("tprHeaderMenuParentItem");
            _mockMainMenuType.Setup(x => x.Key).Returns(Guid.NewGuid());


            _mockChildMenuType = new Mock<IContentType>();
            _mockChildMenuType.Setup(x => x.Alias).Returns("tprHeaderMenuChildItem");
            _mockChildMenuType.Setup(x => x.Key).Returns(Guid.NewGuid()); //This may need to be hard coded to show correct JSON structure

            _mockSettingsNode = new Mock<IContent>();
            _mockSettingsNode.Setup(x => x.Id).Returns(settingsNodeId);

            _mockHomeNode = new Mock<IContent>();
            _mockHomeNode.Setup(x => x.Id).Returns(homeNodeId);

            var contentTypes = new List<IContentType> { _mockMainMenuType.Object, _mockChildMenuType.Object };
            _mockContentTypeService.Setup(x => x.GetAll()).Returns(contentTypes);

            _mockContentService.Setup(x => x.GetById(settingsNodeId)).Returns(_mockSettingsNode.Object);

            _mockNavigationSerivce = new Mock<ITprGlobalNavigationSerivce>();
            _mockNavigationSerivce.Setup(x => x.GetMenuItems(It.IsAny<int>())).Returns(new List<TprHeaderMenuParentItem>
            {
                new TprHeaderMenuParentItem("link one", "/1", new List<TprHeaderMenuChildItem>
                {
                    new TprHeaderMenuChildItem("child link one", "/1.1"),
                    new TprHeaderMenuChildItem("child link two", "1.2")
                }),
                new TprHeaderMenuParentItem("link two", "/2")

            });


            _sut = new TprHeaderMenuBlockListGenerator(_mockContentService.Object, _mockContentTypeService.Object, _mockNavigationSerivce.Object);
        }

        [Test]
        public void GenerateTprHeaderMenuBlockList_WithValidSettingsNide_SetsValueAndSaves()
        {
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            _mockContentService.Verify(x => x.GetById(settingsNodeId), Times.Once);
            _mockContentService.Verify(x => x.SaveAndPublish(_mockSettingsNode.Object, It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void CreateBlockList_WithInvaldSettingsNode_ThrowsException()
        {
            //Arrange
        }

        [Test]
        public void GenerateTprHeaderMenuBlockList_CreatesCorrectJsonStructure()
        {
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(capturedJson, Is.Not.Null);

                var jsonObject = JObject.Parse(capturedJson);
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
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            var jsonObject = JObject.Parse(capturedJson);
            var contentData = jsonObject["contentData"] as JArray;
            var firstItem = contentData?[0] as JObject;

            Assert.True(firstItem?.ContainsKey("contentTypeKey"));
            Assert.True(firstItem?.ContainsKey("linkText"));
            Assert.True(firstItem?.ContainsKey("linkDestination"));
            Assert.True(firstItem?.ContainsKey("udi"));

            //Assert content tpye key are equal??

            var udi = firstItem?["udi"].ToString();
            Assert.True(udi?.StartsWith("umb://element/"));

        }

        [Test]
        public void GenerateChildMenuBlockList_CreatesNestedChildMenuItems()
        {
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            var jsonObject = JObject.Parse(capturedJson);
            var contentData = jsonObject["contentData"] as JArray;
            var childItemBlock = contentData?[1] as JObject;

            Assert.True(childItemBlock?.ContainsKey("tprHeaderMenuChildItems"));

            var childItems = childItemBlock["tprHeaderMenuChildItems"] as JObject;
            Assert.NotNull(childItems);

            Assert.That(childItems.ContainsKey("layout"), Is.True);
            Assert.That(childItems.ContainsKey("contentData"), Is.True);
            Assert.That(childItems.ContainsKey("settingsData"), Is.True);

            var childContentData = childItems["contentData"] as JArray;
            Assert.NotNull(childContentData);
            Assert.AreEqual(2, childContentData.Count);

            var firstChild = childContentData[0] as JObject;
            Assert.True(firstChild?.ContainsKey("contentTypeKey"));
            Assert.True(firstChild?.ContainsKey("linkText"));
            Assert.True(firstChild?.ContainsKey("linkDestination"));
            Assert.True(firstChild?.ContainsKey("udi"));

            //Assert guid
        }

        [Test]
        public void GenerateChildMenuBlockList_HandlesEmptyUrls()
        {
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert

        }

        [Test]
        public void GenerateChildMenuBlockList_CreatesValidUdiReference()
        {
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            var jsonObject = JObject.Parse(capturedJson);
            var layout = jsonObject["layout"]["Umbraco.BlockList"] as JArray;
            var contentData = jsonObject["contentData"] as JArray;

            for(int i= 0; i < layout.Count;i = i + 2)
            {
                var layoutUdi = layout[i]["contentUdi"].ToString();
                var contentUdi = contentData[i]["udi"].ToString();

                Assert.AreEqual(layoutUdi, contentUdi);
            }
        }
        //This may not be needed, as umbraco should do this 
        [Test]
        public void GenerateChildMenuBlockList_ProduceCompatibleOutput()
        {
            //Arrange
            string capturedJson = null;
            _mockSettingsNode.Setup(x => x.SetValue(TestPropertyAlias, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).Callback<string, object, string, string>((alias, value, z, y) => capturedJson = value.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert

            var simulatedXmlStoreage = $"<{TestPropertyAlias}><Value><![CDATA[{capturedJson}]]></Value></{TestPropertyAlias}>";

            var startTag = "<Value><![CDATA[";
            var endTaga = "]]></Value>";
            var startIndex = simulatedXmlStoreage.IndexOf(startTag) + startTag.Length;
            var endIndex = simulatedXmlStoreage.IndexOf(endTaga);

            var extractedJson = simulatedXmlStoreage.Substring(startIndex, endIndex - startIndex);

            Assert.AreEqual(capturedJson, extractedJson);

        }
    }
}
