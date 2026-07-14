using Microsoft.AspNetCore.Mvc;
using Moq;
using ThePensionsRegulator.Frontend.Controllers;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Tests.Controllers;

public class TableDownloadControllerTests
{
    [Fact]
    public void DownloadCsv_returns_BadRequest_when_tableHtml_is_empty()
    {
        var controller = new TableDownloadController(Mock.Of<ITableCsvService>());

        var result = controller.DownloadCsv("", "my-file");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DownloadCsv_returns_BadRequest_when_tableHtml_exceeds_max_size()
    {
        var controller = new TableDownloadController(Mock.Of<ITableCsvService>());
        var oversizedHtml = "<table>" + new string('a', 310 * 1024) + "</table>";

        var result = controller.DownloadCsv(oversizedHtml, "my-file");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DownloadCsv_returns_BadRequest_when_no_table_exists()
    {
        var controller = new TableDownloadController(Mock.Of<ITableCsvService>());

        var result = controller.DownloadCsv("<div>Not a table</div>", "my-file");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DownloadCsv_returns_BadRequest_when_multiple_tables_exist()
    {
        var controller = new TableDownloadController(Mock.Of<ITableCsvService>());

        var result = controller.DownloadCsv("<table><tr><td>A</td></tr></table><table><tr><td>B</td></tr></table>", "my-file");

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DownloadCsv_returns_file_for_valid_table_html_and_only_passes_table_to_service()
    {
        var csvBytes = new byte[] { 1, 2, 3 };
        var tableCsvService = new Mock<ITableCsvService>();
        tableCsvService
            .Setup(x => x.ConvertHtmlTableToCsv(It.IsAny<string>()))
            .Returns(csvBytes);

        var controller = new TableDownloadController(tableCsvService.Object);
        const string html = "<div><script>alert('x')</script><table><tr><td>A</td></tr></table></div>";

        var result = controller.DownloadCsv(html, "My File");

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);
        Assert.Equal("my-file.csv", fileResult.FileDownloadName);
        Assert.Equal(csvBytes, fileResult.FileContents);

        tableCsvService.Verify(
            x => x.ConvertHtmlTableToCsv(It.Is<string>(postedTable =>
                postedTable.StartsWith("<table", StringComparison.OrdinalIgnoreCase)
                && postedTable.Contains("<td>A</td>", StringComparison.Ordinal)
                && !postedTable.Contains("<script", StringComparison.OrdinalIgnoreCase))),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("!!!")]
    public void DownloadCsv_uses_default_filename_when_sanitized_filename_is_missing_or_invalid(string? postedFileName)
    {
        var tableCsvService = new Mock<ITableCsvService>();
        tableCsvService
            .Setup(x => x.ConvertHtmlTableToCsv(It.IsAny<string>()))
            .Returns([1, 2, 3]);

        var controller = new TableDownloadController(tableCsvService.Object);

        var result = controller.DownloadCsv("<table><tr><td>A</td></tr></table>", postedFileName);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("table-data.csv", fileResult.FileDownloadName);
    }
}
