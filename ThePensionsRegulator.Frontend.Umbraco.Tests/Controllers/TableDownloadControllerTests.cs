using Microsoft.AspNetCore.Mvc;
using System.Text;
using ThePensionsRegulator.Frontend.Controllers;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Controllers
{
    public class TableDownloadControllerTests
    {
        private sealed class StubTableCsvService : ITableCsvService
        {
            public byte[] ConvertHtmlTableToCsv(string tableHtml) => Encoding.UTF8.GetBytes("stub-csv-content");
        }

        private static TableDownloadController CreateController() => new(new StubTableCsvService());

        [Fact]
        public void Missing_table_html_returns_bad_request()
        {
            var controller = CreateController();

            var result = controller.DownloadCsv(string.Empty, "report");

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Response_uses_csv_content_type_and_content_from_service()
        {
            var controller = CreateController();

            var result = controller.DownloadCsv("<table></table>", "report");

            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal(Encoding.UTF8.GetBytes("stub-csv-content"), fileResult.FileContents);
        }

        [Fact]
        public void Missing_file_name_defaults_to_table_data()
        {
            var controller = CreateController();

            var result = controller.DownloadCsv("<table></table>", null);

            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("table-data.csv", fileResult.FileDownloadName);
        }

        [Fact]
        public void File_name_is_sanitized()
        {
            var controller = CreateController();

            var result = controller.DownloadCsv("<table></table>", "My Report!!");

            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("my-report.csv", fileResult.FileDownloadName);
        }

        [Fact]
        public void File_name_defaults_to_table_data_when_sanitizing_removes_everything()
        {
            var controller = CreateController();

            var result = controller.DownloadCsv("<table></table>", "###");

            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("table-data.csv", fileResult.FileDownloadName);
        }
    }
}
