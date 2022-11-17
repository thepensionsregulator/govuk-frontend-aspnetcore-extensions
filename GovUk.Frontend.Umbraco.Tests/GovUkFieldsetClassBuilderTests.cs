using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class GovUkFieldsetClassBuilderTests : UmbracoBaseTest
    {
        private const string VIEWMODEL_PROPERTY_NAME = "Field1";

        [Test]
        public void Non_fieldset_returns_empty_string()
        {
            var legendIsPageHeading = true;
            var fieldsetBlock = CreateUmbracoTestContent(ElementTypeAliases.GridRow, ElementTypeAliases.ErrorMessage, true, legendIsPageHeading);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var result = GovUkFieldsetClassBuilder.BuildFieldsetErrorClass(fieldsetBlock, modelState, legendIsPageHeading);

            Assert.True(string.IsNullOrEmpty(result));
        }

        [Test]
        public void Render_error_classes_false_returns_empty_string()
        {
            var legendIsPageHeading = true;
            var fieldsetBlock = CreateUmbracoTestContent(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, false, legendIsPageHeading);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var result = GovUkFieldsetClassBuilder.BuildFieldsetErrorClass(fieldsetBlock, modelState, legendIsPageHeading);

            Assert.True(string.IsNullOrEmpty(result));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        public void If_LegendIsPageHeading_does_not_match_returns_empty_string(bool legendIsPageHeading, bool legendIsPageHeadingMustMatchThis)
        {
            var fieldsetBlock = CreateUmbracoTestContent(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, true, legendIsPageHeading);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var result = GovUkFieldsetClassBuilder.BuildFieldsetErrorClass(fieldsetBlock, modelState, legendIsPageHeadingMustMatchThis);

            Assert.AreEqual(legendIsPageHeading == legendIsPageHeadingMustMatchThis, !string.IsNullOrEmpty(result));
        }

        [Test]
        public void No_ModelState_error_returns_empty_string()
        {
            var legendIsPageHeading = true;
            var fieldsetBlock = CreateUmbracoTestContent(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, true, legendIsPageHeading);

            var modelState = new ModelStateDictionary();

            var result = GovUkFieldsetClassBuilder.BuildFieldsetErrorClass(fieldsetBlock, modelState, legendIsPageHeading);

            Assert.True(string.IsNullOrEmpty(result));
        }

        [Test]
        public void Block_other_than_ErrorMessage_bound_to_invalid_property_returns_empty_string()
        {
            var legendIsPageHeading = true;
            var fieldsetBlock = CreateUmbracoTestContent(ElementTypeAliases.Fieldset, ElementTypeAliases.TextInput, true, legendIsPageHeading);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var result = GovUkFieldsetClassBuilder.BuildFieldsetErrorClass(fieldsetBlock, modelState, legendIsPageHeading);

            Assert.True(string.IsNullOrEmpty(result));
        }

        [Test]
        public void ErrorMessage_block_bound_to_invalid_property_returns_classes()
        {
            var legendIsPageHeading = true;
            var fieldsetBlock = CreateUmbracoTestContent(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, true, legendIsPageHeading);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var result = GovUkFieldsetClassBuilder.BuildFieldsetErrorClass(fieldsetBlock, modelState, legendIsPageHeading);

            Assert.False(string.IsNullOrEmpty(result));
        }

        private static OverridableBlockListItem CreateUmbracoTestContent(string aliasOfParentBlock, string aliasOfChildBlock, bool renderErrorClasses, bool legendIsPageHeading)
        {
            var blockListPropertyType = CreatePropertyType(1, Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration());
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var fieldsetContentType = new Mock<IPublishedContentType>();
            fieldsetContentType.Setup(x => x.Alias).Returns(aliasOfParentBlock);

            var fieldsetContent = new Mock<IOverridablePublishedElement>();
            fieldsetContent.Setup(x => x.ContentType).Returns(fieldsetContentType.Object);

            var fieldsetSettings = new Mock<IOverridablePublishedElement>();
            fieldsetSettings.Setup(x => x.GetProperty(PropertyAliases.FieldsetRenderErrorClasses)).Returns(CreateProperty(PropertyAliases.FieldsetRenderErrorClasses, textBoxPropertyType, renderErrorClasses));
            fieldsetSettings.Setup(x => x.GetProperty(PropertyAliases.FieldsetLegendIsPageHeading)).Returns(CreateProperty(PropertyAliases.FieldsetLegendIsPageHeading, textBoxPropertyType, legendIsPageHeading));

            var errorMessageContentType = new Mock<IPublishedContentType>();
            errorMessageContentType.Setup(x => x.Alias).Returns(aliasOfChildBlock);

            var errorMessageContent = new Mock<IOverridablePublishedElement>();
            errorMessageContent.Setup(x => x.ContentType).Returns(errorMessageContentType.Object);

            var errorMessageSettings = new Mock<IOverridablePublishedElement>();
            errorMessageSettings.Setup(x => x.GetProperty(PropertyAliases.ModelProperty)).Returns(CreateProperty(PropertyAliases.ModelProperty, textBoxPropertyType, VIEWMODEL_PROPERTY_NAME));

            var errorMessageBlock = new OverridableBlockListItem(
                new BlockListItem(
                    Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), errorMessageContent.Object,
                    Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), errorMessageSettings.Object
                    ),
                x => (IOverridablePublishedElement)x
                );

            var fieldsetBlocks = new OverridableBlockListModel(new[] { errorMessageBlock }, null, x => (IOverridablePublishedElement)x);
            var fieldsetContentProperties = new[] { CreateProperty(PropertyAliases.FieldsetBlocks, blockListPropertyType, fieldsetBlocks) };
            fieldsetContent.SetupGet(x => x.Properties).Returns(fieldsetContentProperties);
            fieldsetContent.Setup(x => x.GetProperty(PropertyAliases.FieldsetBlocks)).Returns(fieldsetContentProperties[0]);
            fieldsetContent.Setup(x => x.Value<OverridableBlockListModel>(PropertyAliases.FieldsetBlocks, null, null, It.IsAny<Fallback>(), null)).Returns(fieldsetBlocks);

            var fieldsetBlock = new OverridableBlockListItem(
                new BlockListItem(
                    Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), fieldsetContent.Object,
                    Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), fieldsetSettings.Object
                    ),
                x => (IOverridablePublishedElement)x
            );
            return fieldsetBlock;
        }
    }
}
