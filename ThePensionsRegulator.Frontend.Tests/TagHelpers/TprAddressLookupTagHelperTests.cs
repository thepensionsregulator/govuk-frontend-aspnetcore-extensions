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
            generator.Setup(x => x.GenerateTprAddressLookup(It.IsAny<string>(), It.IsAny<ModelStateDictionary>(), It.IsAny<TprAddressLookupContent>(), It.IsAny<TprAddressLookupSettings>(), It.IsAny<IDictionary<string, int>>()))
                .Callback<string, ModelStateDictionary, TprAddressLookupContent, TprAddressLookupSettings, IDictionary<string, int>>((_, _, content, _, _) => generatedContent = content)
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

    }
}
