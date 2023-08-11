using Moq;
using ThePensionsRegulator.Umbraco.BlockLists;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace ThePensionsRegulator.Umbraco.Tests
{
	public class PublishedElementExtensionsTests
	{
		[Test]
		public void IPublishedElement_is_converted_to_ModelsBuilder_model()
		{
			var contentType = new Mock<IPublishedContentType>();
			contentType.Setup(x => x.Alias).Returns("exampleModelsBuilderModel");

			var blockContent = new Mock<IPublishedElement>();
			blockContent.SetupGet(x => x.ContentType).Returns(contentType.Object);

			var result = blockContent.Object.AsPublishedElementModel<ExampleModelsBuilderModel>();

			Assert.That(result, Is.Not.Null);
		}
	}
}
