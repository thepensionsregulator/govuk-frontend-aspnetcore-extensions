using HtmlAgilityPack;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.HtmlGeneration
{
    public class DateInputHtmlEnhancer : IDateInputHtmlEnhancer
    {
        public string EnhanceHtml(string html, bool dayEnabled, bool yearEnabled)
        {
            if (dayEnabled && yearEnabled) { return html; }

            var document = new HtmlDocument();
            document.LoadHtml(html);

            if (!dayEnabled)
            {
                var wrapperForDayInput = document.DocumentNode.SelectSingleNode($"//div[{WithClass("govuk-date-input__item")} and div[{WithClass(GovUkClassNames.FormGroup)} and input[{EndsWith("@id", ".Day")}]]]");
                if (wrapperForDayInput is not null)
                {
                    wrapperForDayInput.ParentNode.RemoveChild(wrapperForDayInput);
                }
            }

            if (!yearEnabled)
            {
                var wrapperForYearInput = document.DocumentNode.SelectSingleNode($"//div[{WithClass("govuk-date-input__item")} and div[{WithClass(GovUkClassNames.FormGroup)} and input[{EndsWith("@id", ".Year")}]]]");
                if (wrapperForYearInput is not null)
                {
                    wrapperForYearInput.ParentNode.RemoveChild(wrapperForYearInput);
                }
            }

            return document.DocumentNode.OuterHtml;
        }

        private string WithClass(string className) => $"contains(concat(' ',normalize-space(@class),' '),' {className} ')";
        private string EndsWith(string searchWithin, string endsWith) => $"substring({searchWithin}, string-length({searchWithin}) - string-length('{endsWith}') + 1) = '{endsWith}'";
    }
}
