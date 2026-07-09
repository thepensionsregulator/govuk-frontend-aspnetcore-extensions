using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend.Tests.TagHelpers
{
    public class TprDocumentTitleTagHelperTests
    {
        [Theory]
        [InlineData("/media/test.pdf", "pdf", "PDF", "120KB, 4 page(s)")]
        [InlineData("/media/test.doc", "doc", "Word", "120KB, 4 page(s)")]
        [InlineData("/media/test.docx", "doc", "Word", "120KB, 4 page(s)")]
        [InlineData("/media/test.dotx", "doc", "DOTX", "120KB")]
        [InlineData("/media/test.pptx", "powerpoint", "PPTX", "120KB, 4 page(s)")]
        [InlineData("/media/test.rtf", "misc", "RTF", "120KB, 4 page(s)")]
        [InlineData("/media/test.odt", "misc", "ODT", "120KB, 4 page(s)")]
        [InlineData("/media/test.xlsx", "excel", "Excel", "120KB")]
        [InlineData("/media/test.xlst", "excel", "XLST", "120KB")]
        [InlineData("/media/test.csv", "excel", "CSV", "120KB")]    
        public async Task TprDocumentTitleTagHelper_Should_RenderExpected_DocumentMetadata(string href, string expectedIconClass, string expectedFileLabel, string expectedMetaDataText)
        {
            //Act
            var output = await GenerateTagHelper(href, "4");

            var html = RenderHtml(output);

            //Assert
            Assert.Contains("<dt>", html);
            Assert.Contains("govuk-link", html);
            Assert.Contains($"href=\"{href}\"", html);
            Assert.Contains($"<span class=\"{expectedIconClass} fileicon\">{expectedFileLabel}</span>", html);
            Assert.Contains(expectedMetaDataText, html);
            Assert.Contains("download", html);
            Assert.Contains("</dt>", html);
        }

        [Fact]
        public async Task Document_WithZeroPages_DoesNotRender_PageCount()
        { 
            //Act
            var output = await GenerateTagHelper("/media/test.pdf", "0");

            var html = RenderHtml(output);

            //Assert
            Assert.Contains("PDF", html);
            Assert.Contains("120KB", html);
            Assert.DoesNotContain("page(s)", html);
        }

        [Fact]
        public async Task ProcessAsync_WhenHrefIsNull_ThrowsArgumentNullException()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => GenerateTagHelper(null, "4"));
        }

        private static async Task<TagHelperOutput> GenerateTagHelper(string href, string pages)
        {
            var documentContext = new TprDocumentContext
            {
                Href = href,
                KbSize = "120",
                Pages = pages
            };

            var attributes = new TagHelperAttributeList();

            var context = new TagHelperContext(attributes, new Dictionary<object, object> { { typeof(TprDocumentsTagHelper), documentContext } }, Guid.NewGuid().ToString());

            var output = new TagHelperOutput(TprDocumentTitleTagHelper.TagName, attributes, (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

            var tagHelper = new TprDocumentTitleTagHelper();


            await tagHelper.ProcessAsync(context, output);

            return output;
        }

        private static string RenderHtml(TagHelperOutput output)
        {
            using var result = new StringWriter();
            output.WriteTo(result, HtmlEncoder.Create());
            return result.ToString();
        }
    }
}
