using GovUk.Frontend.Umbraco.BlockLists;
using Moq;
using NUnit.Framework;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.Tests.BlockLists
{
	public class PublishedContentModelExtensionsTests
	{
		[Test]
		public void If_PageHeading_block_has_text_PageHeadingOrName_returns_text()
		{
			// Arrange
			var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
				UmbracoBlockListFactory.CreateOverridableBlock(
					UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.PageHeading)
						.SetupUmbracoTextboxPropertyValue(PropertyAliases.PageHeading, "Custom")
						.Object
					)
				);

			var testContext = new UmbracoTestContext();
			testContext.CurrentPage.Setup(page => page.Name).Returns("Page name");
			testContext.CurrentPage.SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);
			var model = new ExampleModelsBuilderModel(testContext.CurrentPage.Object, Mock.Of<IPublishedValueFallback>());

			// Act
			var result = model.PageHeadingOrName();

			// Assert
			Assert.That(result, Is.EqualTo("Custom"));
		}

		[Test]
		public void If_PageHeading_block_has_no_text_PageHeadingOrName_returns_name()
		{
			// Arrange
			var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
				UmbracoBlockListFactory.CreateOverridableBlock(
					UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.PageHeading)
						.Object
					)
				);

			var testContext = new UmbracoTestContext();
			testContext.CurrentPage.Setup(page => page.Name).Returns("Page name");
			testContext.CurrentPage.SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);
			var model = new ExampleModelsBuilderModel(testContext.CurrentPage.Object, Mock.Of<IPublishedValueFallback>());

			// Act
			var result = model.PageHeadingOrName();

			// Assert
			Assert.That(result, Is.EqualTo("Page name"));
		}

		[Test]
		public void If_no_PageHeading_block_PageHeadingOrName_returns_name()
		{
			var testContext = new UmbracoTestContext();
			testContext.CurrentPage.Setup(page => page.Name).Returns("Page name");
			var model = new ExampleModelsBuilderModel(testContext.CurrentPage.Object, Mock.Of<IPublishedValueFallback>());

			// Act
			var result = model.PageHeadingOrName();

			// Assert
			Assert.That(result, Is.EqualTo("Page name"));
		}
	}
}
