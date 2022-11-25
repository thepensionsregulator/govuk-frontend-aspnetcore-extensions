﻿using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class GovUkFieldsetErrorFinderTests : UmbracoBaseTest
    {
        private const string VIEWMODEL_PROPERTY_NAME = "Field1";

        [Test]
        public void Non_fieldset_returns_no_results()
        {
            var fieldsetBlock = CreateUmbracoTestContentForClasses(ElementTypeAliases.GridRow, ElementTypeAliases.ErrorMessage, true);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var results = GovUkFieldsetErrorFinder.FindFieldsetErrors(fieldsetBlock, modelState);

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Render_error_classes_false_no_results()
        {
            var fieldsetBlock = CreateUmbracoTestContentForClasses(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, false);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var results = GovUkFieldsetErrorFinder.FindFieldsetErrors(fieldsetBlock, modelState);

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void No_ModelState_error_returns_no_results()
        {
            var fieldsetBlock = CreateUmbracoTestContentForClasses(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, true);

            var modelState = new ModelStateDictionary();

            var results = GovUkFieldsetErrorFinder.FindFieldsetErrors(fieldsetBlock, modelState);

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void Block_other_than_ErrorMessage_bound_to_invalid_property_returns_no_results()
        {
            var fieldsetBlock = CreateUmbracoTestContentForClasses(ElementTypeAliases.Fieldset, ElementTypeAliases.TextInput, true);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var results = GovUkFieldsetErrorFinder.FindFieldsetErrors(fieldsetBlock, modelState);

            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void ErrorMessage_block_bound_to_invalid_property_returns_ErrorMessage_block()
        {
            var fieldsetBlock = CreateUmbracoTestContentForClasses(ElementTypeAliases.Fieldset, ElementTypeAliases.ErrorMessage, true);

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(VIEWMODEL_PROPERTY_NAME, "Any error");

            var results = GovUkFieldsetErrorFinder.FindFieldsetErrors(fieldsetBlock, modelState);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(ElementTypeAliases.ErrorMessage, results.First().Content.ContentType.Alias);
        }

        private static OverridableBlockListItem CreateUmbracoTestContentForClasses(string aliasOfParentBlock, string aliasOfChildBlock, bool fieldsetErrorsEnabled)
        {
            var blockListPropertyType = CreatePropertyType(1, Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration());
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var fieldsetContentType = new Mock<IPublishedContentType>();
            fieldsetContentType.Setup(x => x.Alias).Returns(aliasOfParentBlock);

            var fieldsetContent = new Mock<IOverridablePublishedElement>();
            fieldsetContent.Setup(x => x.ContentType).Returns(fieldsetContentType.Object);

            var fieldsetSettings = new Mock<IOverridablePublishedElement>();
            fieldsetSettings.Setup(x => x.GetProperty(PropertyAliases.FieldsetErrorsEnabled)).Returns(CreateProperty(PropertyAliases.FieldsetErrorsEnabled, textBoxPropertyType, fieldsetErrorsEnabled));

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
