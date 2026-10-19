using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public TagBuilder GenerateTprAddressLookup(string @for, ModelStateDictionary modelStateDictionary, TprAddressLookupContent content, IDictionary<string, int> countries)
        {
            var name = @for;
            var fieldset = new TagBuilder("fieldset");
            fieldset.AddCssClass("govuk-fieldset");
            fieldset.AddCssClass("tpr-address-lookup-view");

            var legendElement = new TagBuilder("legend");
            legendElement.AddCssClass("govuk-fieldset__legend govuk-fieldset__legend--l");
            legendElement.InnerHtml.Append(content.NoJsLegend);
            fieldset.InnerHtml.AppendHtml(legendElement);

            if (content.AddressLookupIntroductionText is not null)
            {
                var hintElement = new TagBuilder("div");
                hintElement.AddCssClass("govuk-hint");
                hintElement.InnerHtml.Append(content.AddressLookupIntroductionText);
                fieldset.InnerHtml.AppendHtml(hintElement);
            }

            var addressLine1 = GetFieldState(name, nameof(TprAddress.AddressLine1), AddressFieldNames.AddressLine1, modelStateDictionary);
            var addressLine2 = GetFieldState(name, nameof(TprAddress.AddressLine2), AddressFieldNames.AddressLine2, modelStateDictionary);
            var addressLine3 = GetFieldState(name, nameof(TprAddress.AddressLine3), AddressFieldNames.AddressLine3, modelStateDictionary);
            var townOrCity = GetFieldState(name, nameof(TprAddress.PostTown), AddressFieldNames.PostTown, modelStateDictionary);
            var postCountry = GetFieldState(name, nameof(TprAddress.PostCounty), AddressFieldNames.PostCounty, modelStateDictionary);
            var countryId = GetFieldState(name, nameof(TprAddress.CountryId), AddressFieldNames.CountryId, modelStateDictionary);
            var postcode = GetFieldState(name, nameof(TprAddress.Postcode), AddressFieldNames.Postcode, modelStateDictionary);
            var uprnReference = GetFieldState(name, nameof(TprAddress.UPRNReference), AddressFieldNames.UprnReference, modelStateDictionary);

            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine1, content.AddressLine1Label, required: true, requiredErrorMessage: content.AddressLine1RequiredErrorMessage, maxLength: 100, maxLengthErrorMessage: content.AddressLine1MaxLengthErrorMessage));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine2, content.AddressLine2Label, required: false, maxLength: 100, maxLengthErrorMessage: content.AddressLine2MaxLengthErrorMessage));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine3, content.AddressLine3Label, required: false, maxLength: 100, maxLengthErrorMessage: content.AddressLine3MaxLengthErrorMessage));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(townOrCity, content.PostTownLabel, required: true, requiredErrorMessage: content.PostTownRequiredErrorMessage, maxLength: 100, maxLengthErrorMessage: content.PostTownMaxLengthErrorMessage, widthClass: "govuk-input--width-20"));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(postCountry, content.CountyLabel, required: false, maxLength: 100, maxLengthErrorMessage: content.CountyMaxLengthErrorMessage, widthClass: "govuk-input--width-20"));
            fieldset.InnerHtml.AppendHtml(CreateSelectInput(countryId, content.CountryLabel, content.CountrySelectPlaceholder, true, content.CountryRequiredErrorMessage, "govuk-input--width-10", countries));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(postcode, content.PostcodeLabel, required: false, maxLength: 10, maxLengthErrorMessage: content.PostcodeMaxLengthErrorMessage, widthClass: "govuk-input--width-10"));

            var uprnReferenceInput = new TagBuilder("input");
            uprnReferenceInput.Attributes.Add("type", "hidden");
            uprnReferenceInput.Attributes.Add("id", "uprnReference");
            uprnReferenceInput.Attributes.Add("name", $"{name}.uprnReference");
            uprnReferenceInput.Attributes.Add("data-address-field", uprnReference.FieldKey);
            fieldset.InnerHtml.AppendHtml(uprnReferenceInput);

            return fieldset;
        }

        private static TagBuilder CreateTextInput(FieldState fieldState, string labelText, bool required, string? requiredErrorMessage = null, int? maxLength = null, string? maxLengthErrorMessage = null, string? widthClass = null)
        {
            var label = new TagBuilder("label");
            label.AddCssClass("govuk-label");
            label.Attributes.Add("for", fieldState.Name);
            label.InnerHtml.Append(labelText);

            var input = new TagBuilder("input");
            input.AddCssClass("govuk-input");
            input.Attributes.Add("type", "text");
            input.Attributes.Add("id", fieldState.Name);
            input.Attributes.Add("name", fieldState.Name);
            input.Attributes.Add("data-address-field", fieldState.FieldKey);
            if (required)
            {
                input.Attributes.Add("required", "");
                input.Attributes.Add("aria-required", "true");
                input.Attributes["data-val"] = "true";
                input.Attributes.Add("data-val-required", requiredErrorMessage ?? string.Empty);
            }
            if (maxLength is not null)
            {
                input.Attributes.Add("maxlength", maxLength.Value.ToString());
                input.Attributes["data-val"] = "true";
                input.Attributes.Add("data-val-length", maxLengthErrorMessage ?? string.Empty);
                input.Attributes.Add("data-val-length-max", maxLength.Value.ToString());
            }

            if (fieldState.AttemptedValue is not null)
            {
                input.Attributes.Add("value", fieldState.AttemptedValue);
            }

            var group = new TagBuilder("div");
            group.AddCssClass("govuk-form-group");
            if (!string.IsNullOrEmpty(widthClass))
            {
                group.AddCssClass(widthClass);
            }
            group.InnerHtml.AppendHtml(label);

            if (fieldState.HasErrors)
            {
                input.AddCssClass("govuk-input--error");
                input.Attributes.Add("aria-describedby", $"{fieldState.Name}-error");
                group.AddCssClass("govuk-form-group--error");
                group.InnerHtml.AppendHtml(CreateErrorMessage(fieldState));
            }

            group.InnerHtml.AppendHtml(input);

            return group;
        }

        private static TagBuilder CreateSelectInput(FieldState fieldState, string labelText, string placeholderText, bool required, string requiredErrorMessage, string widthClass, IDictionary<string, int> options)
        {
            var label = new TagBuilder("label");
            label.AddCssClass("govuk-label");
            label.Attributes.Add("for", fieldState.Name);
            label.InnerHtml.Append(labelText);

            var select = new TagBuilder("select");
            select.AddCssClass("govuk-select");
            select.Attributes.Add("id", fieldState.Name);
            select.Attributes.Add("name", fieldState.Name);
            select.Attributes.Add("data-address-field", fieldState.FieldKey);
            if (required)
            {
                select.Attributes.Add("required", "");
                select.Attributes.Add("aria-required", "true");
                select.Attributes.Add("data-val", "true");
                select.Attributes.Add("data-val-required", requiredErrorMessage);
            }

            var hasSelection = !string.IsNullOrEmpty(fieldState.AttemptedValue);

            var placeholderOption = new TagBuilder("option");
            placeholderOption.Attributes.Add("value", "");
            if (!hasSelection)
            {
                placeholderOption.Attributes.Add("selected", "selected");
            }
            placeholderOption.InnerHtml.Append(placeholderText);
            select.InnerHtml.AppendHtml(placeholderOption);

            foreach (var option in options)
            {
                var optionTag = new TagBuilder("option");
                optionTag.Attributes.Add("value", option.Value.ToString());
                optionTag.InnerHtml.Append(option.Key);
                if (hasSelection && fieldState.AttemptedValue is not null && fieldState.AttemptedValue == option.Value.ToString())
                {
                    optionTag.Attributes.Add("selected", "selected");
                }
                select.InnerHtml.AppendHtml(optionTag);
            }

            var group = new TagBuilder("div");
            group.AddCssClass("govuk-form-group");
            group.AddCssClass(widthClass);
            group.InnerHtml.AppendHtml(label);
            if (fieldState.HasErrors)
            {
                select.AddCssClass("govuk-select--error");
                select.Attributes.Add("aria-describedby", $"{fieldState.Name}-error");
                group.AddCssClass("govuk-form-group--error");
                group.InnerHtml.AppendHtml(CreateErrorMessage(fieldState));
            }
            group.InnerHtml.AppendHtml(select);
            return group;
        }

        private static TagBuilder CreateErrorMessage(FieldState fieldState)
        {
            var errorMessage = new TagBuilder("p");
            errorMessage.AddCssClass("govuk-error-message");
            errorMessage.Attributes.Add("id", $"{fieldState.Name}-error");

            var errorMessageSpan = new TagBuilder("span");
            errorMessageSpan.AddCssClass("govuk-visually-hidden");
            errorMessageSpan.InnerHtml.Append("Error:");
            errorMessage.InnerHtml.AppendHtml(errorMessageSpan);
            errorMessage.InnerHtml.Append(fieldState.Errors[0].ErrorMessage);
            return errorMessage;
        }

        private sealed record FieldState(string Name, string FieldKey)
        {
            public string? AttemptedValue { get; init; }
            public ModelErrorCollection Errors { get; init; } = [];

            public bool HasErrors => Errors is { Count: > 0 };
        }

        private FieldState GetFieldState(string prefix, string propertyName, string fieldKey, ModelStateDictionary modelState)
        {
            var fullName = $"{prefix}.{propertyName}";

            if (!modelState.TryGetValue(fullName, out var entry))
            {
                return new FieldState(fullName, fieldKey);
            }

            return new FieldState(fullName, fieldKey)
            {
                AttemptedValue = entry.AttemptedValue,
                Errors = entry.Errors
            };
        }

        internal static class AddressFieldNames
        {
            public const string AddressLine1 = "addressLine1";
            public const string AddressLine2 = "addressLine2";
            public const string AddressLine3 = "addressLine3";
            public const string PostTown = "postTown";
            public const string PostCounty = "postCounty";
            public const string CountryId = "countryId";
            public const string Postcode = "postcode";
            public const string UprnReference = "uprnReference";
        }
    }
}