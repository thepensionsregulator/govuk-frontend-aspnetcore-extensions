﻿using GovUk.Frontend.AspNetCore.Extensions.Tests.CustomValidation;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    public class ClientSideValidationHtmlEnhancerTests
    {
        private const string errorMessageRequired = "Property is required";
        private const string errorMessageRegex = "Property must match regex";
        private const string errorMessageEmail = "Property must be an email address";
        private const string errorMessagePhone = "Property must be an phone number";
        private const string errorMessageLength = "Property failed length validation";
        private const string errorMessageMinLength = "Property failed minimum length validation";
        private const string errorMessageMaxLength = "Property failed maximum length validation";
        private const string errorMessageRange = "Property failed range validation";
        private const string errorMessageCompare = "Property failed compare validation";
        private const string errorMessageRequiredCustom = "Property must be FISH";

        private class ChildClass
        {
            [Required(ErrorMessage = errorMessageRequired)]
            public string? RequiredChildField { get; set; }
        }

        private class ExampleClass
        {
            public int IntegerField { get; set; }

            public double DoubleField { get; set; }

            [RegularExpression("[0-9.,]+", ErrorMessage = errorMessageRegex)]
            public double NumberFieldWithRegex { get; set; }

            public string? UnvalidatedField { get; set; }

            [Required(ErrorMessage = errorMessageRequired)]
            public string? RequiredField { get; set; }

            [RegularExpression("[0-9]+", ErrorMessage = errorMessageRegex)]
            public string? RegexField { get; set; }

            [EmailAddress(ErrorMessage = errorMessageEmail)]
            public string? EmailField { get; set; }

            [Phone(ErrorMessage = errorMessagePhone)]
            public string? PhoneField { get; set; }

            [StringLength(20, MinimumLength = 10, ErrorMessage = errorMessageLength)]
            public string? LengthField { get; set; }

            [MinLength(10, ErrorMessage = errorMessageMinLength)]
            public string? MinLengthField { get; set; }

            [MaxLength(10, ErrorMessage = errorMessageMinLength)]
            public string? MaxLengthField { get; set; }

            [Range(10, 20, ErrorMessage = errorMessageRange)]
            public int NumericRangeField { get; set; }

            [Range(typeof(DateTime), "2022-05-01", "2022-05-31", ErrorMessage = errorMessageRange)]
            public DateTime DateRangeField { get; set; }

            [Compare(nameof(UnvalidatedField), ErrorMessage = errorMessageCompare)]
            public string? CompareField { get; set; }


            [CustomTestValidator(ErrorMessage = errorMessageRequiredCustom)]
            public string? CustomField { get; set; }

            [Required(ErrorMessage = errorMessageRequired)]
            public ChildClass? ChildField { get; set; }
        }


        [Test]
        public void Validation_attributes_added_to_error_message_placeholder()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(Mock.Of<IModelPropertyResolver>(), Mock.Of<IModelMetadataProvider>(), Mock.Of<IOptions<MvcDataAnnotationsLocalizationOptions>>(), Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml(@$"<p class=""govuk-error-message""></p>
                                                    <input id=""{nameof(ExampleClass.RequiredField)}"" 
                                                           name=""{nameof(ExampleClass.RequiredField)}"">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode($"//p[@data-valmsg-for='{nameof(ExampleClass.RequiredField)}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//p[@data-valmsg-replace='false']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//p[@id='{nameof(ExampleClass.RequiredField)}-error']") != null);
        }

        [Test]
        public void If_error_message_is_empty_error_classes_are_removed()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(Mock.Of<IModelPropertyResolver>(), Mock.Of<IModelMetadataProvider>(), Mock.Of<IOptions<MvcDataAnnotationsLocalizationOptions>>(), Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml(@$"<div class=""govuk-form-group govuk-form-group--error"">
                                                    <p class=""govuk-error-message""></p>
                                                    <input id=""{nameof(ExampleClass.RequiredField)}"" 
                                                           name=""{nameof(ExampleClass.RequiredField)}""
                                                           class=""govuk-input--error govuk-textarea--error govuk-select--error"">
                                                    </div>",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-form-group--error')]") == null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-input--error')]") == null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-textarea--error')]") == null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-select--error')]") == null);
        }

        [Test]
        public void If_error_message_is_rendered_error_classes_remain()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(Mock.Of<IModelPropertyResolver>(), Mock.Of<IModelMetadataProvider>(), Mock.Of<IOptions<MvcDataAnnotationsLocalizationOptions>>(), Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml(@$"<div class=""govuk-form-group govuk-form-group--error"">
                                                    <p class=""govuk-error-message"">Something went wrong</p>
                                                    <input id=""{nameof(ExampleClass.RequiredField)}"" 
                                                           name=""{nameof(ExampleClass.RequiredField)}""
                                                           class=""govuk-input--error govuk-textarea--error govuk-select--error"">
                                                    </div>",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-form-group--error')]") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-input--error')]") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-textarea--error')]") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//*[contains(@class,'govuk-select--error')]") != null);
        }

        [Test]
        public void Input_is_unchanged_if_validation_disabled()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = false };
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), Mock.Of<IOptions<MvcDataAnnotationsLocalizationOptions>>(), Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.RequiredField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            Assert.AreEqual($"<input name=\"{nameof(ExampleClass.RequiredField)}\">", result);
        }

        [Test]
        public void Input_is_unchanged_if_property_has_no_validators()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.UnvalidatedField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.UnvalidatedField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.UnvalidatedField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            Assert.AreEqual($"<input name=\"{nameof(ExampleClass.UnvalidatedField)}\">", result);
        }

        [Test]
        public void Required_validator_adds_required_attributes_to_multiple_inputs()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            // Check support for multiple inputs with the same name, because the required attribute
            // has to support a group of radio buttons or checkboxes
            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.RequiredField)}\"><input name=\"{nameof(ExampleClass.RequiredField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectNodes("//input[@data-val='true']")?.Count == 2);
            Assert.True(document.DocumentNode.SelectNodes("//input[@required='required']")?.Count == 2);
            Assert.True(document.DocumentNode.SelectNodes($"//input[@data-val-required='{errorMessageRequired}']")?.Count == 2);
        }

        [Test]
        public void Custom_validator_adds_custom_attributes_to_inputs()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.CustomField))!;
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());

            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.CustomField))).Returns(property);

            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForProperty(containerType: typeof(ExampleClass), propertyName: nameof(ExampleClass.CustomField));

            var mockMetaDataProvider = new Mock<IModelMetadataProvider>();
            mockMetaDataProvider.Setup(x => x.GetMetadataForType(typeof(ExampleClass))).Returns(metadata);

            var baseValidationAttribute = property.GetCustomAttribute<CustomTestValidatorAttribute>()!;
            var validationAttributeAdapterProvider = new Mock<IValidationAttributeAdapterProvider>();
            validationAttributeAdapterProvider.Setup(x => x.GetAttributeAdapter(It.IsAny<ValidationAttribute>(), It.IsAny<IStringLocalizer>())).Returns(
                new CustomTestValidatorAttributeAdapter(baseValidationAttribute, null));

            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, mockMetaDataProvider.Object, options,
                validationAttributeAdapterProvider.Object);

            // Check support for multiple inputs with the same name, because the required attribute
            // has to support a group of radio buttons or checkboxes
            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.CustomField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectNodes("//input[@data-val='true']")?.Count == 1);
            Assert.True(document.DocumentNode.SelectNodes($"//input[@data-val-custom='{errorMessageRequiredCustom}']")?.Count == 1);
        }

        [Test]
        public void Required_validator_adds_required_attributes_to_select()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<select name=\"{nameof(ExampleClass.RequiredField)}\"></select>",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//select[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//select[@required='required']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//select[@data-val-required='{errorMessageRequired}']") != null);
        }


        [Test]
        public void Required_validator_adds_required_attributes_to_textarea()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<textarea name=\"{nameof(ExampleClass.RequiredField)}\"></textarea>",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//textarea[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//textarea[@required='required']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//textarea[@data-val-required='{errorMessageRequired}']") != null);
        }

        [Test]
        public void Regex_validator_adds_regex_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.RegexField))!;
            var pattern = ((RegularExpressionAttribute)property.GetCustomAttributes(typeof(RegularExpressionAttribute), false)[0]).Pattern;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RegexField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.RegexField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-regex-pattern='{pattern}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@pattern='{pattern}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-regex='{errorMessageRegex}']") != null);
        }

        [Test]
        public void Email_validator_adds_email_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.EmailField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.EmailField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.EmailField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//input[@type='email']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//input[@autocomplete='email']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-email='{errorMessageEmail}']") != null);
        }

        [Test]
        public void Phone_validator_adds_phone_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.PhoneField))).Returns(typeof(ExampleClass).GetProperty(nameof(ExampleClass.PhoneField))!);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.PhoneField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//input[@type='tel']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode("//input[@autocomplete='tel']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-phone='{errorMessagePhone}']") != null);
        }

        [Test]
        public void Length_validator_adds_length_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.LengthField))!;
            var minLength = ((StringLengthAttribute)property.GetCustomAttributes(typeof(StringLengthAttribute), false)[0]).MinimumLength;
            var maxLength = ((StringLengthAttribute)property.GetCustomAttributes(typeof(StringLengthAttribute), false)[0]).MaximumLength;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.LengthField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.LengthField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-length='{errorMessageLength}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-length-min='{minLength}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-length-max='{maxLength}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@maxlength='{maxLength}']") != null);
        }

        [Test]
        public void MinLength_validator_adds_length_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.MinLengthField))!;
            var minLength = ((MinLengthAttribute)property.GetCustomAttributes(typeof(MinLengthAttribute), false)[0]).Length;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.MinLengthField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.MinLengthField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-minlength='{errorMessageMinLength}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-minlength-min='{minLength}']") != null);
        }


        [Test]
        public void MaxLength_validator_adds_length_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.MaxLengthField))!;
            var maxLength = ((MaxLengthAttribute)property.GetCustomAttributes(typeof(MaxLengthAttribute), false)[0]).Length;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.MaxLengthField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.MaxLengthField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-maxlength='{errorMessageMaxLength}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-maxlength-max='{maxLength}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@maxlength='{maxLength}']") != null);
        }

        [Test]
        public void Numeric_range_validator_adds_range_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.NumericRangeField))!;
            var min = ((RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false)[0]).Minimum;
            var max = ((RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false)[0]).Maximum;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.NumericRangeField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.NumericRangeField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-range='{errorMessageRange}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-range-min='{min}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-range-max='{max}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@type='text']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@inputmode='numeric']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@pattern='[0-9]*']") != null);
        }

        [Test]
        public void Date_range_validator_adds_range_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.DateRangeField))!;
            var min = ((RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false)[0]).Minimum;
            var max = ((RangeAttribute)property.GetCustomAttributes(typeof(RangeAttribute), false)[0]).Maximum;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.DateRangeField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.DateRangeField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-range='{errorMessageRange}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-range-min='{min}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-range-max='{max}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@type='number']") == null); // different to the numeric version of this test
        }

        [TestCase(nameof(ExampleClass.IntegerField), "[0-9]*")]
        [TestCase(nameof(ExampleClass.DoubleField), "[0-9.]*")]
        public void Numeric_fields_without_range_have_text_input_type_numeric_input_mode(string propertyName, string expectedRegex)
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(propertyName)!;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), propertyName)).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{propertyName}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@type='text']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@inputmode='numeric']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@pattern='{expectedRegex}']") != null);
        }


        [Test]
        public void Regex_validator_has_higher_priority_than_regex_for_numeric_fields()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.NumberFieldWithRegex))!;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.NumberFieldWithRegex))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.NumberFieldWithRegex)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@pattern='[0-9.,]+']") != null);
        }

        [Test]
        public void Compare_validator_adds_compare_attributes()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.CompareField))!;
            var other = ((CompareAttribute)property.GetCustomAttributes(typeof(CompareAttribute), false)[0]).OtherProperty;
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.CompareField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.CompareField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val='true']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-equalto='{errorMessageCompare}']") != null);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-equalto-other='{other}']") != null);
        }

        [Test]
        public void Error_message_comes_from_parameters_first()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!;
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(property);
            var localiserFactory = new Mock<IStringLocalizerFactory>();
            var localiser = new Mock<IStringLocalizer>();
            localiserFactory.Setup(x => x.Create(property.DeclaringType!)).Returns(localiser.Object);
            localiser.Setup(x => x[errorMessageRequired]).Returns(new LocalizedString(errorMessageRequired, "Error from localiser"));
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>(), localiserFactory.Object);

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.RequiredField)}\">",
                viewContext,
                "Error from parameters",
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            localiser.Verify(x => x[errorMessageRequired], Times.Never);
            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val-required='Error from parameters']") != null);
        }

        [Test]
        public void Error_message_comes_from_localiser_if_no_parameter()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!;
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(property);
            var localiserFactory = new Mock<IStringLocalizerFactory>();
            var localiser = new Mock<IStringLocalizer>();
            localiserFactory.Setup(x => x.Create(property.DeclaringType!)).Returns(localiser.Object);
            localiser.Setup(x => x[errorMessageRequired]).Returns(new LocalizedString(errorMessageRequired, "Error from localiser"));

            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions()
            {
                DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(type);
                }
            });

            var htmlUpdater = new ClientSideValidationHtmlEnhancer(
                propertyResolver.Object,
                Mock.Of<IModelMetadataProvider>(),
                options,
                Mock.Of<IValidationAttributeAdapterProvider>(),
                localiserFactory.Object);

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.RequiredField)}\">",
                viewContext,
                null,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val-required='Error from localiser']") != null);
        }

        [Test]
        public void Error_message_comes_from_original_attribute_if_no_parameter_or_localiser()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.RequiredField))!;
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.RequiredField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.RequiredField)}\">",
                viewContext,
                null,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Console.Write(document.DocumentNode.OuterHtml);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-required='{errorMessageRequired}']") != null);
        }

        [Test]
        public void Child_Properties_are_validated()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions());
            var property = typeof(ChildClass).GetProperty(nameof(ChildClass.RequiredChildField))!;
            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.ChildField.RequiredChildField))).Returns(property);
            var htmlUpdater = new ClientSideValidationHtmlEnhancer(propertyResolver.Object, Mock.Of<IModelMetadataProvider>(), options, Mock.Of<IValidationAttributeAdapterProvider>());

            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.ChildField.RequiredChildField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Console.Write(document.DocumentNode.OuterHtml);
            Assert.True(document.DocumentNode.SelectSingleNode($"//input[@data-val-required='{errorMessageRequired}']") != null);

        }

        [Test]
        public void Error_message_for_custom_attributes_comes_from_localiser()
        {
            var viewContext = new ViewContext() { ClientValidationEnabled = true };
            var property = typeof(ExampleClass).GetProperty(nameof(ExampleClass.CustomField))!;

            var propertyResolver = new Mock<IModelPropertyResolver>();
            propertyResolver.Setup(x => x.ResolveModelType(viewContext)).Returns(typeof(ExampleClass));
            propertyResolver.Setup(x => x.ResolveModelProperty(typeof(ExampleClass), nameof(ExampleClass.CustomField))).Returns(property);


            var localiser = new Mock<IStringLocalizer>(MockBehavior.Loose);

            localiser.Setup(x => x[errorMessageRequiredCustom, It.IsAny<object[]>()]).Returns(new LocalizedString(errorMessageRequiredCustom, "Error from localiser"));
            var localiserFactory = new Mock<IStringLocalizerFactory>();
            localiserFactory.Setup(x => x.Create(property.DeclaringType!)).Returns(localiser.Object);

            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForProperty(containerType: typeof(ExampleClass), propertyName: nameof(ExampleClass.CustomField));

            var mockMetaDataProvider = new Mock<IModelMetadataProvider>();
            mockMetaDataProvider.Setup(x => x.GetMetadataForType(typeof(ExampleClass))).Returns(metadata);

            var options = Options.Create(new MvcDataAnnotationsLocalizationOptions()
            {
                DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(type);
                }
            });

            var baseValidationAttribute = property.GetCustomAttribute<CustomTestValidatorAttribute>()!;
            var validationAttributeAdapterProvider = new Mock<IValidationAttributeAdapterProvider>();
            validationAttributeAdapterProvider.Setup(x => x.GetAttributeAdapter(It.IsAny<ValidationAttribute>(), It.IsAny<IStringLocalizer>())).Returns(
                new CustomTestValidatorAttributeAdapter(baseValidationAttribute, localiser.Object));

            var htmlUpdater = new ClientSideValidationHtmlEnhancer(
                propertyResolver.Object,
                mockMetaDataProvider.Object,
                options,
                validationAttributeAdapterProvider.Object,
                localiserFactory.Object);

            // Check support for multiple inputs with the same name, because the required attribute
            // has to support a group of radio buttons or checkboxes
            var result = htmlUpdater.EnhanceHtml($"<input name=\"{nameof(ExampleClass.CustomField)}\">",
                viewContext,
                errorMessageRequired,
                errorMessageRegex,
                errorMessageEmail,
                errorMessagePhone,
                errorMessageLength,
                errorMessageMinLength,
                errorMessageMaxLength,
                errorMessageRange,
                errorMessageCompare);

            var document = new HtmlDocument();
            document.LoadHtml(result);

            Assert.True(document.DocumentNode.SelectSingleNode("//input[@data-val-custom='Error from localiser']") != null);
        }
    }
}
