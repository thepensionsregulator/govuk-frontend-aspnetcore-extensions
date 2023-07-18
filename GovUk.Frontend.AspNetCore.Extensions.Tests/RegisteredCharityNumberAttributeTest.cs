using GovUk.Frontend.AspNetCore.Extensions.Validation;
using NUnit.Framework;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    public class RegisteredCharityNumberAttributeTest
    {
        [Test]
        [TestCase("123456")]
        [TestCase("1234567")]
        [TestCase("SC123456")]
        [TestCase("sc123456")]
        [TestCase("Sc123456")]
        [TestCase("sC123456")]
        public void Valid_numbers(string input)
        {
            TestNumber(input, expected: true);
        }

        [TestCase("AB123456")]
        [TestCase("ab123456")]
        [TestCase("Ab123456")]
        [TestCase("aB123456")]
        [TestCase("Sb123456")]
        [TestCase("sb123456")]
        [TestCase("SB123456")]
        [TestCase("BC123456")]
        [TestCase("bC123456")]
        [TestCase("Bc123456")]
        [TestCase("bc123456")]
        public void Incorrect_prefix(string input)
        {
            TestNumber(input, expected: false);
        }

        [Test]
        [TestCase("12345")]
        [TestCase("12345678")]
        [TestCase("sc12345")]
        [TestCase("sc12345678")]
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
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}