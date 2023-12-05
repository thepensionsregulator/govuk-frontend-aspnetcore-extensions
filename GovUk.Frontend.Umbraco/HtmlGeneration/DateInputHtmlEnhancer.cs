using HtmlAgilityPack;

namespace GovUk.Frontend.Umbraco.HtmlGeneration
{
    public class DateInputHtmlEnhancer : IDateInputHtmlEnhancer
    {
        public string EnhanceHtml(string html, bool dayEnabled)
        {
            if (dayEnabled) { return html; }

            var document = new HtmlDocument();
            document.LoadHtml(html);


            var wrapperForDayInput = document.DocumentNode.SelectSingleNode($"//div[{WithClass("govuk-date-input__item")} and div[{WithClass("govuk-form-group")} and input[{EndsWith("@id", ".Day")}]]]");

            if (wrapperForDayInput is not null)
            {
                wrapperForDayInput.ParentNode.RemoveChild(wrapperForDayInput);
            }

            return document.DocumentNode.OuterHtml;
        }

        private string WithClass(string className) => $"contains(concat(' ',normalize-space(@class),' '),' {className} ')";
        private string EndsWith(string searchWithin, string endsWith) => $"substring({searchWithin}, string-length({searchWithin}) - string-length('{endsWith}') + 1) = '{endsWith}'";
    }
}
