using GovUk.Frontend.AspNetCore.Extensions.Validation;


namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
{
    public class CompaniesHouseCompanyNumberAttributeTest
    {
        [Theory]
        [InlineData("AA123456")]
        [InlineData("B1234567")]
        [InlineData("12345678")]
        public void Valid_companies_house_number(string? input)
        {
            TestNumber(input, expected: true);
        }

        [Theory]
        [InlineData("AA1234560")]
        [InlineData("B12345670")]
        [InlineData("123456780")]
        public void Numbers_are_too_long(string number)
        {
            TestNumber(number, expected: false);
        }

        [Theory]
        [InlineData("AA12345")]
        [InlineData("B123456")]
        [InlineData("1234567")]
        public void NumbersAreTooShort(string? number)
        {
            TestNumber(number, expected: false);
        }

        [Theory]
        [InlineData("!A12345")]
        [InlineData("AAA12345")]
        [InlineData("AAA1234")]
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
            Assert.Equal(expected, result);
        }
    }
}