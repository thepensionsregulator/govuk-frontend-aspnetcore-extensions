using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
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
            Assert.That(targetElement.Object.Value<T>(PROPERTY_ALIAS), Is.EqualTo(propertyValue));
            Assert.That(targetElement.Object.Properties.SingleOrDefault(x => x.Alias == PROPERTY_ALIAS), Is.Not.Null);
            Assert.That(targetElement.Object.GetProperty(PROPERTY_ALIAS)?.GetValue(), Is.EqualTo(propertyValue));
            Assert.That(targetElement.Object.GetProperty(PROPERTY_ALIAS)?.PropertyType?.EditorAlias, Is.EqualTo(expectedPropertyEditorAlias));
        }

        [Test]
        public void SetupUmbracoBlockGridPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new BlockGridModel(Array.Empty<BlockGridItem>(), 1),
                (target, alias, value) => target.SetupUmbracoBlockGridPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockGrid
            );
        }

        [Test]
        public void SetupUmbracoBlockListPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new BlockListModel(Array.Empty<BlockListItem>()),
                (target, alias, value) => target.SetupUmbracoBlockListPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockList
            );
        }

        [Test]
        public void SetupUmbracoBooleanPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => true,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );
        }

        [Test]
        public void SetupUmbracoContentPickerPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object,
                (target, alias, value) => target.SetupUmbracoContentPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.ContentPicker
            );
        }

        [Test]
        public void SetupUmbracoIntegerPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => 5,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );
        }

        [Test]
        public void SetupUmbracoMultiUrlPickerPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => new Link { Name = "Example", Url = "https://example.org" },
                (target, alias, value) => target.SetupUmbracoMultiUrlPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.MultiUrlPicker
            );
        }

        [Test]
        public void SetupUmbracoRichTextPropertyValue_works()
        {
            TestSetupUmbracoTypedPropertyValue(
                () => "Some value",
                (target, alias, value) => target.SetupUmbracoRichTextPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.TinyMce
            );
        }

        [Test]
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
