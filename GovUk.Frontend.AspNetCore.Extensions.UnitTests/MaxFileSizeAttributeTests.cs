using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Http;

namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
{
    public class MaxFileSizeAttributeAttributeTests
    {
        private MaxFileSizeAttribute _sut = new MaxFileSizeAttribute(10_000);

        [Fact]
        public void Should_ReturnValidResult_WhenFileSizeIsUnderMaximumValue()
        {
            var file = CreateFormFile(800);
            var result = _sut.IsValid(file);
            Assert.True(result);
        }

        [Fact]
        public void Should_ReturnNotValidResult_FileSizeIsOverMaximumValue()
        {
            var file = CreateFormFile(20_000);
            var result = _sut.IsValid(file);
            Assert.False(result);
        }

        [Fact]
        public void Should_ThrowException_IfTargetPropertyNotIFormFile()
        {
            var testValue = "Some test string property value";
            Assert.Throws<InvalidOperationException>(() => _sut.IsValid(testValue));
        }

        private static IFormFile CreateFormFile(long fileSize) =>
            new FormFile(new MemoryStream(), 0, fileSize, "Data", "test.file");

    }
}
