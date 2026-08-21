using System.Text;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Tests.Services
{
    public class TableCsvServiceTests
    {
        private readonly TableCsvService _service = new();

        private static string DecodeCsv(byte[] csvBytes)
        {
            var preamble = Encoding.UTF8.GetPreamble();
            var content = csvBytes[preamble.Length..];
            return Encoding.UTF8.GetString(content);
        }

        [Fact]
        public void Converts_simple_table_to_csv()
        {
            var html = "<table><tr><th>Name</th><th>Age</th></tr><tr><td>Alice</td><td>30</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("Name,Age" + Environment.NewLine + "Alice,30" + Environment.NewLine, csv);
        }

        [Fact]
        public void Returns_only_bom_when_no_table_present()
        {
            var csvBytes = _service.ConvertHtmlTableToCsv("<div>no table here</div>");

            Assert.Equal(Encoding.UTF8.GetPreamble(), csvBytes);
        }

        [Fact]
        public void Does_not_alter_ordinary_cell_values()
        {
            var html = "<table><tr><td>hello</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("hello" + Environment.NewLine, csv);
        }

        [Fact]
        public void Quotes_cell_value_containing_comma()
        {
            var html = "<table><tr><td>hello,world</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("\"hello,world\"" + Environment.NewLine, csv);
        }

        [Fact]
        public void Quotes_cell_value_containing_double_quote_and_escapes_it()
        {
            var html = "<table><tr><td>say \"hello\"</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("\"say \"\"hello\"\"\"" + Environment.NewLine, csv);
        }

        // CSV injection (formula injection, CWE-1236): table HTML/cell content is untrusted
        // input supplied directly by the client, so values that spreadsheet applications
        // interpret as the start of a formula must be neutralised. Mirrors the equivalent
        // client-side test cases for escapeCsvValue in tpr-table-csv-download.tests.js.
        [Theory]
        [InlineData("=SUM(A1)", "'=SUM(A1)")]
        [InlineData("+44 123", "'+44 123")]
        [InlineData("-1", "'-1")]
        [InlineData("@mention", "'@mention")]
        public void Prefixes_formula_triggering_cell_values_to_prevent_csv_injection(string cellValue, string expected)
        {
            var html = $"<table><tr><td>{cellValue}</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal(expected + Environment.NewLine, csv);
        }

        [Fact]
        public void Prefixes_and_quotes_formula_triggering_value_that_also_contains_a_comma()
        {
            var html = "<table><tr><td>=A,B</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("\"'=A,B\"" + Environment.NewLine, csv);
        }

        // Merged cells (colspan/rowspan) are expanded into a rectangular grid so the CSV stays
        // aligned. The value goes in the top-left cell of a span; the remaining spanned positions
        // are emitted as empty fields. Mirrors the client-side tableToCsv test cases.
        [Fact]
        public void Expands_colspan_into_empty_trailing_fields()
        {
            var html = "<table><tr><td colspan=\"2\">Merged</td></tr><tr><td>A</td><td>B</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("Merged," + Environment.NewLine + "A,B" + Environment.NewLine, csv);
        }

        [Fact]
        public void Expands_rowspan_by_carrying_empty_field_into_following_rows()
        {
            var html = "<table><tr><td rowspan=\"2\">R</td><td>B1</td></tr><tr><td>B2</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("R,B1" + Environment.NewLine + ",B2" + Environment.NewLine, csv);
        }

        [Fact]
        public void Expands_rowspan_in_last_column()
        {
            var html = "<table><tr><td>A1</td><td rowspan=\"2\">B</td></tr><tr><td>A2</td></tr></table>";

            var csv = DecodeCsv(_service.ConvertHtmlTableToCsv(html));

            Assert.Equal("A1,B" + Environment.NewLine + "A2," + Environment.NewLine, csv);
        }
    }
}
