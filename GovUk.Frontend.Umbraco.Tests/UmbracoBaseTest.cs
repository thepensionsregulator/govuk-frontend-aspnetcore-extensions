using Moq;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace GovUk.Frontend.Umbraco.Tests
{
    public abstract class UmbracoBaseTest
    {
        protected static BlockListModel CreateBlockListModel(IEnumerable<IPublishedProperty> contentProperties, IEnumerable<IPublishedProperty> settingsProperties)
        {
            var blockContent = new Mock<IPublishedElement>();
            blockContent.SetupGet(x => x.Properties).Returns(contentProperties);
            foreach (var property in contentProperties)
            {
                blockContent.Setup(x => x.GetProperty(property.Alias)).Returns(property);
            }

            var blockSettings = new Mock<IPublishedElement>();
            blockSettings.SetupGet(x => x.Properties).Returns(settingsProperties);
            foreach (var property in settingsProperties)
            {
                blockSettings.Setup(x => x.GetProperty(property.Alias)).Returns(property);
            }

            return new BlockListModel(new List<BlockListItem> {
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), blockContent.Object, Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), blockSettings.Object)
            });
        }

        protected static IPublishedProperty CreateProperty(string propertyAlias, PublishedPropertyType publishedPropertyType, object value)
        {
            var property = new Mock<IPublishedProperty>();
            property.SetupGet(x => x.Alias).Returns(propertyAlias);
            property.Setup(x => x.PropertyType).Returns(publishedPropertyType);
            property.Setup(x => x.HasValue(null, null)).Returns(true);
            property.Setup(x => x.GetValue(null, null)).Returns(value);
            return property.Object;
        }

        protected static PublishedPropertyType CreatePropertyType(int dataTypeId, string propertyEditorAlias, object configuration)
        {
            var propertyType = new Mock<IPropertyType>();
            propertyType.SetupGet(x => x.DataTypeId).Returns(1);
            propertyType.SetupGet(x => x.PropertyEditorAlias).Returns(propertyEditorAlias);

            var converter = new Mock<IPropertyValueConverter>();

            var propertyValueConverters = new PropertyValueConverterCollection(() => new IPropertyValueConverter[] { converter.Object });

            var contentTypeFactory = new Mock<IPublishedContentTypeFactory>();
            contentTypeFactory.Setup(x => x.GetDataType(dataTypeId)).Returns(new PublishedDataType(dataTypeId, propertyEditorAlias, new Lazy<object>(configuration)));
            var publishedPropertyType = new PublishedPropertyType(Mock.Of<IPublishedContentType>(), propertyType.Object, propertyValueConverters, Mock.Of<IPublishedModelFactory>(), contentTypeFactory.Object);

            converter.Setup(x => x.IsConverter(publishedPropertyType)).Returns(true);
            return publishedPropertyType;
        }
    }
}