using FileSignatures.Formats;
using GovUk.Frontend.AspNetCore.Extensions.UnitTests.BinaryFileValidators;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
{
    public class AllowedFileTypesAttributeTests
    {
        private class ValidFileFormatDataProvider : IEnumerable
        {
            private string _emptyExcelFile => TestFileLocator.EmptyExcelFile;
            private string _emptyPdf => TestFileLocator.WordSavedAsPdf;

            public IFormFile CreateFormFile(byte[] bytes, string filename)
            {
                var memoryStream = new MemoryStream(bytes);
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, null!, filename);
                return formFile;
            }

            public IFormFile GetExcelFile(string filename)
            {
                var bytes = File.ReadAllBytes(_emptyExcelFile);
                return CreateFormFile(bytes, filename);
            }

            private IFormFile GetPdf(string filename)
            {
                var bytes = File.ReadAllBytes(_emptyPdf);
                return CreateFormFile(bytes, filename);
            }

            public IEnumerator GetEnumerator()
            {
                yield return new object[] { GetExcelFile("myExcel.xlsx"), new Type[1] { typeof(Excel) }, true };
                yield return new object[] { GetPdf("myPdf.pdf"), new Type[1] { typeof(Pdf) }, true };
                // It is important that typeof(pdf) is before typeof(Excel) for the below test. We are testing that it breaks as soon as it finds a matching extension
                yield return new object[] { GetPdf("myPdf.pdf"), new Type[2] { typeof(Pdf), typeof(Excel) }, true };
            }
        }

        public static IEnumerable<object[]> ValidFileFormatData()
        {
            var provider = new ValidFileFormatDataProvider();
            foreach (var item in provider)
            {
                yield return (object[])item;
            }
        }

        #region returnstrue

        [Theory]
        [MemberData(nameof(ValidFileFormatData))]
        public void Return_true_when_file_matches_file_types_specified(IFormFile file, Type[] allowedFileTypes, bool expected)
        {
            var sut = new AllowedFileTypesAttribute(allowedFileTypes);
            var result = sut.IsValid(file);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Return_true_if_value_is_null()
        {
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var result = sut.IsValid(null!);

            Assert.True(result);
        }

        [Fact]
        public void Return_true_if_no_file_validation_types_have_been_supplied()
        {
            var sut = new AllowedFileTypesAttribute([]);
            var testValue = new ValidFileFormatDataProvider().GetExcelFile("abc.xlsx");
            var result = sut.IsValid(testValue);

            Assert.True(result);
        }

        #endregion returnstrue

        #region returnsfalse

        [Fact]
        public void Return_false_if_file_signature_does_not_match()
        {
            Random rnd = new Random();
            var randomFileSignature = new byte[10];
            rnd.NextBytes(randomFileSignature);

            var memoryStream = new MemoryStream(randomFileSignature);
            var file = new FormFile(memoryStream, 0, memoryStream.Length, null!, "myfile.xlsx");

            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var result = sut.IsValid(file);

            Assert.False(result);
        }

        [Fact]
        public void Return_false_if_file_extension_is_wrong()
        {
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var testValue = new ValidFileFormatDataProvider().GetExcelFile("abc.txt");
            var result = sut.IsValid(testValue);

            Assert.False(result);
        }

        #endregion returnsfalse

        #region exceptions

        [Fact]
        public void Throw_Exception_if_value_is_not_IFormFile()
        {
            var testValue = "Some test string property value";
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var fn = () =>
            {
                sut.IsValid(testValue);
            };
            Assert.Throws<InvalidOperationException>(fn);
        }

        [Fact]
        public void Throw_exception_if_supplied_validation_type_has_not_been_implemented()
        {
            var typeWeWillNeverImplementAFileValidatorFor = typeof(Exception);
            var fn = () => { var sut = new AllowedFileTypesAttribute([typeWeWillNeverImplementAFileValidatorFor]); };

            Assert.Throws<ArgumentException>(fn);
        }

        #endregion exceptions
    }
}