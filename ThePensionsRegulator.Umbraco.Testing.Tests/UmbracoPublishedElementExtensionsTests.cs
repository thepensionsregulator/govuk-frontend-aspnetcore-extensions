using ThePensionsRegulator.Umbraco.Core;
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
        private UmbracoTestContext _testContext = new();

        private void TestSetupUmbracoTypedPropertyValue<TContentType, TValue>(Func<TValue> createPropertyValue, Action<TContentType, string, TValue> act, string expectedPropertyEditorAlias)
            where TContentType : class, IPublishedElement
        {
            // Arrange
            var targetElement = UmbracoContentFactory.CreateContent<TContentType>(PAGE_ALIAS);
            TValue propertyValue = createPropertyValue();

            // Act
            act(targetElement.Object, PROPERTY_ALIAS, propertyValue);

            // Assert
            Assert.NotNull(targetElement.Object.Properties.SingleOrDefault(x => x.Alias == PROPERTY_ALIAS)); // returns PublishedElementPropertyBase
            Assert.Equal(propertyValue, targetElement.Object.GetProperty(PROPERTY_ALIAS)?.GetValue());
            Assert.Equal(expectedPropertyEditorAlias, targetElement.Object.GetProperty(PROPERTY_ALIAS)?.PropertyType?.EditorAlias);

            // Assert value via Umbraco's IPublishedElement extension methods (even when TContentType is IOverridablePublishedElement, because C# uses the method constraint IPublishedElement instead)
            Assert.Equal(propertyValue, targetElement.Object.Value<TValue>(PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement.Object.Value<TValue>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));

            // Assert value via IOverridablePublishedElement extension methods (direct call, not extension method, as C# would use the method constraint IPublishedElement instead)
            if (targetElement.Object is IOverridablePublishedElement overridable)
            {
                Assert.Equal(propertyValue, overridable.Value<TValue>(PROPERTY_ALIAS));
                Assert.Equal(propertyValue, overridable.Value<TValue>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
            }
        }

        // Overload that additionally asserts via TNullableValue, which must be supplied as a concrete type argument (e.g. int?) so it
        // produces a genuine Nullable<T> generic instantiation at the IL level rather than the annotation-only TValue? inside a generic method.
        private void TestSetupUmbracoTypedPropertyValue<TContentType, TValue, TNullableValue>(Func<TValue> createPropertyValue, Action<TContentType, string, TValue> act, string expectedPropertyEditorAlias)
            where TContentType : class, IPublishedElement
        {
            TestSetupUmbracoTypedPropertyValue(createPropertyValue, act, expectedPropertyEditorAlias);

            // Arrange
            var targetElement = UmbracoContentFactory.CreateContent<TContentType>(PAGE_ALIAS);
            TValue propertyValue = createPropertyValue();

            // Act
            act(targetElement.Object, PROPERTY_ALIAS, propertyValue);

            // Assert value via Umbraco's IPublishedElement extension methods (covers IPublishedContent and the extension-method path for IOverridablePublishedElement)
            Assert.Equal((object?)propertyValue, (object?)targetElement.Object.Value<TNullableValue>(PROPERTY_ALIAS)); // cast to object forces compiler to use Assert.Equal(object?, object?) overload
            Assert.Equal((object?)propertyValue, (object?)targetElement.Object.Value<TNullableValue>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));

            // Assert value via IOverridablePublishedElement direct methods (direct call, not extension method, as C# would use the method constraint IPublishedElement instead)
            if (targetElement.Object is IOverridablePublishedElement overridable)
            {
                Assert.Equal((object?)propertyValue, (object?)overridable.Value<TNullableValue>(PROPERTY_ALIAS)); // cast to object forces compiler to use Assert.Equal(object?, object?) overload
                Assert.Equal((object?)propertyValue, (object?)overridable.Value<TNullableValue>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
            }
        }

        [Fact]
        public void SetupUmbracoBlockGridPropertyValue_works()
        {
            // IPublishedContent: set BlockGridModel, read BlockGridModel (reference type, so BlockGridModel is the same IL type as BlockGridModel?)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, BlockGridModel>(
                () => new BlockGridModel(Array.Empty<BlockGridItem>(), 1),
                (target, alias, value) => target.SetupUmbracoBlockGridPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockGrid
            );

            // IOverridablePublishedElement: set BlockGridModel, read BlockGridModel
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, BlockGridModel>(
                () => new BlockGridModel(Array.Empty<BlockGridItem>(), 1),
                (target, alias, value) => target.SetupUmbracoBlockGridPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockGrid
            );

            // IPublishedContent: set BlockGridModel?, read BlockGridModel? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, BlockGridModel?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoBlockGridPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockGrid
            );

            // IOverridablePublishedElement: set BlockGridModel?, read BlockGridModel? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, BlockGridModel?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoBlockGridPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockGrid
            );
        }

        [Fact]
        public void SetupUmbracoBlockListPropertyValue_works()
        {
            // IPublishedContent: set BlockListModel, read BlockListModel (reference type, so BlockListModel is the same IL type as BlockListModel?)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, BlockListModel>(
                () => new BlockListModel(Array.Empty<BlockListItem>()),
                (target, alias, value) => target.SetupUmbracoBlockListPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockList
            );

            // IOverridablePublishedElement: set BlockListModel, read BlockListModel
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, BlockListModel>(
                () => new BlockListModel(Array.Empty<BlockListItem>()),
                (target, alias, value) => target.SetupUmbracoBlockListPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockList
            );

            // IPublishedContent: set BlockListModel?, read BlockListModel? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, BlockListModel?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoBlockListPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockList
            );

            // IOverridablePublishedElement: set BlockListModel?, read BlockListModel? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, BlockListModel?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoBlockListPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.BlockList
            );
        }

        [Fact]
        public void SetupUmbracoBooleanPropertyValue_works()
        {
            // IPublishedContent: set bool, read bool and bool?
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, bool, bool?>(
                () => true,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );

            // IPublishedContent: set bool?, read bool? and bool (non-null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, bool?, bool>(
                () => true,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );

            // IPublishedContent: set bool?, read bool? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, bool?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );

            // IOverridablePublishedElement: set bool, read bool and bool?
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, bool, bool?>(
                () => true,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );

            // IOverridablePublishedElement: set bool?, read bool? and bool (non-null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, bool?, bool>(
                () => true,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );

            // IOverridablePublishedElement: set bool?, read bool? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, bool?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoBooleanPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Boolean
            );
        }

        [Fact]
        public void SetupUmbracoContentPickerPropertyValue_works()
        {
            // IPublishedContent: set IPublishedContent, read IPublishedContent (reference type, so IPublishedContent is the same IL type as IPublishedContent?)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, IPublishedContent>(
                () => UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object,
                (target, alias, value) => target.SetupUmbracoContentPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.ContentPicker
            );

            // IOverridablePublishedElement: set IPublishedContent, read IPublishedContent
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, IPublishedContent>(
                () => UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object,
                (target, alias, value) => target.SetupUmbracoContentPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.ContentPicker
            );

            // IPublishedContent: set IPublishedContent?, read IPublishedContent? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, IPublishedContent?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoContentPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.ContentPicker
            );

            // IOverridablePublishedElement: set IPublishedContent?, read IPublishedContent? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, IPublishedContent?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoContentPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.ContentPicker
            );
        }

        [Fact]
        public void SetupUmbracoIntegerPropertyValue_works()
        {
            // IPublishedContent: set int, read int and int?
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, int, int?>(
                () => 5,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );

            // IPublishedContent: set int?, read int? and int (non-null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, int?, int>(
                () => 5,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );

            // IPublishedContent: set int?, read int? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, int?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );

            // IOverridablePublishedElement: set int, read int and int?
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, int, int?>(
                () => 5,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );

            // IOverridablePublishedElement: set int?, read int? and int (non-null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, int?, int>(
                () => 5,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );

            // IOverridablePublishedElement: set int?, read int? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, int?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoIntegerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.Integer
            );
        }

        [Fact]
        public void SetupUmbracoMultiUrlPickerPropertyValue_works()
        {
            // IPublishedContent: set Link, read Link (reference type, so Link is the same IL type as Link?)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, Link>(
                () => new Link { Name = "Example", Url = "https://example.org" },
                (target, alias, value) => target.SetupUmbracoMultiUrlPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.MultiUrlPicker
            );

            // IOverridablePublishedElement: set Link, read Link
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, Link>(
                () => new Link { Name = "Example", Url = "https://example.org" },
                (target, alias, value) => target.SetupUmbracoMultiUrlPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.MultiUrlPicker
            );

            // IPublishedContent: set Link?, read Link? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, Link?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoMultiUrlPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.MultiUrlPicker
            );

            // IOverridablePublishedElement: set Link?, read Link? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, Link?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoMultiUrlPickerPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.MultiUrlPicker
            );
        }

        [Fact]
        public void SetupUmbracoRichTextPropertyValue_works_with_HtmlEncodedString()
        {
            // IPublishedContent: set HtmlEncodedString, read HtmlEncodedString (reference type, so HtmlEncodedString is the same IL type as HtmlEncodedString?)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, HtmlEncodedString>(
                () => new HtmlEncodedString("<p>Some value</p>"),
                (target, alias, value) => target.SetupUmbracoRichTextPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.RichText
            );

            // IOverridablePublishedElement: set HtmlEncodedString, read HtmlEncodedString
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, HtmlEncodedString>(
                () => new HtmlEncodedString("<p>Some value</p>"),
                (target, alias, value) => target.SetupUmbracoRichTextPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.RichText
            );

            // IPublishedContent: set HtmlEncodedString?, read HtmlEncodedString? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, HtmlEncodedString?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoRichTextPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.RichText
            );

            // IOverridablePublishedElement: set HtmlEncodedString?, read HtmlEncodedString? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, HtmlEncodedString?>(
                () => null,
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
            var targetElement1 = UmbracoContentFactory.CreateContent<IPublishedContent>(PAGE_ALIAS);
            var targetElement2 = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(PAGE_ALIAS);
            var targetElement3 = UmbracoContentFactory.CreateContent<IPublishedContent>(PAGE_ALIAS);
            var targetElement4 = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(PAGE_ALIAS);
            var propertyValue = "Some value";

            // Act
            targetElement1.Object.SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, propertyValue);
            targetElement2.Object.SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, propertyValue);
            targetElement3.Object.SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, (string?)null);
            targetElement4.Object.SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, (string?)null);

            // Assert
            Assert.Equal(propertyValue, targetElement1.Object.Value<string>(PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement1.Object.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement1.Object.Value<string?>(PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement1.Object.Value<string?>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));

            Assert.Equal(propertyValue, targetElement2.Object.Value<string>(PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement2.Object.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement2.Object.Value<string?>(PROPERTY_ALIAS));
            Assert.Equal(propertyValue, targetElement2.Object.Value<string?>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));

            Assert.Null(targetElement3.Object.Value<string>(PROPERTY_ALIAS));
            Assert.Null(targetElement3.Object.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
            Assert.Null(targetElement3.Object.Value<string?>(PROPERTY_ALIAS));
            Assert.Null(targetElement3.Object.Value<string?>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));

            Assert.Null(targetElement4.Object.Value<string>(PROPERTY_ALIAS));
            Assert.Null(targetElement4.Object.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
            Assert.Null(targetElement4.Object.Value<string?>(PROPERTY_ALIAS));
            Assert.Null(targetElement4.Object.Value<string?>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
        }

        [Fact]
        public void SetupUmbracoTextboxPropertyValue_works()
        {
            // IPublishedContent: set string, read string (reference type, so string is the same IL type as string?)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, string>(
                () => "Some value",
                (target, alias, value) => target.SetupUmbracoTextboxPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.TextBox
            );

            // IOverridablePublishedElement: set string, read string
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, string>(
                () => "Some value",
                (target, alias, value) => target.SetupUmbracoTextboxPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.TextBox
            );

            // IPublishedContent: set string?, read string? (null)
            TestSetupUmbracoTypedPropertyValue<IPublishedContent, string?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoTextboxPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.TextBox
            );

            // IOverridablePublishedElement: set string?, read string? (null)
            TestSetupUmbracoTypedPropertyValue<IOverridablePublishedElement, string?>(
                () => null,
                (target, alias, value) => target.SetupUmbracoTextboxPropertyValue(alias, value),
                Constants.PropertyEditors.Aliases.TextBox
            );
        }
    }
}
