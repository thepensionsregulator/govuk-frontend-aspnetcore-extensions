using Moq;
using Newtonsoft.Json;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models;


namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprGlobalNavigationTests
    {
        private Mock<IContent> _settingsNode;
        private string _propertyAlias;
        private string _linkTextAlias;
        private string _linkUrlAlias;
        private string _childItemsAlias;
        private TprGlobalNavigationService _sut;

        public TprGlobalNavigationTests()
        {

            _settingsNode = new Mock<IContent>();
            _propertyAlias = "tprHeaderMenu";
            _linkTextAlias = "linkText";
            _linkUrlAlias = "linkUrl";
            _childItemsAlias = "tprHeaderMenuChildItems";
            _sut = new TprGlobalNavigationService();

            var blockList = new BlockListJson();

            _settingsNode.Setup(x => x.GetValue(_propertyAlias, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(blockList.HeaderBlockListJson);
        }

        [Fact]
        public void GetMenuItems_ShouldReturnExpectedMenuItems()
        {
            //Act
            var result = _sut.GetMenuItems(_settingsNode.Object, _propertyAlias, _linkTextAlias, _linkUrlAlias, _childItemsAlias);

            //Assert 
            Assert.Multiple(() =>
            {
                Assert.Equal(2, result?.Count);
                Assert.Equal("Employers", result?[0].LinkText);
                Assert.Equal("/test", result?[0].LinkUrl);
                Assert.Equal("Business Advisors", result?[1].LinkText);
                Assert.Equal("umb://document/contentpage", result?[1].LinkUrl);


                Assert.Equal(result?[0].HeaderMenuChildItems?.Count, 2);
                Assert.Equal("Child One", result?[0].HeaderMenuChildItems?[0].LinkText);
                Assert.Equal("/child1", result?[0].HeaderMenuChildItems?[0].LinkUrl);
                Assert.Equal("Child Two", result?[0].HeaderMenuChildItems?[1].LinkText);
                Assert.Equal("/child2", result?[0].HeaderMenuChildItems?[1].LinkUrl);
            });
        }


        [Fact]
        public void GetMenuItems_WithInvlaidAliasAndJson_ThrowsExpectedExceptions()
        {
            //Arrange
            var invlaidAlias = "invalidValue";
            _settingsNode.Setup(x => x.GetValue(_propertyAlias, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).Returns("");

            //Assert
            Assert.Throws<ArgumentNullException>(() => _sut.GetMenuItems(_settingsNode.Object, invlaidAlias, _linkTextAlias, _linkUrlAlias, _childItemsAlias));
            Assert.Throws<JsonException>(() => _sut.GetMenuItems(_settingsNode.Object, _propertyAlias, _linkTextAlias, _linkUrlAlias, _childItemsAlias));
        }
    }

    public class BlockListJson
    {
        public string _headerBlockListJson = @"{
 
  ""contentData"": [
    {
      ""contentTypeKey"": ""7cddfcdc-5921-4dcf-a203-2ce06c447ebf"",
      ""linkText"": ""Employers"",
      ""linkUrl"": [
        {
          ""url"": ""/test""
        }
      ],
      ""tprHeaderMenuChildItems"": {
        
        ""contentData"": [
          {
            ""linkText"": ""Child One"",
            ""linkUrl"": [
              {
                ""url"": ""/child1""
              }
            ],
            
          },
          {
            ""linkText"": ""Child Two"",
            ""linkUrl"": [
              {
                ""url"": ""/child2""
              }
            ],
            
          }
        ],
        ""settingsData"": []
      },
     
    },
    {
  
      ""linkText"": ""Business Advisors"",
      ""linkUrl"": [
        {
          ""udi"": ""umb://document/contentpage""
        }
      ],
      ""tprHeaderMenuChildItems"": """",
      ""udi"": ""umb://element/133e6a93ed184ff792320f951dd801d0""
    }
  ],
  ""settingsData"": []
}";

        public string HeaderBlockListJson { get { return _headerBlockListJson; } }
    }
}
