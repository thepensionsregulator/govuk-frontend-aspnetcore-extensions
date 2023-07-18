using GovUk.Frontend.Umbraco.Validation;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class UmbracoBlockListValidationMetadataProviderTests
    {
        [Test]
        public void Attribute_error_message_is_updated_from_display_text_when_block_is_error_message()
        {
            var errorMessageContentType = new Mock<IPublishedContentType>();
            errorMessageContentType.Setup(x => x.Alias).Returns(ElementTypeAliases.ErrorMessage);

            var errorBlockContent = new Mock<IPublishedElement>();
            errorBlockContent.Setup(x => x.ContentType).Returns(errorMessageContentType.Object);
            errorBlockContent.Setup(x => x.GetProperty(PropertyAliases.ErrorMessage)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(PropertyAliases.ErrorMessage, "Custom required error"));

            var errorBlockSettings = new Mock<IPublishedElement>();
            errorBlockSettings.Setup(x => x.GetProperty(PropertyAliases.ModelProperty)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(PropertyAliases.ModelProperty, "Field1"));

            var errorBlock = new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), errorBlockContent.Object, Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), errorBlockSettings.Object);

            var attribute = new RequiredAttribute { ErrorMessage = "Field1" };
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { errorBlock },
                new List<object> { attribute },
                new Dictionary<Type, string> { { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired } });

            Assert.AreEqual("Custom required error", attribute.ErrorMessage);
        }

        [Test]
        public void Attribute_error_message_is_updated_from_settings_when_modelProperty_matches()
        {
            var textInputContentType = new Mock<IPublishedContentType>();
            textInputContentType.Setup(x => x.Alias).Returns(ElementTypeAliases.TextInput);

            var textInputContent = new Mock<IPublishedElement>();
            textInputContent.Setup(x => x.ContentType).Returns(textInputContentType.Object);

            var textInputSettings = new Mock<IPublishedElement>();
            textInputSettings.Setup(x => x.GetProperty(PropertyAliases.ModelProperty)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(PropertyAliases.ModelProperty, "Field1"));
            textInputSettings.Setup(x => x.GetProperty(PropertyAliases.ErrorMessageRequired)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(PropertyAliases.ErrorMessageRequired, "Custom required error"));

            var textInputBlock = new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), textInputContent.Object, Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), textInputSettings.Object);

            var attribute = new RequiredAttribute { ErrorMessage = "Field1" };
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { textInputBlock },
                new List<object> { attribute },
                new Dictionary<Type, string> { { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired } });

            Assert.AreEqual("Custom required error", attribute.ErrorMessage);
        }

        [Test]
        public void Attribute_error_message_is_not_updated_from_settings_when_modelProperty_does_not_match()
        {
            var block = UmbracoBlockListFactory.CreateBlock(
                            UmbracoBlockListFactory.CreateContentOrSettings().Object,
                            UmbracoBlockListFactory.CreateContentOrSettings()
                            .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, "Field1")
                            .SetupUmbracoTextboxPropertyValue(PropertyAliases.ErrorMessageRequired, "Custom required error")
                            .Object
                        );

            var attribute = new RequiredAttribute { ErrorMessage = "Original error" };
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { block },
                new List<object> { attribute },
                new Dictionary<Type, string> { { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired } });

            Assert.AreEqual("Original error", attribute.ErrorMessage);
        }


    }
}
