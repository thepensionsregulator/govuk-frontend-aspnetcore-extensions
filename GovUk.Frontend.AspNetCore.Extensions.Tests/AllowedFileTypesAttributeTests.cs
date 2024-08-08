using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators;
using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    public class AllowedFileTypesAttributeTests
    {
        private class ExcelDataProvider : IEnumerable
        {
            private string _testFilesPath = @"BinaryFileValidators\TestFiles";
            private string _emptyExcelFile => Path.Combine(new[] { _testFilesPath, "EmptyExcelFile.xlsx" });
            private string _emptyPowerPoint => Path.Combine(new[] { _testFilesPath, "EmptyPowerPoint.pptx" });

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

            private IFormFile GetPowerPoint(string filename)
            {
                var bytes = File.ReadAllBytes(_emptyPowerPoint);
                return CreateFormFile(bytes, filename);
            }

            public IEnumerator GetEnumerator()
            {
                yield return new object[] { GetExcelFile("myExcel.xlsx"), true };
                yield return new object[] { GetPowerPoint("myPowerpoint.pptx"), false };
            }
        }

        [TestCaseSource(typeof(ExcelDataProvider))]
        public void Return_true_when_file_matches_file_types_specified(IFormFile file, bool expected)
        {
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var result = sut.IsValid(file);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Return_false_if_file_signature_does_not_match()
        {
            Random rnd = new Random();
            var randomFileSignature = new Byte[10];
            rnd.NextBytes(randomFileSignature);

            var memoryStream = new MemoryStream(randomFileSignature);
            var file = new FormFile(memoryStream, 0, memoryStream.Length, null!, "myfile.xlsx");

            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var result = sut.IsValid(file);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Return_true_if_value_is_null()
        {
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var result = sut.IsValid(null!);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void Return_false_if_file_extension_is_wrong()
        {
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var testValue = new ExcelDataProvider().GetExcelFile("abc.txt");
            var result = sut.IsValid(testValue);

            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void Throw_Exception_if_value_is_not_IFormFile()
        {
            var testValue = "Some test string property value";
            var sut = new AllowedFileTypesAttribute([typeof(Excel)]);
            var fn = () =>
            {
                sut.IsValid(testValue);
            };
            Assert.That(fn, Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void Return_true_if_no_file_validation_types_have_been_supplied()
        {
            var sut = new AllowedFileTypesAttribute([]);
            var testValue = new ExcelDataProvider().GetExcelFile("abc.xlsx");
            var result = sut.IsValid(testValue);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void Throw_exception_if_supplied_validation_type_has_not_been_implemented()
        {
            var typeWeWillNeverImplementAFileValidatorFor = typeof(Exception);
            var fn = () => { var sut = new AllowedFileTypesAttribute([typeWeWillNeverImplementAFileValidatorFor]); };

            Assert.That(fn, Throws.InstanceOf<ArgumentException>());
        }
    }
}