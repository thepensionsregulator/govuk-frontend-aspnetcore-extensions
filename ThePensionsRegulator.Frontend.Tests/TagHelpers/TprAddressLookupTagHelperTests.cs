using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Models;
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
            generator.Setup(x => x.GenerateTprAddressLookup(It.IsAny<ModelExpression>(), It.IsAny<ModelStateDictionary>(), It.IsAny<TprAddressLookupContent>(), It.IsAny<TprAddressLookupSettings>(), It.IsAny<IDictionary<string, int>>()))
                .Callback<ModelExpression, ModelStateDictionary, TprAddressLookupContent, TprAddressLookupSettings, IDictionary<string, int>>((_, _, content, _, _) => generatedContent = content)
                .Returns(new TagBuilder("div"));

            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int>());

            var metadataProvider = new EmptyModelMetadataProvider();
            var tagHelper = new TprAddressLookupTagHelper(generator.Object, countries.Object)
            {
                For = new ModelExpression("Address", metadataProvider.GetModelExplorerForType(typeof(TprAddress), new TprAddress())),
                Content = new TprAddressLookupContent { FindAddressButton = "From content", AddressSelectErrorMessage = "From content", CountrySelectPlaceholder = "Pick a country" },
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
            Assert.Equal("Pick a country", generatedContent.CountrySelectPlaceholder);
            Assert.Equal(new TprAddressLookupContent().PostcodeLabel, generatedContent.PostcodeLabel);
        }

        [Fact]
        public async Task Resolved_copy_is_rendered_for_fallback_and_exposed_as_data_attributes()
        {
            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int>());
            var metadataProvider = new EmptyModelMetadataProvider();

            var tagHelper = new TprAddressLookupTagHelper(new ComponentGenerator(), countries.Object)
            {
                For = new ModelExpression("Address", metadataProvider.GetModelExplorerForType(typeof(TprAddress), new TprAddress())),
                Content = new TprAddressLookupContent
                {
                    AddressLookupIntroductionHeading = "Introduction heading",
                    AddressLookupIntroductionText = "No-JS introduction text",
                    AddressLookupHint = "Enhanced search hint",
                    ErrorPrefix = "Problem: ",
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
            Assert.Contains("No-JS introduction text", html);
            Assert.DoesNotContain("Enhanced search hint", html);
            Assert.Contains("<legend", html);
            Assert.Contains("No-JS legend", html);
            Assert.Equal("Introduction heading", output.Attributes["data-address-lookup-legend"].Value);
            Assert.Equal("Enhanced search hint", output.Attributes["data-address-lookup-hint"].Value);
            Assert.Equal("No-JS legend", output.Attributes["data-address-lookup-no-js-legend"].Value);
            Assert.Equal("Problem: ", output.Attributes["data-address-lookup-error-prefix"].Value);
            Assert.Equal("Search legend", output.Attributes["data-address-lookup-search-legend"].Value);
        }

        [Fact]
        public async Task Fallback_validation_uses_content_messages_and_preserves_model_state_overrides()
        {
            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int>());
            var tagHelper = new TprAddressLookupTagHelper(new ComponentGenerator(), countries.Object)
            {
                Name = "Address",
                Content = new TprAddressLookupContent
                {
                    AddressLine1RequiredErrorMessage = "Custom address line 1 required",
                    AddressLine1MaxLengthErrorMessage = "Custom address line 1 length",
                    PostTownRequiredErrorMessage = "Custom town required",
                    CountryRequiredErrorMessage = "Custom country required"
                },
                ViewContext = new ViewContext { ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) }
            };
            tagHelper.ViewContext.ModelState.AddModelError("Address.CountryId", "Country ModelState override");

            var context = new TagHelperContext([], new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput(TprAddressLookupTagHelper.TagName, [], (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            await tagHelper.ProcessAsync(context, output);

            var html = output.Content.GetContent();
            Assert.Contains("data-val-required=\"Custom address line 1 required\"", html);
            Assert.Contains("data-val-length=\"Custom address line 1 length\"", html);
            Assert.Contains("data-val-required=\"Custom town required\"", html);
            Assert.Contains("data-val-required=\"Custom country required\"", html);
            Assert.Contains("Country ModelState override", html);
            Assert.Contains("govuk-select--error", html);
            Assert.Contains("aria-describedby=\"Address.CountryId-error\"", html);
        }

        [Fact]
        public async Task Name_and_value_use_model_state_before_address_values()
        {
            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int> { ["United Kingdom"] = 1, ["France"] = 2 });
            var address = new Mock<ITprAddress>();
            address.SetupGet(x => x.AddressLine1).Returns("Existing address");
            address.SetupGet(x => x.AddressLine2).Returns("Existing second line");
            address.SetupGet(x => x.PostTown).Returns("Existing town");
            address.SetupGet(x => x.CountryId).Returns(1);
            address.SetupGet(x => x.Uprn).Returns("123");
            var tagHelper = new TprAddressLookupTagHelper(new ComponentGenerator(), countries.Object)
            {
                Name = "Addresses[0].Shipping",
                Value = address.Object,
                ViewContext = new ViewContext { ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) }
            };
            tagHelper.ViewContext.ModelState.SetModelValue("Addresses[0].Shipping.AddressLine1", "Submitted address", "Submitted address");
            tagHelper.ViewContext.ModelState.SetModelValue("Addresses[0].Shipping.AddressLine2", "", "");
            tagHelper.ViewContext.ModelState.SetModelValue("Addresses[0].Shipping.CountryId", "2", "2");
            tagHelper.ViewContext.ModelState.AddModelError("Addresses[0].Shipping.CountryId", "Invalid country");
            var context = new TagHelperContext([], new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput(TprAddressLookupTagHelper.TagName, [], (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            await tagHelper.ProcessAsync(context, output);

            var html = output.Content.GetContent();
            Assert.Contains("name=\"Addresses[0].Shipping.AddressLine1\"", html);
            Assert.Contains("value=\"Submitted address\"", html);
            Assert.DoesNotContain("Existing address", html);
            Assert.DoesNotContain("Existing second line", html);
            Assert.Contains("value=\"Existing town\"", html);
            Assert.Contains("value=\"123\"", html);
            Assert.Contains("selected=\"selected\" value=\"2\"", html);
            Assert.Contains("Invalid country", html);
        }

        [Fact]
        public async Task Name_without_value_uses_model_state()
        {
            var countries = new Mock<ITprCountryRepository>();
            countries.Setup(x => x.GetCountries()).ReturnsAsync(new Dictionary<string, int>());
            var tagHelper = new TprAddressLookupTagHelper(new ComponentGenerator(), countries.Object)
            {
                Name = "Address",
                ViewContext = new ViewContext { ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) }
            };
            tagHelper.ViewContext.ModelState.SetModelValue("Address.AddressLine1", "Submitted address", "Submitted address");
            var context = new TagHelperContext([], new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput(TprAddressLookupTagHelper.TagName, [], (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            await tagHelper.ProcessAsync(context, output);

            Assert.Contains("name=\"Address.AddressLine1\"", output.Content.GetContent());
            Assert.Contains("value=\"Submitted address\"", output.Content.GetContent());
        }

        [Theory]
        [InlineData(false, null, false)]
        [InlineData(false, " ", false)]
        [InlineData(true, "Address", false)]
        [InlineData(true, null, true)]
        public async Task Invalid_attribute_combinations_are_rejected(bool hasFor, string? name, bool hasValue)
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var tagHelper = new TprAddressLookupTagHelper(Mock.Of<ITprCountryRepository>())
            {
                For = hasFor ? new ModelExpression("Address", metadataProvider.GetModelExplorerForType(typeof(TprAddress), new TprAddress())) : null,
                Name = name,
                Value = hasValue ? new TprAddress() : null
            };
            var context = new TagHelperContext([], new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput(TprAddressLookupTagHelper.TagName, [], (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            await Assert.ThrowsAsync<InvalidOperationException>(() => tagHelper.ProcessAsync(context, output));
        }

    }
}
