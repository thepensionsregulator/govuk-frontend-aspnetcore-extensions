using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend.Tests.TagHelpers
{
    public class TprAddressLookupTagHelperTests
    {
        [Fact]
        public async Task Content_is_passed_to_generator()
        {
            // Arrange
            TprAddressLookupContent? generatedContent = null;
            var generator = new Mock<ITprHtmlGenerator>();
            generator.Setup(x => x.GenerateTprAddressLookupIntroduction(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<TprAddressLookupContent>()))
                .Returns(new TagBuilder("div"));
            generator.Setup(x => x.GenerateTprAddressLookup(It.IsAny<string>(), It.IsAny<ModelStateDictionary>(), It.IsAny<TprAddressLookupContent>(), It.IsAny<IDictionary<string, int>>()))
                .Callback<string, ModelStateDictionary, TprAddressLookupContent, IDictionary<string, int>>((_, _, content, _) => generatedContent = content)
                .Returns(new TagBuilder("div"));

            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int>());

            var tagHelper = new TprAddressLookupTagHelper(generator.Object, countries.Object)
            {
                For = "Address",
                Content = new TprAddressLookupContent { FindAddressButton = "From content", AddressSelectErrorMessage = "From content" },
                ViewContext = new ViewContext { ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) }
            };

            var context = new TagHelperContext([], new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput(TprAddressLookupTagHelper.TagName, [], (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.NotNull(generatedContent);
            Assert.Equal("From content", generatedContent.FindAddressButton);
            Assert.Equal("From content", generatedContent.AddressSelectErrorMessage);
            Assert.Equal(new TprAddressLookupContent().PostcodeLabel, generatedContent.PostcodeLabel);
        }

        [Fact]
        public async Task Resolved_copy_is_rendered_for_fallback_and_exposed_as_data_attributes()
        {
            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int>());

            var tagHelper = new TprAddressLookupTagHelper(new ComponentGenerator(), countries.Object)
            {
                For = "Address",
                Content = new TprAddressLookupContent
                {
                    AddressLookupIntroductionHeading = "Introduction heading",
                    AddressLookupIntroductionText = "Introduction text",
                    NoJsLegend = "No-JS legend",
                    SearchLegend = "Search legend"
                },
                ViewContext = new ViewContext { ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) }
            };

            var context = new TagHelperContext([], new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput(TprAddressLookupTagHelper.TagName, [], (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            await tagHelper.ProcessAsync(context, output);

            var html = output.Content.GetContent();
            Assert.Contains("<h2", html);
            Assert.Contains("Introduction heading", html);
            Assert.Contains("Introduction text", html);
            Assert.Contains("<legend", html);
            Assert.Contains("No-JS legend", html);
            Assert.Equal("Introduction heading", output.Attributes["data-address-lookup-legend"].Value);
            Assert.Equal("Introduction text", output.Attributes["data-address-lookup-hint"].Value);
            Assert.Equal("No-JS legend", output.Attributes["data-address-lookup-no-js-legend"].Value);
            Assert.Equal("Search legend", output.Attributes["data-address-lookup-search-legend"].Value);
        }

    }
}
