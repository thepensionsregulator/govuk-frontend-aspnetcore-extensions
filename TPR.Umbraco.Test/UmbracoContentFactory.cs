using Moq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace TPR.Umbraco.Test
{
    public static class UmbracoContentFactory
    {
        /// <summary>
        /// Mock an <see cref="IPublishedElement"/>
        /// </summary>
        /// <typeparam name="T">Use <see cref="IPublishedContent"/> to create an Umbraco page, or <see cref="IPublishedElement"/> for a block in an Umbraco block list.</typeparam>
        /// <param name="contentTypeAlias">The alias of the Umbraco content type assigned to the <see cref="IPublishedElement.ContentType"/> property of the mocked object.</param>
        /// <returns>The mocked Umbraco content.</returns>
        public static Mock<T> CreateContent<T>(string? contentTypeAlias = null) where T : class, IPublishedElement
        {
            var publishedContent = new Mock<T>();

            if (!string.IsNullOrEmpty(contentTypeAlias))
            {
                var contentType = new Mock<IPublishedContentType>();
                contentType.Setup(x => x.Alias).Returns(contentTypeAlias);
                publishedContent.Setup(x => x.ContentType).Returns(contentType.Object);
            }

            publishedContent.Setup(x => x.Properties).Returns(new List<IPublishedProperty>());
            return publishedContent;
        }
    }
}
