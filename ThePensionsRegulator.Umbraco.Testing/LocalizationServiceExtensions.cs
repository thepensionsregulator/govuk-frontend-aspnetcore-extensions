using Moq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Umbraco.Testing
{
    public static class LocalizationServiceExtensions
    {
        public static Mock<IDictionaryItemService> SetupUmbracoDictionaryItem(this Mock<IDictionaryItemService> dictionaryItemService, string key, string value, string languageIsoCode)
        {
            var dictionaryTranslation = new Mock<IDictionaryTranslation>();
            dictionaryTranslation
                .Setup(x => x.LanguageIsoCode)
                .Returns(languageIsoCode);

            dictionaryTranslation
                .Setup(x => x.Value)
                .Returns(value);

            var dictionaryItem = new Mock<IDictionaryItem>();
            dictionaryItem
                .Setup(x => x.Translations)
                .Returns([dictionaryTranslation.Object]);


            dictionaryItemService
               .Setup(x => x.GetAsync(key))
               .ReturnsAsync(dictionaryItem.Object);

            return dictionaryItemService;
        }

        public static Mock<IDictionaryItemService> SetupUmbracoDictionaryItem(this IDictionaryItemService dictionaryItemService, string key, string value, string languageIsoCode)
        {
            return Mock.Get(dictionaryItemService).SetupUmbracoDictionaryItem(key, value, languageIsoCode);
        }
    }
}
