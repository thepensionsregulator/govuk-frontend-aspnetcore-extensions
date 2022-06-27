using GovUk.Frontend.Umbraco.Validation;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Umbraco.Cms.Core;
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

            var provider = new UmbracoBlockListValidationMetadataProvider(Mock.Of<IUmbracoContextAccessor>());

            var result = provider.RecursivelyGetBlockLists(new List<IPublishedProperty> { CreateProperty("blocks", blockListPropertyType, parentBlockList) });

            Assert.AreEqual(3, result.Count());
        }

        [TestCase(typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired, null, null)]
        [TestCase(typeof(RegularExpressionAttribute), PropertyAliases.ErrorMessageRegex, "test", null)]
        [TestCase(typeof(EmailAddressAttribute), PropertyAliases.ErrorMessageEmail, null, null)]
        [TestCase(typeof(PhoneAttribute), PropertyAliases.ErrorMessagePhone, null, null)]
        [TestCase(typeof(StringLengthAttribute), PropertyAliases.ErrorMessageLength, 1, null)]
        [TestCase(typeof(MinLengthAttribute), PropertyAliases.ErrorMessageMinLength, 1, null)]
        [TestCase(typeof(MaxLengthAttribute), PropertyAliases.ErrorMessageMaxLength, 1, null)]
        [TestCase(typeof(System.ComponentModel.DataAnnotations.RangeAttribute), PropertyAliases.ErrorMessageRange, 1, 1)]
        [TestCase(typeof(CompareAttribute), PropertyAliases.ErrorMessageCompare, "test", null)]
        public void Attribute_error_message_is_updated_from_settings_when_modelProperty_matches_for_all_supported_attributes(Type attributeType, string errorMessagePropertyAlias, object param1, object param2)
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var blockList = CreateBlockListModel(Array.Empty<IPublishedProperty>(), new[] {
                CreateProperty(PropertyAliases.ModelProperty, textBoxPropertyType, "Field1"),
                CreateProperty(errorMessagePropertyAlias, textBoxPropertyType, "Custom required error")
            });

            ValidationAttribute? attribute = null;
            if (param1 != null && param2 != null)
            {
                attribute = (ValidationAttribute)Activator.CreateInstance(attributeType, param1, param2)!;
            }
            else if (param1 != null)
            {
                attribute = (ValidationAttribute)Activator.CreateInstance(attributeType, param1)!;
            }
            else
            {
                attribute = (ValidationAttribute)Activator.CreateInstance(attributeType)!;
            }
            attribute.ErrorMessage = "Field1";
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { blockList }, new List<object> { attribute });

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
            UmbracoBlockListValidationMetadataProvider.UpdateValidationAttributeErrorMessages(new[] { blockList }, new List<object> { attribute });

            Assert.AreEqual("Original error", attribute.ErrorMessage);
        }


    }
}
