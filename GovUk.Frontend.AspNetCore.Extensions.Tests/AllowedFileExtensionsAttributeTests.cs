using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    [TestFixture]
    public class AllowedFileExtensionsAttributeTests
    {

        [Test]
        [TestCaseSource(nameof(TestExcelFiles))]
        public void Should_do_the_thing(IFormFile file)
        {
            var sut = new AllowedFileExtensionsAttribute(new[] { ".xcls" });

            // Act
            var result = sut.IsValid(file);

            // Assert
            Assert.AreEqual(result, true);
        }


        private static IEnumerable<IFormFile> TestExcelFiles;

    }
}
