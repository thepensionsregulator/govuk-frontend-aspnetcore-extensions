using GovUk.Frontend.AspNetCore.Extensions.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
{

    public class UkPostcodeModelBinderTests
    {
        [Theory]
        [InlineData(null!, "")]
        [InlineData("", "")]
        [InlineData("ab01aa", "AB0 1AA")]
        [InlineData("aa2 3aa", "AA2 3AA")]
        [InlineData("   AA4 5AA   ", "AA4 5AA")]
        [InlineData("AA6 -7AA", "AA6 7AA")]
        [InlineData("(AA8)9AA.", "AA8 9AA")]
        [InlineData("AA9  9AA", "AA9 9AA")]
        [InlineData(". AA.9.9.AA.", "AA9 9AA")]
        [InlineData("- AA-9-9-AA-", "AA9 9AA")] // hyphen
        [InlineData("AA9–9AA", "AA9 9AA")] // n-dash
        [InlineData(" BN 77 9 AA ", "BN77 9AA")]
        [InlineData("AA9A 9AA", "AA9A 9AA")]
        [InlineData("AA9A9AA", "AA9A 9AA")]
        [InlineData("A9A 9AA", "A9A 9AA")]
        [InlineData("A9A9AA", "A9A 9AA")]
        [InlineData("A9 9AA", "A9 9AA")]
        [InlineData("A99AA", "A9 9AA")]
        [InlineData("A99 9AA", "A99 9AA")]
        [InlineData("A999AA", "A99 9AA")]
        [InlineData("AA9 9AA", "AA9 9AA")]
        [InlineData("AA99AA", "AA9 9AA")]
        [InlineData("AA99 9AA", "AA99 9AA")]
        [InlineData("AA999AA", "AA99 9AA")]
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
            Assert.Equal(expected, context.Result.Model);
        }
    }
}
