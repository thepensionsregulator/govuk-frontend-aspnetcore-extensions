using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public TagBuilder GenerateTprAddressLookup(string @for, ModelStateDictionary modelStateDictionary, TprAddressLookupContent content, TprAddressLookupSettings settings, IDictionary<string, int> countries)
        {
            var name = @for;
            var fieldset = new TagBuilder("fieldset");
            fieldset.AddCssClass("govuk-fieldset");

            var legendElement = new TagBuilder("legend");
            legendElement.AddCssClass("govuk-fieldset__legend govuk-fieldset__legend--l");
            legendElement.InnerHtml.Append(content.AddressLookupLegend);
            fieldset.InnerHtml.AppendHtml(legendElement);

            if (content.AddressLookupHint is not null)
            {
                var hintElement = new TagBuilder("div");
                hintElement.AddCssClass("govuk-hint");
                hintElement.InnerHtml.Append(content.AddressLookupHint);
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

            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine1, content.AddressLine1Label, true, null));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine2, content.AddressLine2Label, false, null));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine3, content.AddressLine3Label, false, null));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(townOrCity, content.PostTownLabel, true, "govuk-input--width-20"));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(postCountry, content.CountyLabel, false, "govuk-input--width-20"));
            fieldset.InnerHtml.AppendHtml(CreateSelectInput(countryId, content.CountryLabel, content.CountryLabel, true, "govuk-input--width-10", countries));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(postcode, content.PostcodeLabel, false, "govuk-input--width-10"));

            var uprnReferenceInput = new TagBuilder("input");
            uprnReferenceInput.Attributes.Add("type", "hidden");
            uprnReferenceInput.Attributes.Add("id", "uprnReference");
            uprnReferenceInput.Attributes.Add("name", $"{name}.uprnReference");
            uprnReferenceInput.Attributes.Add("data-address-field", uprnReference.FieldKey);
            fieldset.InnerHtml.AppendHtml(uprnReferenceInput);

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("tpr-address-lookup");
            //TODO: Add role attribute
            wrapper.Attributes.Add("data-address-lookup-search-url", settings.SearchUrl);
            wrapper.Attributes.Add("data-address-lookup-legend", content.AddressLookupLegend);
            wrapper.Attributes.Add("data-address-lookup-hint", content.AddressLookupHint);
            wrapper.Attributes.Add("data-address-lookup-address-line-1-label", content.AddressLine1Label);
            wrapper.Attributes.Add("data-address-lookup-address-line-1-required-error-message", content.AddressLine1RequiredErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-address-line-1-max-length-error-message", content.AddressLine1MaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-address-line-2-label", content.AddressLine2Label);
            wrapper.Attributes.Add("data-address-lookup-address-line-2-max-length-error-message", content.AddressLine2MaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-address-line-3-label", content.AddressLine3Label);
            wrapper.Attributes.Add("data-address-lookup-address-line-3-max-length-error-message", content.AddressLine3MaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-post-town-label", content.PostTownLabel);
            wrapper.Attributes.Add("data-address-lookup-post-town-required-error-message", content.PostTownRequiredErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-post-town-max-length-error-message", content.PostTownMaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-county-label", content.CountyLabel);
            wrapper.Attributes.Add("data-address-lookup-county-max-length-error-message", content.CountyMaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-county-state-province-label", content.CountyStateProvinceLabel);
            wrapper.Attributes.Add("data-address-lookup-county-state-province-max-length-error-message", content.CountyStateProvinceMaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-postcode-label", content.PostcodeLabel);
            wrapper.Attributes.Add("data-address-lookup-postcode-required-error-message", content.PostcodeRequiredErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-postcode-max-length-error-message", content.PostcodeMaxLengthErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-postcode-pattern-error-message", content.PostcodePatternErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-country-label", content.CountryLabel);
            wrapper.Attributes.Add("data-address-lookup-country-select-placeholder", content.CountrySelectPlaceholder);
            wrapper.Attributes.Add("data-address-lookup-country-required-error-message", content.CountryRequiredErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-building-name-or-number-label", content.BuildingNameOrNumberLabel);

            // UK manual entry labels
            wrapper.Attributes.Add("data-address-lookup-uk-manual-entry-link-label", content.UkManualEntryLinkLabel);
            wrapper.Attributes.Add("data-address-lookup-uk-manual-entry-legend", content.UkManualEntryLegend);

            // international manual entry labels
            wrapper.Attributes.Add("data-address-lookup-international-manual-entry-label", content.InternationalManualEntryLabel);
            wrapper.Attributes.Add("data-address-lookup-international-manual-entry-legend", content.InternationalManualEntryLegend);

            // search view
            wrapper.Attributes.Add("data-address-lookup-find-address-button", content.FindAddressButton);
            wrapper.Attributes.Add("data-address-lookup-address-not-found-message", content.AddressNotFoundMessage);
            wrapper.Attributes.Add("data-address-lookup-service-unavailable-message", content.ServiceUnavailableMessage);
            wrapper.Attributes.Add("data-address-lookup-search-not-confirmed-error-message", content.SearchNotConfirmedErrorMessage);

            // results view
            wrapper.Attributes.Add("data-address-lookup-address-select-label", content.AddressSelectLabel);
            wrapper.Attributes.Add("data-address-lookup-address-select-error-message", content.AddressSelectErrorMessage);
            wrapper.Attributes.Add("data-address-lookup-confirm-address-label", content.ConfirmAddressLabel);
            wrapper.Attributes.Add("data-address-lookup-back-to-postcode-search-label", content.BackToPostcodeSearchLabel);
            wrapper.Attributes.Add("data-address-lookup-results-not-confirmed-error-message", content.ResultsNotConfirmedErrorMessage);

            // UK manual entry view
            wrapper.Attributes.Add("data-address-lookup-uk-manual-entry-not-confirmed-error-message", content.UkManualEntryNotConfirmedErrorMessage);

            // international manual entry view
            wrapper.Attributes.Add("data-address-lookup-international-manual-entry-not-confirmed-error-message", content.InternationalManualEntryNotConfirmedErrorMessage);

            // confirmed view
            wrapper.Attributes.Add("data-address-lookup-change-address-label", content.ChangeAddressLabel);
            wrapper.InnerHtml.AppendHtml(fieldset);

            return wrapper;
        }

        private static TagBuilder CreateTextInput(FieldState fieldState, string labelText, bool required, string? widthClass = null)
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
                input.Attributes.Add("data-val-required", "required error message");
                input.Attributes.Add("data-val", "true");
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
                var errorMessage = new TagBuilder("p");
                errorMessage.AddCssClass("govuk-error-message");
                errorMessage.Attributes.Add("id", $"{fieldState.Name}-error");

                var errorMessageSpan = new TagBuilder("span");
                errorMessageSpan.AddCssClass("govuk-visually-hidden");
                errorMessageSpan.InnerHtml.Append("Error:");
                errorMessage.InnerHtml.AppendHtml(errorMessageSpan);
                errorMessage.InnerHtml.Append(fieldState.Errors[0].ErrorMessage);
                group.InnerHtml.AppendHtml(errorMessage);
            }

            group.InnerHtml.AppendHtml(input);

            return group;
        }

        private static TagBuilder CreateSelectInput(FieldState fieldState, string labelText, string placeholderText, bool required, string widthClass, IDictionary<string, int> options)
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
            group.InnerHtml.AppendHtml(select);
            return group;
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