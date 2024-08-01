namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes
{
    /// <summary>
    /// Specifies the format of an Excel workbook.
    /// </summary>
    public class Excel : OfficeOpenXml, IFileTypeValidator
    {
        public Excel() : base("xl/workbook.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx")
        {
        }
    }
}