using GovUk.Frontend.AspNetCore.Extensions.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NUnit.Framework;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    [TestFixture]
    public class UkPostcodeModelBinderTests
    {
        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("ab01aa", "AB0 1AA")]
        [TestCase("aa2 3aa", "AA2 3AA")]
        [TestCase("   AA4 5AA   ", "AA4 5AA")]
        [TestCase("AA6 -7AA", "AA6 7AA")]
        [TestCase("(AA8)9AA.", "AA8 9AA")]
        [TestCase("AA9  9AA", "AA9 9AA")]
        [TestCase(". AA.9.9.AA.", "AA9 9AA")]
        [TestCase("- AA-9-9-AA-", "AA9 9AA")] // hyphen
        [TestCase("AA9–9AA", "AA9 9AA")] // n-dash
        [TestCase(" BN 77 9 AA ", "BN77 9AA")]
        [TestCase("AA9A 9AA", "AA9A 9AA")]
        [TestCase("AA9A9AA", "AA9A 9AA")]
        [TestCase("A9A 9AA", "A9A 9AA")]
        [TestCase("A9A9AA", "A9A 9AA")]
        [TestCase("A9 9AA", "A9 9AA")]
        [TestCase("A99AA", "A9 9AA")]
        [TestCase("A99 9AA", "A99 9AA")]
        [TestCase("A999AA", "A99 9AA")]
        [TestCase("AA9 9AA", "AA9 9AA")]
        [TestCase("AA99AA", "AA9 9AA")]
        [TestCase("AA99 9AA", "AA99 9AA")]
        [TestCase("AA999AA", "AA99 9AA")]
        public async Task Formats_Postcode(string input, string expected)
        {
            // Arrange
            var modelBinder = new UkPostcodeModelBinder();
            var valueProvider = new SimpleValueProvider();
            var propertyName = "test";
            valueProvider.Add(propertyName, input);

            // Act
            var context = new DefaultModelBindingContext
            {
                ModelName = propertyName,
                ValueProvider = valueProvider
            };

            await modelBinder.BindModelAsync(context);

            // Assert
            Assert.That(context.Result.Model, Is.EqualTo(expected));
        }
    }
}
