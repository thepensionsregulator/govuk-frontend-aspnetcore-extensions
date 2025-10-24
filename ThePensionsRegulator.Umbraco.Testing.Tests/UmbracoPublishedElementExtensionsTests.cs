using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Testing.Tests
{
    public class UmbracoPublishedElementExtensionsTests
    {
        private const string PAGE_ALIAS = "myPage";
        private const string PROPERTY_ALIAS = "myProperty";

        public UmbracoPublishedElementExtensionsTests()
        {
            _ = new UmbracoTestContext(); // Sets up DI
        }

        private static void TestSetupUmbracoTypedPropertyValue<T>(Func<T> createPropertyValue, Action<IPublishedContent, string, T> act, string expectedPropertyEditorAlias)
        {
            // Arrange
            var targetElement = UmbracoContentFactory.CreateContent<IPublishedContent>(PAGE_ALIAS);
            T propertyValue = createPropertyValue();

            // Act
            act(targetElement.Object, PROPERTY_ALIAS, propertyValue);

            // Assert
            Assert.Equal(propertyValue, targetElement.Object.Value<T>(PROPERTY_ALIAS));
            Assert.NotNull(targetElement.Object.Properties.SingleOrDefault(x => x.Alias == PROPERTY_ALIAS)); // returns PublishedElementPropertyBase
            Assert.Equal(propertyValue, targetElement.Object.GetProperty(PROPERTY_ALIAS)?.GetValue());
            Assert.Equal(expectedPropertyEditorAlias, targetElement.Object.GetProperty(PROPERTY_ALIAS)?.PropertyType?.EditorAlias);
        }

        [Fact]
        public void SetupUmbracoBlockGridPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new BlockGridModel(Array.Empty<BlockGridItem>(), 1),
                (target, alias, value) => target.SetupUmbracoBlockGridPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockGrid
            );
        }

        [Fact]
        public void SetupUmbracoBlockListPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new BlockListModel(Array.Empty<BlockListItem>()),
                (target, alias, value) => target.SetupUmbracoBlockListPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockList
            );
        }

        [Fact]
        public void SetupUmbracoBooleanPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => true,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );
        }

        [Fact]
        public void SetupUmbracoContentPickerPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object,
                (target, alias, value) => target.SetupUmbracoContentPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.ContentPicker
            );
        }

        [Fact]
        public void SetupUmbracoIntegerPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => 5,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );
        }

        [Fact]
        public void SetupUmbracoMultiUrlPickerPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new Link { Name = "Example", Url = "https://example.org" },
                (target, alias, value) => target.SetupUmbracoMultiUrlPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.MultiUrlPicker
            );
        }

        [Fact]
        public void SetupUmbracoRichTextPropertyValue_works_with_HtmlEncodedString()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new HtmlEncodedString("<p>Some value</p>"),
                (target, alias, value) => target.SetupUmbracoRichTextPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.RichText
            );
        }

        /// <summary>
        /// Native format of rich text values is HtmlEncodedString which is covered by another test, but you can also use .Value<string>().
        /// </summary>
        [Fact]
        public void SetupUmbracoRichTextPropertyValue_works_with_string()
        {
            // Arrange
            var targetElement = UmbracoContentFactory.CreateContent<IPublishedContent>(PAGE_ALIAS);
            var propertyValue = "Some value";

            // Act
            targetElement.Object.SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, propertyValue);

            // Assert
            Assert.Equal(propertyValue, targetElement.Object.Value<string>(PROPERTY_ALIAS));
        }

        [Fact]
        public void SetupUmbracoTextboxPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => "Some value",
                (target, alias, value) => target.SetupUmbracoTextboxPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.TextBox
            );
        }
    }
}
