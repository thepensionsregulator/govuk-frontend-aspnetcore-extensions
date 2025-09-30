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
        private Guid homeNodeId = Guid.NewGuid();
        private Guid settingsNodeId = Guid.NewGuid();
        private const string TestPropertyAlias = "tprHeaderMenu";
        private string? _capturedJson;
        private List<TprHeaderMenuParentItem> _parentItems;

        public MenuBlockListGeneratorTests()
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
                new TprHeaderMenuParentItem(Guid.NewGuid(),"link one", "/1", new List<TprHeaderMenuChildItem>
                {
                    new TprHeaderMenuChildItem(Guid.NewGuid(),"child link one", "/1.1"),
                    new TprHeaderMenuChildItem(Guid.NewGuid(), "child link two", "1.2")
                }),
                new TprHeaderMenuParentItem(Guid.NewGuid(), "link two", "/2")

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
            var childItemBlock = contentData?[0] as JObject;

            //Assert.Multiple(() =>
            //{

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
            //});
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_PreservesManualEntries()
        {
            //Arrange
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);
            var initialJson = JObject.Parse(_capturedJson);

            var manualUdi = Guid.NewGuid().ToString();
            var manualItem = new JObject
            {
                {"contentTypeKey", Guid.NewGuid() },
                {"linkText", "Manual Item" },
                {"linkUrl", new JArray(new JObject{ {"url", "/manual-item" } }) },
                {"udi", $"umb://element/{manualUdi}" }
            };

            var contentData = initialJson["contentData"] as JArray;
            contentData.Add(manualItem);

            var layout = initialJson["layout"]["Umbraco.BlockList"] as JArray;
            layout.Add(new JObject { { "contentUdi", $"umb://element/{manualUdi}" } });

            _mockSettingsNode.Setup(x => x.GetValue("tprHeaderMenu", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(initialJson.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            var finalJson = JObject.Parse(_capturedJson);
            var finalContentData = finalJson["contentData"] as JArray;
            var finalLayout = finalJson["layout"]["Umbraco.BlockList"] as JArray;

            Assert.Contains(finalContentData, item =>
            item["linkText"]?.ToString() == "Manual Item" &&
            item["udi"]?.ToString() == $"umb://element/{manualUdi}");

            Assert.Contains(finalLayout, item =>
            item["contentUdi"]?.ToString() == $"umb://element/{manualUdi}");

            Assert.Contains(finalContentData, item =>
            item["linkText"]?.ToString() == "link one"
            );
            Assert.Contains(finalContentData, item =>
            item["linkText"]?.ToString() == "link two"
            );
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_UpadetesContentBasedItems_WhilePreservingManualOnes()
        {
            //Arrange
            var initialJson = JObject.Parse(_capturedJson);

            var manualUdi = Guid.NewGuid().ToString();
            var manualItem = new JObject
            {
                {"contentTypeKey", Guid.NewGuid() },
                {"linkText", "Manual Item" },
                {"linkUrl", new JArray(new JObject{ {"url", "/manual-item" } }) },
                {"udi", $"umb://element/{manualUdi}" }
            };

            (initialJson["contentData"] as JArray).Add(manualItem);
            (initialJson["layout"]["Umbraco.BlockList"] as JArray).Add(new JObject { { "contentUdi", $"umb://element/{manualUdi}" } });

            _mockSettingsNode.Setup(x => x.GetValue("tprHeaderMenu", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
             .Returns(initialJson.ToString());

            var updatedParentItems = new List<TprHeaderMenuParentItem>
            {
                new TprHeaderMenuParentItem(_parentItems[0].ContentKey, "Updated Link One", "/updated-1"),
                new TprHeaderMenuParentItem(_parentItems[1].ContentKey, "Updated Link Two", "/updated-2")
            };

            _mockNavigationSerivce.Setup(x => x.GetMenuItems(It.IsAny<Guid>())).Returns(updatedParentItems);

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert 
            var finalJson = JObject.Parse(_capturedJson);
            var finalContentData = finalJson["contentData"] as JArray;
            var finalLayout = finalJson["layout"]["Umbraco.BlockList"] as JArray;

            Assert.Contains(finalContentData, item =>
            item["linkText"]?.ToString() == "Manual Item" &&
            item["udi"]?.ToString() == $"umb://element/{manualUdi}");

            Assert.Contains(finalLayout, item =>
            item["contentUdi"]?.ToString() == $"umb://element/{manualUdi}");

            Assert.Contains(finalContentData, item =>
            item["linkText"]?.ToString() == "Updated Link One"
            );
            Assert.Contains(finalContentData, item =>
            item["linkText"]?.ToString() == "Updated Link Two"
            );

            Assert.DoesNotContain(finalContentData, item =>
            item["linkText"]?.ToString() == "link one");
        }

        [Fact]
        public void GenerateTprHeaderMenuBlockList_PreservesManualChildItems()
        {
            //Arrange
            var initialJson = JObject.Parse(_capturedJson);

            var contentData = initialJson["contentData"] as JArray;
            var firstParentItem = contentData[0] as JObject;
            var exsistingChildBlock = firstParentItem["tprHeaderMenuChildItems"] as JObject;
            var childContentData = exsistingChildBlock["contentData"] as JArray;

            var manualChildUdi = Guid.NewGuid();
            var manualItem = new JObject
            {
                {"contentTypeKey", _mockChildMenuType.Object.Key.ToString() },
                {"linkText", "Manual Child Item" },
                {"linkUrl", new JArray(new JObject{ {"url", "/manual-item" } }) },
                {"udi", $"umb://element/{manualChildUdi}" }
            };
            childContentData.Add(manualItem);

            var childLayout = exsistingChildBlock["layout"]["Umbraco.BlockList"] as JArray;
            childLayout.Add(new JObject { { "contentUdi", $"umb://element/{manualChildUdi}" } });

            _mockSettingsNode.Setup(x => x.GetValue("tprHeaderMenu", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(initialJson.ToString());

            //Act
            _sut.GenerateTprHeaderMenuBlockList(homeNodeId, settingsNodeId);

            //Assert
            var finalJson = JObject.Parse(_capturedJson);
            var finalContentData = finalJson["contentData"] as JArray;
            var finalFirstParent = finalContentData.FirstOrDefault(x => x["linkText"]?.ToString() == "link one") as JObject;

            Assert.NotNull(finalFirstParent);
            var finalChildBlock = finalFirstParent["tprHeaderMenuChildItems"] as JObject;
            var finalChildContentData = finalChildBlock["contentData"] as JArray;

            Assert.Contains(finalChildContentData, item =>
            item["linkText"]?.ToString() == "Manual Child Item" &&
            item["udi"]?.ToString() == $"umb://element/{manualChildUdi}");

            Assert.Contains(finalChildContentData, item =>
            item["linkText"]?.ToString() == "child link one");
            Assert.Contains(finalChildContentData, item =>
           item["linkText"]?.ToString() == "child link two");

            Assert.Equal(3, finalChildContentData.Count);
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
