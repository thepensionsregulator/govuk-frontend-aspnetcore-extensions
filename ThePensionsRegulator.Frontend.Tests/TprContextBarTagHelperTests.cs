using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using System.Text.Encodings.Web;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend.Tests
{
    [TestFixture]
    public class TprContextBarTagHelperTests
    {
        [Test]
        public async Task All_3_contexts_empty_does_not_render_bar()
        {
            // Arrange
            var tagHelper = new TprContextBarTagHelper();
            var attributes = new TagHelperAttributeList();
            var context = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("tpr-context-bar", attributes, (result, encoder) =>
            {
                return Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());
            });

            // Act
            await tagHelper.ProcessAsync(context, output);
            using var result = new StringWriter();
            output.WriteTo(result, HtmlEncoder.Create());

            // Assert
            Assert.True(string.IsNullOrEmpty(result.ToString()));
        }

        [Test]
        public async Task Context_1_content_is_rendered()
        {
            // Arrange
            var htmlGenerator = new Mock<ITprHtmlGenerator>();
            htmlGenerator.Setup(x => x.GenerateTprContextBar(It.IsAny<TprContextBar>())).Returns(new TagBuilder(TprContextBarTagHelper.TagName));
            const string CONTEXT_1_CONTENT = "<tpr-context-bar-context-1>Example</tpr-context-bar-context-1>";

            var tagHelper = new TprContextBarTagHelper(htmlGenerator.Object);
            var attributes = new TagHelperAttributeList();
            var tagHelperContext = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("tpr-context-bar", attributes, (result, encoder) =>
            {
                var contextBarContext = (TprContextBarContext)tagHelperContext.Items[typeof(TprContextBarContext)];
                contextBarContext.SetContext(1, new AttributeDictionary(), new HtmlString(CONTEXT_1_CONTENT), false);

                return Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());
            });

            // Act
            await tagHelper.ProcessAsync(tagHelperContext, output);

            // Assert
            htmlGenerator.Verify(x => x.GenerateTprContextBar(It.Is<TprContextBar>(bar => bar.Context1Content != null && bar.Context1Content.ToHtmlString() == CONTEXT_1_CONTENT)), Times.Once);
        }

        [Test]
        public async Task Context_2_content_is_rendered()
        {
            // Arrange
            var htmlGenerator = new Mock<ITprHtmlGenerator>();
            htmlGenerator.Setup(x => x.GenerateTprContextBar(It.IsAny<TprContextBar>())).Returns(new TagBuilder(TprContextBarTagHelper.TagName));
            const string CONTEXT_2_CONTENT = "<tpr-context-bar-context-2>Example</tpr-context-bar-context-2>";

            var tagHelper = new TprContextBarTagHelper(htmlGenerator.Object);
            var attributes = new TagHelperAttributeList();
            var tagHelperContext = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("tpr-context-bar", attributes, (result, encoder) =>
            {
                var contextBarContext = (TprContextBarContext)tagHelperContext.Items[typeof(TprContextBarContext)];
                contextBarContext.SetContext(2, new AttributeDictionary(), new HtmlString(CONTEXT_2_CONTENT), false);

                return Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());
            });

            // Act
            await tagHelper.ProcessAsync(tagHelperContext, output);

            // Assert
            htmlGenerator.Verify(x => x.GenerateTprContextBar(It.Is<TprContextBar>(bar => bar.Context2Content != null && bar.Context2Content.ToHtmlString() == CONTEXT_2_CONTENT)), Times.Once);
        }

        [Test]
        public async Task Context_3_content_is_rendered()
        {
            // Arrange
            var htmlGenerator = new Mock<ITprHtmlGenerator>();
            htmlGenerator.Setup(x => x.GenerateTprContextBar(It.IsAny<TprContextBar>())).Returns(new TagBuilder(TprContextBarTagHelper.TagName));
            const string CONTEXT_3_CONTENT = "<tpr-context-bar-context-3>Example</tpr-context-bar-context-3>";

            var tagHelper = new TprContextBarTagHelper(htmlGenerator.Object);
            var attributes = new TagHelperAttributeList();
            var tagHelperContext = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());
            var output = new TagHelperOutput("tpr-context-bar", attributes, (result, encoder) =>
            {
                var contextBarContext = (TprContextBarContext)tagHelperContext.Items[typeof(TprContextBarContext)];
                contextBarContext.SetContext(3, new AttributeDictionary(), new HtmlString(CONTEXT_3_CONTENT), false);

                return Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());
            });

            // Act
            await tagHelper.ProcessAsync(tagHelperContext, output);

            // Assert
            htmlGenerator.Verify(x => x.GenerateTprContextBar(It.Is<TprContextBar>(bar => bar.Context3Content != null && bar.Context3Content.ToHtmlString() == CONTEXT_3_CONTENT)), Times.Once);
        }
    }
}
