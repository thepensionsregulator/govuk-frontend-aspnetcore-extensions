using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    [TestFixture]
    public class AllowedFileExtensionsAttributeTests
    {

        [Test]
        [TestCaseSource(nameof(TestExcelFiles))]
        public void Should_ReturnValidResult_WhenFilesMatchFileTypesSpecified(IFormFile file)
        {
            var sut = new AllowedFileExtensionsAttribute([".xls", ".xlsx", ".xlsm"]);

            // Act
            var result = sut.IsValid(file);

            // Assert
            Assert.AreEqual(result, true);
        }


        private static IEnumerable<IFormFile> TestExcelFiles() =>
            new List<IFormFile>
            {
                CreateFormFile("test.xls"),
                CreateFormFile("test.xlsx"),
                CreateFormFile("test.xlsm")
            };

        private static IFormFile CreateFormFile(string filename)
        {
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", filename);
        }

    }
}
