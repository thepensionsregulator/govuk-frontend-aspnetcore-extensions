using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend.Tests.TagHelpers
{
    public class TprHeaderSearchTagHelperTests
    {
        [Fact]
        public async Task All_values_are_correctly_stored_in_context()
        {
            // Arrange
            var expectedFormId = "form-1";
            var expectedActionPath = "/search";
            var expectedAutoCompleteUrl = "/autocomplete";
            var expectedPlaceholderText = "Search";
            var expectedAriaLabel = "search";
            var expectedInputName = "query";

            var tagHelper = new TprHeaderSearchTagHelper
            {
                FormId = expectedFormId,
                ActionPath = expectedActionPath,
                AutocompleteUrl = expectedAutoCompleteUrl,
                PlaceholderText = expectedPlaceholderText,
                AriaLabel = expectedAriaLabel,
                InputName = expectedInputName
            };

            var attributes = new TagHelperAttributeList { { "data-test", "value" } };

            var headerBarContext = new TprHeaderBarContext();

            var context = new TagHelperContext(attributes, new Dictionary<object, object> { { typeof(TprHeaderBarContext), headerBarContext } }, Guid.NewGuid().ToString());

            var output = new TagHelperOutput("tpr-header-search", attributes, (result, encoder) =>
            {
                return Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());
            });

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.True(headerBarContext.ShowSearch);
            Assert.Equal(expectedFormId, headerBarContext.SearchFormId);
            Assert.Equal(expectedActionPath, headerBarContext.ActionPath);
            Assert.Equal(expectedAutoCompleteUrl, headerBarContext.AutoCompleteUrl);
            Assert.Equal(expectedPlaceholderText, headerBarContext.SearchPlaceholderText);
            Assert.Equal(expectedAriaLabel, headerBarContext.SearchAriaLabel);
            Assert.Equal(expectedInputName, headerBarContext.SearchInputName);
            Assert.NotNull(headerBarContext.SearchAttributes);
            Assert.True(headerBarContext.SearchAttributes.ContainsKey("data-test"));
        }
    }
}
