using FileSignatures.Formats;
using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.GovUk.Frontend.UnitTests.BinaryFileValidators;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests
{
    public class AllowedFileTypesAttributeTests
    {
        public class ValidFileFormatTestData : TheoryData<Func<IFormFile>, Type[], bool>
        {
            public ValidFileFormatTestData()
            {
                // Excel file test
                Add(() =>
                {
                    var bytes = File.ReadAllBytes(TestFileLocator.EmptyExcelFile);
                    var memoryStream = new MemoryStream(bytes);
                    return new FormFile(memoryStream, 0, memoryStream.Length, null!, "myExcel.xlsx");
                }, new Type[1] { typeof(Excel) }, true);

                // PDF file test
                Add(() =>
                {
                    var bytes = File.ReadAllBytes(TestFileLocator.WordSavedAsPdf);
                    var memoryStream = new MemoryStream(bytes);
                    return new FormFile(memoryStream, 0, memoryStream.Length, null!, "myPdf.pdf");
                }, new Type[1] { typeof(Pdf) }, true);

                // PDF file with multiple types test (it is important that typeof(Pdf) is before typeof(Excel) for the below test. We are testing that it breaks as soon as it finds a matching extension)
                Add(() =>
                {
                    var bytes = File.ReadAllBytes(TestFileLocator.WordSavedAsPdf);
                    var memoryStream = new MemoryStream(bytes);
                    return new FormFile(memoryStream, 0, memoryStream.Length, null!, "myPdf.pdf");
                }, new Type[2] { typeof(Pdf), typeof(Excel) }, true);
            }
        }

        #region returnstrue

        [Theory]
        [ClassData(typeof(ValidFileFormatTestData))]
        public void Return_true_when_file_matches_file_types_specified(Func<IFormFile> fileFactory, Type[] allowedFileTypes, bool expected)
        {
            var file = fileFactory();
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
            var bytes = File.ReadAllBytes(TestFileLocator.EmptyExcelFile);
            var memoryStream = new MemoryStream(bytes);
            var testValue = new FormFile(memoryStream, 0, memoryStream.Length, null!, "abc.xlsx");

            var sut = new AllowedFileTypesAttribute([]);
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
            var bytes = File.ReadAllBytes(TestFileLocator.EmptyExcelFile);
            var memoryStream = new MemoryStream(bytes);
            var testValue = new FormFile(memoryStream, 0, memoryStream.Length, null!, "abc.txt");

            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
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