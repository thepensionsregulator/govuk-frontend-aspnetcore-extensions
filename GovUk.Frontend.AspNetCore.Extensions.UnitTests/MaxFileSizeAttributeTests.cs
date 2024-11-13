using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
{
    [TestFixture]
    public class MaxFileSizeAttributeAttributeTests
    {
        private MaxFileSizeAttribute _sut = new MaxFileSizeAttribute(10_000);

        [Test]
        public void Should_ReturnValidResult_WhenFileSizeIsUnderMaximumValue()
        {
            var file = CreateFormFile(800);
            var result = _sut.IsValid(file);
            Assert.AreEqual(result, true);
        }

        [Test]
        public void Should_ReturnNotValidResult_FileSizeIsOverMaximumValue()
        {
            var file = CreateFormFile(20_000);
            var result = _sut.IsValid(file);
            Assert.AreEqual(result, false);
        }

        [Test]
        public void Should_ThrowException_IfTargetPropertyNotIFormFile()
        {
            var testValue = "Some test string property value";
            Assert.Throws<InvalidOperationException>(() => _sut.IsValid(testValue));
        }

        private static IFormFile CreateFormFile(long fileSize) =>
            new FormFile(new MemoryStream(), 0, fileSize, "Data", "test.file");

    }
}
