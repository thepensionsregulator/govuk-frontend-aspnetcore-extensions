using Moq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Core = Umbraco.Cms.Core;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// Mock Umbraco properties and property types
    /// </summary>
    public static class UmbracoPropertyFactory
    {
        private const int BLOCKLIST_DATA_TYPE_ID = 1;
        private const int TEXTBOX_DATA_TYPE_ID = 2;
        private const int BOOLEAN_DATA_TYPE_ID = 3;
        private const int MULTI_URL_PICKER_DATA_TYPE_ID = 4;
        private const int INTEGER_DATA_TYPE_ID = 5;

        /// <summary>
        /// Mock an Umbraco property and set its value.
        /// </summary>
        /// <param name="propertyAlias">The alias of the Umbraco property to mock.</param>
        /// <param name="publishedPropertyType">The Umbraco property type, typically mocked using <see cref="CreatePropertyType"/>.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The mocked Umbraco property.</returns>
        public static IPublishedProperty CreateProperty(string propertyAlias, PublishedPropertyType? publishedPropertyType, object? value)
        {
            var property = new Mock<IPublishedProperty>();
            property.SetupGet(x => x.Alias).Returns(propertyAlias);
            if (publishedPropertyType != null)
            {
                property.Setup(x => x.PropertyType).Returns(publishedPropertyType);
            }
            property.Setup(x => x.HasValue(null, null)).Returns(value != null);
            property.Setup(x => x.GetValue(null, null)).Returns(value);
            return property.Object;
        }

        /// <summary>
        /// Mock an Umbraco property type.
        /// </summary>
        /// <param name="dataTypeId">The id of the data type. This does not need to match the id in a real instance of Umbraco, but it must be unique to this property type within your test.</param>
        /// <param name="propertyEditorAlias">The alias of the Umbraco property editor used by the data type.</param>
        /// <param name="configuration">An internal Umbraco configuration object specific to the property type.</param>
        /// <returns>The mocked Umbraco property type.</returns>
        public static PublishedPropertyType CreatePropertyType(int dataTypeId, string propertyEditorAlias, object? configuration)
        {
            var propertyType = new Mock<IPropertyType>();
            propertyType.SetupGet(x => x.DataTypeId).Returns(dataTypeId);
            propertyType.SetupGet(x => x.PropertyEditorAlias).Returns(propertyEditorAlias);

            var converter = new Mock<IPropertyValueConverter>();

            var propertyValueConverters = new PropertyValueConverterCollection(() => new IPropertyValueConverter[] { converter.Object });

            var contentTypeFactory = new Mock<IPublishedContentTypeFactory>();
            contentTypeFactory.Setup(x => x.GetDataType(dataTypeId)).Returns(new PublishedDataType(dataTypeId, propertyEditorAlias, new Lazy<object?>(configuration)));
            var publishedPropertyType = new PublishedPropertyType(Mock.Of<IPublishedContentType>(), propertyType.Object, propertyValueConverters, Mock.Of<IPublishedModelFactory>(), contentTypeFactory.Object);

            converter.Setup(x => x.IsConverter(publishedPropertyType)).Returns(true);
            return publishedPropertyType;
        }

        /// <summary>
        /// Mock an Umbraco property using a textbox data type, and set its value.
        /// </summary>
        /// <param name="propertyAlias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The mocked Umbraco property.</returns>
        public static IPublishedProperty CreateTextboxProperty(string propertyAlias, string? value)
        {
            return CreateProperty(propertyAlias, CreatePropertyType(TEXTBOX_DATA_TYPE_ID, Core.Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration()), value);
        }


        /// <summary>
        /// Mock an Umbraco property using a integer data type, and set its value.
        /// </summary>
        /// <param name="propertyAlias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The mocked Umbraco property.</returns>
        public static IPublishedProperty CreateIntegerProperty(string propertyAlias, int? value)
        {
            return CreateProperty(propertyAlias, CreatePropertyType(INTEGER_DATA_TYPE_ID, Core.Constants.PropertyEditors.Aliases.Integer, null), value);
        }

        /// <summary>
        /// Mock an Umbraco property using a block list data type, and set its value.
        /// </summary>
        /// <param name="propertyAlias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The block list to assign to the mocked Umbraco property.</param>
        /// <returns>The mocked Umbraco property.</returns>
        public static IPublishedProperty CreateBlockListProperty(string propertyAlias, IEnumerable<BlockListItem>? value)
        {
            return CreateProperty(propertyAlias, CreatePropertyType(BLOCKLIST_DATA_TYPE_ID, Core.Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration()), value);
        }

        /// <summary>
        /// Mock an Umbraco property using a boolean data type, and set its value.
        /// </summary>
        /// <param name="propertyAlias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The mocked Umbraco property.</returns>
        public static IPublishedProperty CreateBooleanProperty(string propertyAlias, bool? value)
        {
            return CreateProperty(propertyAlias, CreatePropertyType(BOOLEAN_DATA_TYPE_ID, Core.Constants.PropertyEditors.Aliases.Boolean, new TrueFalseConfiguration()), value);
        }

        /// <summary>
        /// Mock an Umbraco property using a multi-URL picker data type configured to pick a maximum of one URL, and set its value.
        /// </summary>
        /// <param name="propertyAlias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The mocked Umbraco property.</returns>
        public static IPublishedProperty CreateMultiUrlPickerProperty(string propertyAlias, Link? value)
        {
            return CreateProperty(propertyAlias, CreatePropertyType(MULTI_URL_PICKER_DATA_TYPE_ID, Core.Constants.PropertyEditors.Aliases.MultiUrlPicker, new MultiUrlPickerConfiguration()), value);
        }
    }
}
