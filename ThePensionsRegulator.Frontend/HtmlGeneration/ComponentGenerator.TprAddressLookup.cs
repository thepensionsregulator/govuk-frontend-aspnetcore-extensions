using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public TagBuilder GenerateTprAddressLookup(ModelExpression @for, ModelStateDictionary modelStateDictionary, TprAddressLookupContent content)
        {
            var name = @for.Name;
            var fieldset = new TagBuilder("fieldset");
            fieldset.AddCssClass("govuk-fieldset");

            var legendElement = new TagBuilder("legend");
            legendElement.AddCssClass("govuk-fieldset__legend govuk-fieldset__legend--l");
            legendElement.InnerHtml.Append(content.Legend);
            fieldset.InnerHtml.AppendHtml(legendElement);

            if (content.Hint is not null)
            {
                var hintElement = new TagBuilder("div");
                hintElement.AddCssClass("govuk-hint");
                hintElement.InnerHtml.Append(content.Hint);
                fieldset.InnerHtml.AppendHtml(hintElement);
            }

            var addressLine1 = GetFieldState(name, nameof(TprAddress.AddressLine1), modelStateDictionary);
            var addressLine2 = GetFieldState(name, nameof(TprAddress.AddressLine2), modelStateDictionary);
            var addressLine3 = GetFieldState(name, nameof(TprAddress.AddressLine3), modelStateDictionary);
            var townOrCity = GetFieldState(name, nameof(TprAddress.PostTown), modelStateDictionary);
            var postCountry = GetFieldState(name, nameof(TprAddress.PostCounty), modelStateDictionary);
            var countryId = GetFieldState(name, nameof(TprAddress.CountryId), modelStateDictionary);
            var postcode = GetFieldState(name, nameof(TprAddress.Postcode), modelStateDictionary);

            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine1, content.AddressLine1Label, true, null));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine2, content.AddressLine2Label, false, null));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(addressLine3, content.AddressLine3Label, false, null));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(townOrCity, content.PostTownLabel, true, "govuk-input--width-20"));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(postCountry, content.PostCountyLabel, false, "govuk-input--width-20"));
            fieldset.InnerHtml.AppendHtml(CreateSelectInput($"{name}.CountryId", content.CountryLabel, true, "govuk-input--width-10"));
            fieldset.InnerHtml.AppendHtml(CreateTextInput(postcode, content.PostcodeLabel, false, "govuk-input--width-10"));

            var uprnReference = new TagBuilder("input");
            uprnReference.Attributes.Add("type", "hidden");
            uprnReference.Attributes.Add("id", "uprnReference");
            uprnReference.Attributes.Add("name", $"{name}.uprnReference");
            fieldset.InnerHtml.AppendHtml(uprnReference);

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("tpr-address-lookup");
            //TODO: Add role attribute
            wrapper.Attributes.Add("data-address-lookup-legend", content.Legend);
            wrapper.Attributes.Add("data-address-lookup-hint", content.Hint);
            wrapper.Attributes.Add("data-address-lookup-search-button-text", content.SearchButtonText);
            //wrapper.Attributes.Add("data-address-lookup-error-message", content);
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
            if (required)
            {
                input.Attributes.Add("required", "");
                input.Attributes.Add("aria-required", "true");
                input.Attributes.Add("data-val-required", "required error message");
                input.Attributes.Add("data-val", "true");
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

        private static TagBuilder CreateSelectInput(string name, string labelText, bool required, string widthClass)
        {
            var label = new TagBuilder("label");
            label.AddCssClass("govuk-label");
            label.Attributes.Add("for", name);
            label.InnerHtml.Append(labelText);

            var select = new TagBuilder("select");
            select.AddCssClass("govuk-select");
            select.Attributes.Add("id", name);
            select.Attributes.Add("name", name);
            if (required)
            {
                select.Attributes.Add("required", "");
            }

            var countryOption = new TagBuilder("option");
            countryOption.Attributes.Add("value", "GB");
            countryOption.Attributes.Add("selected", "selected");
            countryOption.InnerHtml.Append("United Kingdom");
            select.InnerHtml.AppendHtml(countryOption);

            var group = new TagBuilder("div");
            group.AddCssClass("govuk-form-group");
            group.AddCssClass(widthClass);
            group.InnerHtml.AppendHtml(label);
            group.InnerHtml.AppendHtml(select);
            return group;
        }

        private sealed record FieldState(string Name)
        {
            public string? AttemptedValue { get; init; }
            public ModelErrorCollection Errors { get; init; } = [];

            public bool HasErrors => Errors is { Count: > 0 };
        }

        private FieldState GetFieldState(string prefix, string propertyName, ModelStateDictionary modelState)
        {
            var fullName = $"{prefix}.{propertyName}";

            if (!modelState.TryGetValue(fullName, out var entry))
            {
                return new FieldState(fullName);
            }

            return new FieldState(fullName)
            {
                AttemptedValue = entry.AttemptedValue,
                Errors = entry.Errors
            };
        }
    }
}