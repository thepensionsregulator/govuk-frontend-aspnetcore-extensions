namespace ThePensionsRegulator.Frontend.Services;

/// <summary>
/// Converts HTML table elements to CSV format.
/// </summary>
public interface ITableCsvService
{
    /// <summary>
    /// Converts an HTML table to CSV bytes with UTF-8 BOM.
    /// </summary>
    /// <param name="tableHtml">The table element HTML.</param>
    /// <returns>CSV content as bytes with UTF-8 BOM.</returns>
    byte[] ConvertHtmlTableToCsv(string tableHtml);
}
