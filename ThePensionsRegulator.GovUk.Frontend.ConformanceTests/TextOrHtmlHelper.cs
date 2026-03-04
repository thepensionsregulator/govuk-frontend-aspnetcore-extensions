using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests
{
    public static class TextOrHtmlHelper
    {
        public static IHtmlContent? GetHtmlContent(string? text, string? html)
        {
            if (html is not null)
            {
                return new HtmlString(html);
            }
            else if (text is not null)
            {
                return new HtmlString(HtmlEncoder.Default.Encode(text));
            }
            else
            {
                return null;
            }
        }
    }
}
