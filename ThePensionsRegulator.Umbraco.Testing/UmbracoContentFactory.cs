using Moq;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Testing
{
    public static class UmbracoContentFactory
    {
        /// <summary>
        /// Mock an <see cref="IPublishedElement"/>
        /// </summary>
        /// <typeparam name="T">Use <see cref="IPublishedContent"/> to create an Umbraco page, or <see cref="IPublishedElement"/> for a block in an Umbraco block list.</typeparam>
        /// <param name="contentTypeAlias">The alias of the Umbraco content type assigned to the <see cref="IPublishedElement.ContentType"/> property of the mocked object.</param>
        /// <param name="name">When using <see cref="IPublishedContent"/>, the name of the Umbraco content assigned to the <see cref="IPublishedContent.Name"/> property of the mocked object.</param>
        /// <returns>The mocked Umbraco content.</returns>
        public static Mock<T> CreateContent<T>(string? contentTypeAlias = null, string? name = null) where T : class, IPublishedElement
        {
            var publishedElement = new Mock<T>();

            if (!string.IsNullOrEmpty(contentTypeAlias))
            {
                var contentType = new Mock<IPublishedContentType>();
                contentType.Setup(x => x.Alias).Returns(contentTypeAlias);
                contentType.Setup(x => x.ItemType).Returns(PublishedItemType.Content);
                publishedElement.Setup(x => x.ContentType).Returns(contentType.Object);
            }

            publishedElement.Setup(x => x.Key).Returns(Guid.NewGuid());
            publishedElement.Setup(x => x.Properties).Returns(new List<IPublishedProperty>());

            var publishedContent = publishedElement as Mock<IPublishedContent>;
            if (publishedContent is not null)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    publishedContent.Setup(x => x.Name).Returns(name);
                }
            }

            var overridablePublishedElement = publishedElement as Mock<IOverridablePublishedElement>;
            if (overridablePublishedElement is not null)
            {
                overridablePublishedElement.Setup(element => element.OverrideValue(It.IsAny<string>(), It.IsAny<object>()))
                    .Callback<string, object>((alias, overriddenValue) => SetupOverriddenValue(alias, overriddenValue, overridablePublishedElement));
            }

            return publishedElement;
        }

        private static void SetupOverriddenValue<T>(string alias, T overriddenValue, Mock<IOverridablePublishedElement> overridablePublishedElement)
        {
            overridablePublishedElement.Setup(element => element.Value<T>(It.Is<string>(x => string.Equals(alias, x, StringComparison.OrdinalIgnoreCase)), null, null, default, default)).Returns(overriddenValue);
            overridablePublishedElement.Setup(element => element.Value<T>(It.IsAny<IPublishedValueFallback>(), It.Is<string>(x => string.Equals(alias, x, StringComparison.OrdinalIgnoreCase)), null, null, default, default)).Returns(overriddenValue);
        }
    }
}
