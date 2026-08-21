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
            // Tracks active rowspans keyed by column index: the number of further rows still to fill.
            var rowspanCarry = new Dictionary<int, int>();

            foreach (var row in rows)
            {
                var cellList = row.SelectNodes("th|td")?.ToList() ?? [];
                var rowData = new List<string>();
                var col = 0;
                var cellIndex = 0;

                // Expand merged cells (colspan/rowspan) into a rectangular grid so the CSV stays
                // aligned. The value is placed in the top-left cell of a span; the remaining
                // spanned positions are emitted as empty fields. This mirrors tableToCsv in the
                // client-side tpr-table-csv-download.js so both paths produce identical output.
                while (cellIndex < cellList.Count || HasCarryAtOrBeyond(rowspanCarry, col))
                {
                    if (rowspanCarry.TryGetValue(col, out var remaining) && remaining > 0)
                    {
                        SetCell(rowData, col, string.Empty);
                        rowspanCarry[col] = remaining - 1;
                        if (rowspanCarry[col] == 0) { rowspanCarry.Remove(col); }
                        col++;
                        continue;
                    }

                    if (cellIndex < cellList.Count)
                    {
                        var cell = cellList[cellIndex++];
                        var colspan = Math.Max(1, ParseSpan(cell, "colspan"));
                        var rowspan = Math.Max(1, ParseSpan(cell, "rowspan"));
                        var text = EscapeCsvValue(GetCellText(cell));

                        for (var c = 0; c < colspan; c++)
                        {
                            SetCell(rowData, col + c, c == 0 ? text : string.Empty);
                            if (rowspan > 1)
                            {
                                rowspanCarry[col + c] = rowspan - 1;
                            }
                        }
                        col += colspan;
                        continue;
                    }

                    // No cell and no carry at this column, but a rowspan carry exists further
                    // right: fill the gap with an empty field to preserve grid alignment.
                    SetCell(rowData, col, string.Empty);
                    col++;
                }

                if (rowData.Count > 0)
                {
                    csvBuilder.AppendLine(string.Join(",", rowData));
                }
            }
        }

        var bom = Encoding.UTF8.GetPreamble();
        var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        return [.. bom, .. csvBytes];
    }

    private static bool HasCarryAtOrBeyond(Dictionary<int, int> rowspanCarry, int col)
    {
        foreach (var carry in rowspanCarry)
        {
            if (carry.Key >= col && carry.Value > 0)
            {
                return true;
            }
        }
        return false;
    }

    private static void SetCell(List<string> row, int index, string value)
    {
        while (row.Count <= index)
        {
            row.Add(string.Empty);
        }
        row[index] = value;
    }

    private static int ParseSpan(HtmlNode cell, string attributeName)
    {
        var raw = cell.GetAttributeValue(attributeName, "1");
        return int.TryParse(raw, out var value) ? value : 1;
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
