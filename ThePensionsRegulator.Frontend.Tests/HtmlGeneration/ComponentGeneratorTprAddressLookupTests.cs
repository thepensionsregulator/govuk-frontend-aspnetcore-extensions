using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using ThePensionsRegulator.Frontend.HtmlGeneration;

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

    public ComponentGeneratorTprAddressLookupTests()
    {
        _content = new();
        _settings = new();
        var metadataProvider = new EmptyModelMetadataProvider();
        var modelExplorer = new ModelExplorer(metadataProvider, metadataProvider.GetMetadataForType(typeof(object)), null);
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

