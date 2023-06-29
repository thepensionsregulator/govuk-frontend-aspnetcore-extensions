using GovUk.Frontend.AspNetCore.Extensions.Validation;
using NUnit.Framework;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    public class CompaniesHouseCompanyNumberAttributeTest
    {
        [Test]
        [TestCase("AA123456")]
        [TestCase("B1234567")]
        [TestCase("12345678")]
        public void Valid_companies_house_number(string? input)
        {
            TestNumber(input, expected: true);
        }

        [Test]
        [TestCase("AA1234560")]
        [TestCase("B12345670")]
        [TestCase("123456780")]
        public void Numbers_are_too_long(string number)
        {
            TestNumber(number, expected: false);
        }

        [Test]
        [TestCase("AA12345")]
        [TestCase("B123456")]
        [TestCase("1234567")]
        public void NumbersAreTooShort(string? number)
        {
            TestNumber(number, expected: false);
        }

        [Test]
        [TestCase("!A12345")]
        [TestCase("AAA12345")]
        [TestCase("AAA1234")]
        public void DoesNotStartWithCorrectLetters(string? number)
        {
            TestNumber(number, expected: false);
        }

        private void TestNumber(string? input, bool expected)
        {
            // Arrange
            var attribute = new CompaniesHouseCompanyNumberAttribute();

            // Act
            var result = attribute.IsValid(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}