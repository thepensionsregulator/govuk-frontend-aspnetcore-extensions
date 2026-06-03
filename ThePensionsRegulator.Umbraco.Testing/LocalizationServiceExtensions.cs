using Moq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Umbraco.Testing
{
    public static class LocalizationServiceExtensions
    {
        public static Mock<ILocalizationService> SetupUmbracoDictionaryItem(this Mock<ILocalizationService> localizationService, string key, string value, int languageId = 2)
        {
            var dictionaryTranslation = new Mock<IDictionaryTranslation>();
            dictionaryTranslation
                .Setup(x => x.LanguageId)
                .Returns(languageId);

            dictionaryTranslation
                .Setup(x => x.Value)
                .Returns(value);

            var dictionaryItem = new Mock<IDictionaryItem>();
            dictionaryItem
                .Setup(x => x.Translations)
                .Returns(new[] { dictionaryTranslation.Object });

            localizationService
               .Setup(x => x.GetDictionaryItemByKey(key))
               .Returns(dictionaryItem.Object);

            return localizationService;
        }

        public static Mock<ILocalizationService> SetupUmbracoDictionaryItem(this ILocalizationService localizationService, string key, string value, int languageId = 2)
        {
            return Mock.Get(localizationService).SetupUmbracoDictionaryItem(key, value, languageId);
        }
    }
}
