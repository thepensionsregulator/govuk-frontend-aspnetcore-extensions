using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Tests.HtmlGeneration;

public class ComponentGeneratorTprAddressLookupTests
{
    private readonly ITprHtmlGenerator _componentGenerator;
    private readonly TprAddressLookupContent _content;
    private readonly TprAddressLookupSettings _settings;
    private readonly IDictionary<string, int> _countries = new Dictionary<string, int>
    {
        { "GB", 1 },
        { "US", 2 },
        { "CA", 3 }
    };
    const string MODEL_FOR = "test";
    private readonly ModelStateDictionary _modelStateDictionary = new ModelStateDictionary();
    private readonly ModelExpression _modelExpression;
    private readonly TprAddress _address;

    public ComponentGeneratorTprAddressLookupTests()
    {
        _content = new();
        _settings = new();
        var metadataProvider = new EmptyModelMetadataProvider();
        _address = new TprAddress();
        var modelExplorer = new ModelExplorer(metadataProvider, metadataProvider.GetMetadataForType(typeof(TprAddress)), _address);
        _modelExpression = new ModelExpression("test", modelExplorer);
        _componentGenerator = new ComponentGenerator();
    }

    [Fact]
    public void GenerateTprAddressLookupComponent_Contains_A_FieldSet_With_Legend_With_Correct_Styling_And_Text_Applied()
    {
        // Arrange
        const string exampleLegend = "Address details";
        _content.AddressLookupLegend = exampleLegend;
        // Act
        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);

        // Assert
        var fieldset = document.DocumentNode.SelectSingleNode("/div/fieldset");
        Assert.NotNull(fieldset);
        AssertCssClass("govuk-fieldset", fieldset);

        var legend = fieldset.SelectSingleNode("./legend");
        Assert.NotNull(legend);
        Assert.Multiple(
            () => AssertCssClass("govuk-fieldset__legend", legend),
            () => AssertCssClass("govuk-fieldset__legend--for-fieldset", legend),
            () => Assert.Equal(exampleLegend, HtmlEntity.DeEntitize(legend.InnerText))
        );
    }

    [Fact]
    public void GenerateTprAddressLookupComponent_GeneratesTextInputsWithCorrectAttributes()
    {
        // Arrange
        const string addressLine1Label = "Address line 1";
        _content.AddressLine1Label = addressLine1Label;
        const string addressLine2Label = "Address line 2";
        _content.AddressLine2Label = addressLine2Label;
        const string addressLine3Label = "Address line 3";
        _content.AddressLine3Label = addressLine3Label;
        const string townLabel = "Town";
        _content.PostTownLabel = townLabel;
        const string countyLabel = "County (optional)";
        _content.CountyLabel = countyLabel;
        const string postcodeLabel = "Postcode";
        _content.PostcodeLabel = postcodeLabel;

        // Act
        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);

        // Assert
        Assert.Multiple(
            () => AssertInputRenderedCorrectly(addressLine1Label, $"{MODEL_FOR}.AddressLine1", "govuk-input--width-20", "address-line1", "addressLine1", document),
            () => AssertInputRenderedCorrectly(addressLine2Label, $"{MODEL_FOR}.AddressLine2", "govuk-input--width-20", "address-line2", "addressLine2", document),
            () => AssertInputRenderedCorrectly(addressLine3Label, $"{MODEL_FOR}.AddressLine3", "govuk-input--width-20", "address-line3", "addressLine3", document),
            () => AssertInputRenderedCorrectly(townLabel, $"{MODEL_FOR}.PostTown", "govuk-input--width-10", "address-level2", "postTown", document),
            () => AssertInputRenderedCorrectly(countyLabel, $"{MODEL_FOR}.PostCounty", "govuk-input--width-10", "address-level1", "postCounty", document),
            () => AssertInputRenderedCorrectly(postcodeLabel, $"{MODEL_FOR}.Postcode", "govuk-input--width-10", "postal-code", "postcode", document)
        );
    }

    [Fact]
    public void GenerateTprAddressLookupComponent_UsesNativeRequiredValidationWithoutJQueryValidationAttributes()
    {
        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);
        var textInputs = document.DocumentNode.SelectNodes("//input[@type='text']");

        Assert.NotNull(textInputs);
        Assert.All(textInputs, input =>
        {
            Assert.Null(input.Attributes["data-val"]);
            Assert.Null(input.Attributes["data-val-required"]);
        });

        var requiredInput = document.GetElementbyId($"{MODEL_FOR}.{nameof(TprAddress.AddressLine1)}");
        Assert.NotNull(requiredInput);
        Assert.NotNull(requiredInput.Attributes["required"]);
        AssertAttribute("true", requiredInput, "aria-required");

        var optionalInput = document.GetElementbyId($"{MODEL_FOR}.{nameof(TprAddress.AddressLine2)}");
        Assert.NotNull(optionalInput);
        Assert.Null(optionalInput.Attributes["required"]);
    }

    [Fact]
    public void GenerateTprAddressLookupComponent_UsesModelValuesWhenModelStateHasNoEntry()
    {
        // Arrange
        _address.AddressLine1 = "Address from view model";
        _address.CountryId = 2;
        _address.Uprn = "UPRN123";

        // Act
        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);

        // Assert
        var addressLine1 = document.GetElementbyId($"{MODEL_FOR}.{nameof(TprAddress.AddressLine1)}");
        Assert.NotNull(addressLine1);
        AssertAttribute("Address from view model", addressLine1, "value");

        var selectedCountry = document.DocumentNode.SelectSingleNode($"//select[@id='{MODEL_FOR}.{nameof(TprAddress.CountryId)}']/option[@value='2']");
        Assert.NotNull(selectedCountry);
        AssertAttribute("selected", selectedCountry, "selected");

        var uprn = document.GetElementbyId("uprnReference");
        Assert.NotNull(uprn);
        AssertAttribute("UPRN123", uprn, "value");
    }

    [Fact]
    public void GenerateTprAddressLookupComponent_PrefersModelStateValueOverModelValue()
    {
        // Arrange
        _address.AddressLine1 = "Address from view model";
        _modelStateDictionary.SetModelValue($"{MODEL_FOR}.{nameof(TprAddress.AddressLine1)}", "Address from request", "Address from request");

        // Act
        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);

        // Assert
        var addressLine1 = document.GetElementbyId($"{MODEL_FOR}.{nameof(TprAddress.AddressLine1)}");
        Assert.NotNull(addressLine1);
        AssertAttribute("Address from request", addressLine1, "value");
    }

    [Theory]
    [InlineData(nameof(TprAddress.AddressLine1), "govuk-input--error")]
    [InlineData(nameof(TprAddress.CountryId), "govuk-select--error")]
    public void GenerateTprAddressLookupComponent_RendersFieldErrors(string propertyName, string errorClass)
    {
        var fieldName = $"{MODEL_FOR}.{propertyName}";
        const string errorText = "Choose a value <required>";
        _modelStateDictionary.AddModelError(fieldName, errorText);
        _modelStateDictionary.AddModelError(fieldName, "Another error");

        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);

        var input = document.GetElementbyId(fieldName);
        Assert.NotNull(input);
        AssertCssClass(errorClass, input);
        AssertAttribute($"{fieldName}-error", input, "aria-describedby");
        AssertCssClass("govuk-form-group--error", input.ParentNode);

        var errorMessage = input.ParentNode.SelectSingleNode("./p[@class='govuk-error-message']");
        Assert.NotNull(errorMessage);
        AssertAttribute($"{fieldName}-error", errorMessage, "id");
        Assert.Equal("Error:" + errorText, HtmlEntity.DeEntitize(errorMessage.InnerText));
        Assert.Equal("label", errorMessage.PreviousSibling.Name);
        Assert.Same(input, errorMessage.NextSibling);

        var hiddenPrefix = errorMessage.SelectSingleNode("./span");
        Assert.NotNull(hiddenPrefix);
        AssertCssClass("govuk-visually-hidden", hiddenPrefix);
        Assert.Equal("Error:", hiddenPrefix.InnerText);
        Assert.Single(errorMessage.ChildNodes, node => node.NodeType == HtmlNodeType.Element);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GenerateTprAddressLookupComponent_PreservesCountrySelectionWithOrWithoutErrors(bool hasErrors)
    {
        var fieldName = $"{MODEL_FOR}.{nameof(TprAddress.CountryId)}";
        _address.CountryId = 3;
        _modelStateDictionary.SetModelValue(fieldName, "2", "2");
        if (hasErrors)
        {
            _modelStateDictionary.AddModelError(fieldName, "Country is not available");
        }

        var component = _componentGenerator.GenerateTprAddressLookup(_modelExpression, _modelStateDictionary, _content, _settings, _countries);
        var document = Parse(component);

        var select = document.GetElementbyId(fieldName);
        Assert.NotNull(select);
        AssertCssClass("govuk-select", select);
        var selectedOption = Assert.Single(select.SelectNodes("./option[@selected]"));
        AssertAttribute("2", selectedOption, "value");
        Assert.Equal(hasErrors, select.HasClass("govuk-select--error"));
        Assert.Equal(hasErrors, select.ParentNode.HasClass("govuk-form-group--error"));
        Assert.Equal(hasErrors, select.Attributes["aria-describedby"] is not null);
        Assert.Equal(hasErrors, select.ParentNode.SelectSingleNode("./p[@class='govuk-error-message']") is not null);
    }

    private static void AssertInputRenderedCorrectly(string expectedLabelText, string expectedInputId, string expectedWidthClass, string expectedAutocomplete, string expectedAddressField, HtmlDocument document)
    {
        var input = document.GetElementbyId(expectedInputId);
        Assert.NotNull(input);
        Assert.Multiple(
            () => Assert.Equal("input", input.Name),
            () => AssertCssClass("govuk-input", input),
            () => AssertAttribute(expectedInputId, input, "id"),
            () => AssertAttribute(expectedInputId, input, "name"),
            () => AssertAttribute("text", input, "type"),
            () => AssertAttribute(expectedAddressField, input, "data-address-field"),
            () => AssertAttribute(expectedAutocomplete, input, "autocomplete")
        );

        var group = input.ParentNode;
        Assert.Multiple(
            () => Assert.Equal("div", group.Name),
            () => AssertCssClass("govuk-form-group", group),
            () => AssertCssClass(expectedWidthClass, group)
        );

        var label = group.SelectSingleNode("./label");
        Assert.NotNull(label);
        Assert.Multiple(
            () => AssertCssClass("govuk-label", label),
            () => AssertAttribute(expectedInputId, label, "for"),
            () => Assert.Equal(expectedLabelText, HtmlEntity.DeEntitize(label.InnerText))
        );
    }

    private static void AssertAttribute(string expectedValue, HtmlNode node, string name)
    {
        var attribute = node.Attributes[name];
        Assert.NotNull(attribute);
        Assert.Equal(expectedValue, attribute.Value);
    }

    private static void AssertCssClass(string expectedClass, HtmlNode node)
    {
        Assert.NotNull(node.Attributes["class"]);
        Assert.Contains(expectedClass, node.GetClasses());
    }

    private static HtmlDocument Parse(TagBuilder component)
    {
        using var writer = new StringWriter();
        component.WriteTo(writer, HtmlEncoder.Default);
        var document = new HtmlDocument();
        document.LoadHtml(writer.ToString());
        return document;
    }
}

