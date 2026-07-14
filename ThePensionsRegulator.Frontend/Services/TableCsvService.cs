using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace ThePensionsRegulator.Frontend.Services;

public class TableCsvService : ITableCsvService
{
    public byte[] ConvertHtmlTableToCsv(string tableHtml)
    {
        ArgumentNullException.ThrowIfNull(tableHtml);

        var doc = new HtmlDocument();
        doc.LoadHtml(tableHtml);

        var table = doc.DocumentNode.SelectSingleNode("//table");
        if (table == null)
        {
            return Encoding.UTF8.GetPreamble();
        }

        var csvBuilder = new StringBuilder();
        var rows = table.SelectNodes(".//tr");

        if (rows != null)
        {
            foreach (var row in rows)
            {
                var cells = row.SelectNodes("th|td");
                if (cells != null)
                {
                    var cellValues = cells.Select(cell => EscapeCsvValue(GetCellText(cell)));
                    csvBuilder.AppendLine(string.Join(",", cellValues));
                }
            }
        }

        var bom = Encoding.UTF8.GetPreamble();
        var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        return [.. bom, .. csvBytes];
    }

    private static string GetCellText(HtmlNode cell)
    {
        var text = cell.InnerText;
        text = Regex.Replace(text, @"\s+", " ");
        return text.Trim();
    }

    private static string EscapeCsvValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        // CSV injection (formula injection) mitigation: if the value starts with a character
        // that spreadsheet applications (Excel, Google Sheets, LibreOffice) interpret as the
        // start of a formula, prefix it with a single quote so it is opened as literal text
        // instead of being executed. This mirrors the client-side escapeCsvValue in
        // tpr-table-csv-download.js, since tableHtml/cell content here is untrusted input
        // supplied directly by the client.
        if (value[0] is '=' or '+' or '-' or '@' or '\t' or '\r')
        {
            value = "'" + value;
        }

        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}
