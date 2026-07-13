using HtmlAgilityPack;
using System.Web;

namespace ThePensionsRegulator.Frontend.Services;

/// <summary>
/// Generates and injects CSV download forms into HTML table elements for no-JS support.
/// </summary>
public class TableHtmlCsvFormGenerator
{
    private const string DEFAULT_BUTTON_TEXT = "Download table data (CSV)";

    /// <summary>
    /// Adds a CSV download form after each table in the HTML, with anti-forgery token.
    /// </summary>
    /// <param name="html">The HTML content containing tables.</param>
    /// <param name="antiForgeryToken">The anti-forgery token to include in the form.</param>
    /// <param name="buttonText">Optional text for the download button.</param>
    /// <returns>HTML with download forms injected after each table.</returns>
    public string AddCsvDownloadForms(string html, string antiForgeryToken, string? buttonText = null)
    {
        if (string.IsNullOrWhiteSpace(html))
            return html;

        var resolvedButtonText = string.IsNullOrWhiteSpace(buttonText) ? DEFAULT_BUTTON_TEXT : buttonText;

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var tables = doc.DocumentNode.SelectNodes("//table")?.ToList();
        if (tables == null || tables.Count == 0)
            return html;

        foreach (var table in tables)
        {
            var caption = table.SelectSingleNode(".//caption")?.InnerText?.Trim();
            var fileName = !string.IsNullOrEmpty(caption) ? caption : "table-data";

            var wrapper = doc.CreateElement("div");
            wrapper.SetAttributeValue("class", "tpr-table-wrapper");

            var form = doc.CreateElement("form");
            form.SetAttributeValue("method", "post");
            form.SetAttributeValue("action", "/api/table/download-csv");
            form.SetAttributeValue("class", "tpr-table-download-form");

            var tableInput = doc.CreateElement("input");
            tableInput.SetAttributeValue("type", "hidden");
            tableInput.SetAttributeValue("name", "tableHtml");
            tableInput.SetAttributeValue("value", HttpUtility.HtmlEncode(table.OuterHtml));
            form.AppendChild(tableInput);

            var fileNameInput = doc.CreateElement("input");
            fileNameInput.SetAttributeValue("type", "hidden");
            fileNameInput.SetAttributeValue("name", "fileName");
            fileNameInput.SetAttributeValue("value", HttpUtility.HtmlEncode(fileName));
            form.AppendChild(fileNameInput);

            var tokenInput = doc.CreateElement("input");
            tokenInput.SetAttributeValue("type", "hidden");
            tokenInput.SetAttributeValue("name", "__RequestVerificationToken");
            tokenInput.SetAttributeValue("value", antiForgeryToken);
            form.AppendChild(tokenInput);

            var submitButton = doc.CreateElement("button");
            submitButton.SetAttributeValue("type", "submit");
            submitButton.SetAttributeValue("class", "govuk-button govuk-button--secondary");
            submitButton.SetAttributeValue("data-module", "govuk-button");
            submitButton.InnerHtml = HttpUtility.HtmlEncode(resolvedButtonText);
            form.AppendChild(submitButton);

            table.ParentNode.InsertBefore(wrapper, table);
            table.Remove();
            wrapper.AppendChild(table);
            wrapper.AppendChild(form);
        }

        return doc.DocumentNode.OuterHtml;
    }
}
