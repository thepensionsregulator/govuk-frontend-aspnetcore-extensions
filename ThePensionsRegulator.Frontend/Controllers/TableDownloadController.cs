using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Controllers;

/// <summary>
/// Handles CSV download requests for HTML tables.
/// Provides both server-side fallback for no-JS scenarios and client-side form interception.
/// </summary>
[ApiController]
public class TableDownloadController : ControllerBase
{
    private const int MaxTableHtmlBytes = 300 * 1024;
    private readonly ITableCsvService _tableCsvService;

    public TableDownloadController(ITableCsvService tableCsvService)
    {
        _tableCsvService = tableCsvService;
    }

    /// <summary>
    /// Downloads a table as a CSV file.
    /// Accepts POST from the tpr-table-download-form when JavaScript is unavailable.
    /// </summary>
    /// <param name="tableHtml">The complete table element HTML to convert.</param>
    /// <param name="fileName">The desired filename (optional, defaults to 'table-data').</param>
    /// <returns>CSV file download.</returns>
    [HttpPost]
    [Route("api/table/download-csv")]
    [ValidateAntiForgeryToken]
    public IActionResult DownloadCsv([FromForm] string tableHtml, [FromForm] string? fileName)
    {
        if (string.IsNullOrWhiteSpace(tableHtml))
        {
            return BadRequest("Table HTML is required.");
        }

        if (Encoding.UTF8.GetByteCount(tableHtml) > MaxTableHtmlBytes)
        {
            return BadRequest("Table HTML is too large.");
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(tableHtml);

        var tableNodes = htmlDoc.DocumentNode.SelectNodes("//table");
        if (tableNodes == null || tableNodes.Count == 0)
        {
            return BadRequest("Table HTML must contain a table element.");
        }

        if (tableNodes.Count > 1)
        {
            return BadRequest("Table HTML must contain exactly one table element.");
        }

        var tableOnlyHtml = tableNodes[0].OuterHtml;

        var csvBytes = _tableCsvService.ConvertHtmlTableToCsv(tableOnlyHtml);
        var sanitizedFileName = SanitizeFileName(fileName) ?? "table-data";

        return File(csvBytes, "text/csv", $"{sanitizedFileName}.csv");
    }

    private static string? SanitizeFileName(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        var sanitized = Regex.Replace(fileName.Trim(), @"[^\w\s\-]", "");
        sanitized = Regex.Replace(sanitized, @"\s+", "-").ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(sanitized))
        {
            return null;
        }

        return sanitized.Length > 50 ? sanitized[..50] : sanitized;
    }
}