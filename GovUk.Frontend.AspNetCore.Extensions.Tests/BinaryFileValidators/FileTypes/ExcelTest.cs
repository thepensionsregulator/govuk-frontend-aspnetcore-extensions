using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests.BinaryFileValidators.FileTypes
{
    internal class ExcelTest
    {
        private class ExcelDataProvider : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { TestFileLocator.EmptyExcelFile, true };
                yield return new object[] { TestFileLocator.EmptyPowerPoint, false };
            }
        }

        [TestCaseSource(typeof(ExcelDataProvider))]
        public void Can_identify_valid_excel_file(string filename, bool expected)
        {
            var bytes = File.ReadAllBytes(filename);
            var excelValidator = new Excel();
            var memoryStream = new MemoryStream(bytes);

            var isValid = excelValidator.IsMatch(memoryStream, filename);
            memoryStream.Dispose();

            Assert.That(isValid, Is.EqualTo(expected));
        }
    }
}