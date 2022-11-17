using GovUk.Frontend.Umbraco.Validation;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class UmbracoBlockListValidationMetadataProviderTests : UmbracoBaseTest
    {
        [Test]
        public void Nested_blocklists_are_found_recursively()
        {
            var blockListPropertyType = CreatePropertyType(1, Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration());

            var grandChildBlockList = CreateBlockListModel(Array.Empty<IPublishedProperty>(), Array.Empty<IPublishedProperty>());
            var childBlockList = CreateBlockListModel(new[] { CreateProperty("blocks", blockListPropertyType, grandChildBlockList) }, Array.Empty<IPublishedProperty>());
            var parentBlockList = CreateBlockListModel(new[] { CreateProperty("blocks", blockListPropertyType, childBlockList) }, Array.Empty<IPublishedProperty>());

            var provider = new UmbracoBlockListValidationMetadataProvider(Mock.Of<IUmbracoContextAccessor>(), typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired);

            var result = provider.RecursivelyGetBlockLists(new List<IPublishedProperty> { CreateProperty("blocks", blockListPropertyType, parentBlockList) });

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void Attribute_error_message_is_updated_from_display_text_when_block_is_error_message()
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var errorMessageContentType = new Mock<IPublishedContentType>();
            errorMessageContentType.Setup(x => x.Alias).Returns(ElementTypeAliases.ErrorMessage);

            var errorBlockContent = new Mock<IPublishedElement>();
            errorBlockContent.Setup(x => x.ContentType).Returns(errorMessageContentType.Object);
            errorBlockContent.Setup(x => x.GetProperty(PropertyAliases.ErrorMessage)).Returns(CreateProperty(PropertyAliases.ErrorMessage, textBoxPropertyType, "Custom required error"));

            var errorBlockSettings = new Mock<IPublishedElement>();
            errorBlockSettings.Setup(x => x.GetProperty(PropertyAliases.ModelProperty)).Returns(CreateProperty(PropertyAliases.ModelProperty, textBoxPropertyType, "Field1"));

            var errorBlock = new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), errorBlockContent.Object, Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), errorBlockSettings.Object);

            var blockList = new BlockListModel(new[] { errorBlock });

            var attribute = new RequiredAttribute { ErrorMessage = "Field1" };
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { blockList },
                new List<object> { attribute },
                new Dictionary<Type, string> { { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired } });

            Assert.AreEqual("Custom required error", attribute.ErrorMessage);
        }

        [Test]
        public void Attribute_error_message_is_updated_from_settings_when_modelProperty_matches()
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var textInputContentType = new Mock<IPublishedContentType>();
            textInputContentType.Setup(x => x.Alias).Returns(ElementTypeAliases.TextInput);

            var textInputContent = new Mock<IPublishedElement>();
            textInputContent.Setup(x => x.ContentType).Returns(textInputContentType.Object);

            var textInputSettings = new Mock<IPublishedElement>();
            textInputSettings.Setup(x => x.GetProperty(PropertyAliases.ModelProperty)).Returns(CreateProperty(PropertyAliases.ModelProperty, textBoxPropertyType, "Field1"));
            textInputSettings.Setup(x => x.GetProperty(PropertyAliases.ErrorMessageRequired)).Returns(CreateProperty(PropertyAliases.ErrorMessageRequired, textBoxPropertyType, "Custom required error"));

            var textInputBlock = new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), textInputContent.Object, Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), textInputSettings.Object);

            var blockList = new BlockListModel(new[] { textInputBlock });

            var attribute = new RequiredAttribute { ErrorMessage = "Field1" };
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { blockList },
                new List<object> { attribute },
                new Dictionary<Type, string> { { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired } });

            Assert.AreEqual("Custom required error", attribute.ErrorMessage);
        }

        [Test]
        public void Attribute_error_message_is_not_updated_from_settings_when_modelProperty_does_not_match()
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var blockList = CreateBlockListModel(Array.Empty<IPublishedProperty>(), new[] {
                CreateProperty(PropertyAliases.ModelProperty, textBoxPropertyType, "Field1"),
                CreateProperty(PropertyAliases.ErrorMessageRequired, textBoxPropertyType, "Custom required error")
            });

            var attribute = new RequiredAttribute { ErrorMessage = "Original error" };
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { blockList },
                new List<object> { attribute },
                new Dictionary<Type, string> { { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired } });

            Assert.AreEqual("Original error", attribute.ErrorMessage);
        }


    }
}
