

using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests
{
    public class RegisteredCharityNumberAttributeTest
    {
        [Theory]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("SC123456")]
        [InlineData("sc123456")]
        [InlineData("Sc123456")]
        [InlineData("sC123456")]
        public void Valid_numbers(string input)
        {
            TestNumber(input, expected: true);
        }

        [Theory]
        [InlineData("AB123456")]
        [InlineData("ab123456")]
        [InlineData("Ab123456")]
        [InlineData("aB123456")]
        [InlineData("Sb123456")]
        [InlineData("sb123456")]
        [InlineData("SB123456")]
        [InlineData("BC123456")]
        [InlineData("bC123456")]
        [InlineData("Bc123456")]
        [InlineData("bc123456")]
        public void Incorrect_prefix(string input)
        {
            TestNumber(input, expected: false);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("12345678")]
        [InlineData("sc12345")]
        [InlineData("sc12345678")]
        public void Numbers_incorrect_length(string? input)
        {
            TestNumber(input, expected: false);
        }

        private void TestNumber(string? input, bool expected)
        {
            // Arrange
            var attribute = new RegisteredCharityNumberAttribute();

            // Act
            var result = attribute.IsValid(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}