using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using NUnit.Framework;
using System.Collections;
using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests.BinaryFileValidators.FileTypes
{
    internal class PdfTest
    {
        private class PdfDataProvider : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                yield return new object[] { TestFileLocator.WordSavedAsPdf, true };
            }
        }

        [TestCaseSource(typeof(PdfDataProvider))]
        public void Test(string filename, bool expected)
        {
            var bytes = File.ReadAllBytes(filename);

            var memoryStream = new MemoryStream(bytes);
            var pdf = new Pdf();
            var valid = pdf.IsMatch(memoryStream);

            memoryStream.Dispose();

            Assert.That(valid, Is.EqualTo(expected));
        }
    }
}