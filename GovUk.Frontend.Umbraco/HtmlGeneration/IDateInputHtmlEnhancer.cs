namespace GovUk.Frontend.Umbraco.HtmlGeneration
{
    public interface IDateInputHtmlEnhancer
    {
        string EnhanceHtml(string html, bool dayEnabled);
    }
}