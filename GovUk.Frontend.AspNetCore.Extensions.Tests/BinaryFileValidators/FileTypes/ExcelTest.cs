using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using NUnit.Framework;
using System.Collections;
using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests.BinaryFileValidators.FileTypes
{
    internal class ExcelTest
    {
        private string _emptyExcelFileName = TestFileLocator.EmptyExcelFile;
        private string _emptyPowerPointFileName = TestFileLocator.EmptyPowerPoint;

        [Test]
        public void Can_identify_valid_Excel_file()
        {
            var bytes = File.ReadAllBytes(_emptyExcelFileName);
            var excelValidator = new Excel();
            var memoryStream = new MemoryStream(bytes);

            var isValid = excelValidator.IsMatch(memoryStream);
            memoryStream.Dispose();

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void An_OfficeOpenXml_file_that_is_not_Excel_returns_false()
        {
            var bytes = File.ReadAllBytes(_emptyPowerPointFileName);
            var excelValidator = new Excel();
            var memoryStream = new MemoryStream(bytes);

            var isValid = excelValidator.IsMatch(memoryStream);
            memoryStream.Dispose();

            Assert.That(isValid, Is.False);
        }
    }
}