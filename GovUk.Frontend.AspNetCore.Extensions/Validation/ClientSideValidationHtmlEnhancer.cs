﻿using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class ClientSideValidationHtmlEnhancer : IClientSideValidationHtmlEnhancer
    {
        private readonly IModelPropertyResolver _modelPropertyResolver;
        private readonly IStringLocalizerFactory? _factory;

        public ClientSideValidationHtmlEnhancer(IModelPropertyResolver modelPropertyResolver, IStringLocalizerFactory? factory = null)
        {
            _modelPropertyResolver = modelPropertyResolver ?? throw new ArgumentNullException(nameof(modelPropertyResolver));
            _factory = factory;
        }

        public string EnhanceHtml(string html,
            ViewContext viewContext,
            string? errorMessageRequired,
            string? errorMessageRegex,
            string? errorMessageEmail,
            string? errorMessageLength,
            string? errorMessageMinLength,
            string? errorMessageMaxLength,
            string? errorMessageRange,
            string? errorMessageCompare)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            // Get the input element that should always be there if a <govuk-input> child exists.
            var inputs = document.DocumentNode.SelectNodes("//input");
            if (inputs == null) inputs = document.DocumentNode.SelectNodes("//select");
            if (inputs == null) inputs = document.DocumentNode.SelectNodes("//textarea");

            if (inputs != null && inputs.Count > 0)
            {
                // Get the output of the <govuk-input-error-message> grandchild tag helper, if present.
                var errorMessage = document.DocumentNode.SelectSingleNode("//*[@class='govuk-error-message']");

                // If <govuk-input-error-message> is present, the <govuk-input> tag helper always adds error classes because it assumes
                // we put it there because we already detected an error on the server. But we might want it there at all times to enable 
                // client-side validation, so remove those classes if there is not actually an error.
                bool hasError = ErrorMessageTagHelperHasRenderedAnError(errorMessage);
                if (!hasError)
                {
                    RemoveErrorClasses(document, inputs);
                }

                // Add the data-val-* attributes for ASP.NET / jQuery validation to pick up
                AddClientSideValidationAttributes(viewContext, _modelPropertyResolver, _factory, inputs, errorMessage?.Attributes,
                    errorMessageRequired,
                    errorMessageRegex,
                    errorMessageEmail,
                    errorMessageLength,
                    errorMessageMinLength,
                    errorMessageMaxLength,
                    errorMessageRange,
                    errorMessageCompare);
            }

            return document.DocumentNode.OuterHtml;
        }

        private static void RemoveErrorClasses(HtmlDocument html, HtmlNodeCollection inputs)
        {
            var errorContainer = html.DocumentNode.SelectSingleNode("//*[contains(@class,'govuk-form-group--error')]");
            if (errorContainer != null) { errorContainer.RemoveClass("govuk-form-group--error"); }

            foreach (var input in inputs)
            {
                input.RemoveClass("govuk-input--error");
                input.RemoveClass("govuk-textarea--error");
                input.RemoveClass("govuk-select--error");
            }
        }

        private static bool ErrorMessageTagHelperHasRenderedAnError(HtmlNode? errorMessage)
        {
            if (errorMessage == null) { return false; }
            foreach (var node in errorMessage.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element && !node.HasClass("govuk-visually-hidden") ||
                    node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerText))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddClientSideValidationAttributes(ViewContext viewContext,
            IModelPropertyResolver propertyResolver,
            IStringLocalizerFactory? stringLocalizerFactory,
            HtmlNodeCollection targetElements,
            HtmlAttributeCollection? errorMessageAttributes,
            string? errorMessageRequired,
            string? errorMessageRegex,
            string? errorMessageEmail,
            string? errorMessageLength,
            string? errorMessageMinLength,
            string? errorMessageMaxLength,
            string? errorMessageRange,
            string? errorMessageCompare)
        {
            if (viewContext == null || !viewContext.ClientValidationEnabled) { return; }

            IStringLocalizer? localizer = null;
            var modelType = propertyResolver.ResolveModelType(viewContext);

            foreach (var targetElement in targetElements)
            {
                if (errorMessageAttributes != null)
                {
                    var targetElementId = targetElement.Attributes["id"]?.Value;
                    errorMessageAttributes.Add("data-valmsg-for", targetElementId);
                    errorMessageAttributes.Add("data-valmsg-replace", "false");
                    errorMessageAttributes.Add("id", targetElementId + "-error");
                }

                var modelPropertyName = targetElement.Attributes["name"]?.Value;
                if (string.IsNullOrEmpty(modelPropertyName)) { continue; }
                var modelProperty = propertyResolver.ResolveModelProperty(modelType, modelPropertyName);

                if (modelProperty != null)
                {
                    if (stringLocalizerFactory != null)
                    {
                        localizer = stringLocalizerFactory.Create(modelProperty.DeclaringType!);
                    }

                    var validateElement = false;

                    // Compare
                    var compareAttr = modelProperty.GetCustomAttributes<CompareAttribute>().FirstOrDefault();
                    if (compareAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-equalto", SelectBestErrorMessage(errorMessageCompare, compareAttr.ErrorMessage, localizer));
                        targetElement.Attributes.Add("data-val-equalto-other", compareAttr.OtherProperty);
                        validateElement = true;
                    }

                    // Email Address
                    var emailAttr = modelProperty.GetCustomAttributes<EmailAddressAttribute>().FirstOrDefault();
                    if (emailAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-email", SelectBestErrorMessage(errorMessageEmail, emailAttr.ErrorMessage, localizer));
                        AddOrUpdateHtmlAttribute(targetElement, "type", "email");
                        validateElement = true;
                    }

                    // Max Length
                    var maxLengthAttr = modelProperty.GetCustomAttributes<MaxLengthAttribute>().FirstOrDefault();
                    if (maxLengthAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-maxlength", SelectBestErrorMessage(errorMessageMaxLength, maxLengthAttr.ErrorMessage, localizer));
                        targetElement.Attributes.Add("data-val-maxlength-max", maxLengthAttr.Length.ToString());
                        targetElement.Attributes.Add("maxlength", maxLengthAttr.Length.ToString());
                        validateElement = true;
                    }

                    // Min Length
                    var minLengthAttr = modelProperty.GetCustomAttributes<MinLengthAttribute>().FirstOrDefault();
                    if (minLengthAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-minlength", SelectBestErrorMessage(errorMessageMinLength, minLengthAttr.ErrorMessage, localizer));
                        targetElement.Attributes.Add("data-val-minlength-min", minLengthAttr.Length.ToString());
                        validateElement = true;
                    }

                    // Range
                    var rangeAttr = modelProperty.GetCustomAttributes<RangeAttribute>().FirstOrDefault();
                    if (rangeAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-range", SelectBestErrorMessage(errorMessageRange, rangeAttr.ErrorMessage, localizer));
                        targetElement.Attributes.Add("data-val-range-max", rangeAttr.Maximum.ToString());
                        targetElement.Attributes.Add("data-val-range-min", rangeAttr.Minimum.ToString());
                        if (IsNumericType(modelProperty.PropertyType))
                        {
                            AddOrUpdateHtmlAttribute(targetElement, "type", "number");
                        }
                        validateElement = true;
                    }

                    // Regex
                    var regexAttr = modelProperty.GetCustomAttributes<RegularExpressionAttribute>().FirstOrDefault();
                    if (regexAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-regex", SelectBestErrorMessage(errorMessageRegex, regexAttr.ErrorMessage, localizer));
                        targetElement.Attributes.Add("data-val-regex-pattern", regexAttr.Pattern);
                        targetElement.Attributes.Add("pattern", regexAttr.Pattern);
                        validateElement = true;
                    }

                    // Required
                    var reqdAttr = modelProperty.GetCustomAttributes<RequiredAttribute>().FirstOrDefault();
                    if (reqdAttr != null)
                    {
                        targetElement.Attributes.Add("required", "required");
                        targetElement.Attributes.Add("data-val-required", SelectBestErrorMessage(errorMessageRequired, reqdAttr.ErrorMessage, localizer));
                        validateElement = true;
                    }

                    // String Length
                    var strLenAttr = modelProperty.GetCustomAttributes(typeof(StringLengthAttribute), true).Cast<StringLengthAttribute>().FirstOrDefault();
                    if (strLenAttr != null)
                    {
                        targetElement.Attributes.Add("data-val-length", SelectBestErrorMessage(errorMessageLength, strLenAttr.ErrorMessage, localizer));
                        targetElement.Attributes.Add("data-val-length-max", strLenAttr.MaximumLength.ToString());
                        targetElement.Attributes.Add("data-val-length-min", strLenAttr.MinimumLength.ToString());
                        targetElement.Attributes.Add("maxlength", strLenAttr.MaximumLength.ToString());
                        validateElement = true;
                    }

                    if (validateElement) { targetElement.Attributes.Add("data-val", "true"); }

                }
            }
        }

        private static void AddOrUpdateHtmlAttribute(HtmlNode element, string name, string value)
        {
            if (element.Attributes.AttributesWithName(name).Any())
            {
                element.Attributes[name].Value = value;
            }
            else
            {
                element.Attributes.Add(name, value);
            }
        }

        private static string? SelectBestErrorMessage(string? fromTagHelperAttribute, string? fromDataAnnotationsAttribute, IStringLocalizer? localizer)
        {
            return string.IsNullOrEmpty(fromTagHelperAttribute) ?
                        (localizer != null && !string.IsNullOrEmpty(fromDataAnnotationsAttribute) ? localizer[fromDataAnnotationsAttribute!] : fromDataAnnotationsAttribute) :
                        fromTagHelperAttribute;
        }

        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        internal static bool IsNumericType(Type type)
        {
            return NumericTypes.Contains(type) ||
                   NumericTypes.Contains(Nullable.GetUnderlyingType(type)!);
        }
    }
}