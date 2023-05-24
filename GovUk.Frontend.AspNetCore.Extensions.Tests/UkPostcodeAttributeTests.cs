using GovUk.Frontend.AspNetCore.Extensions.Validation;
using NUnit.Framework;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    [TestFixture]
    public class UkPostcodeAttributeTests
    {
        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase("bn74dw", true)]
        [TestCase("bn7 4dw", true)]
        [TestCase("BN7 4DW", true)]
        [TestCase("BN77 4DW", true)]
        [TestCase("BN74DW", true)]
        [TestCase("BN774DW", true)]
        [TestCase("   BN7 4DW   ", true)]
        [TestCase("BN7 -4DW", true)]
        [TestCase("(BN7)4DW.", true)]
        [TestCase("BN7  4DW", true)]
        [TestCase("- BN-7-4-DW-", true)] // hyphen
        [TestCase("BN7–4DW", false)] // n-dash
        [TestCase(" BN 77 4 DW ", true)]
        [TestCase("AA9A 9AA", true)]
        [TestCase("AA9A9AA", true)]
        [TestCase("A9A 9AA", true)]
        [TestCase("A9A9AA", true)]
        [TestCase("A9 9AA", true)]
        [TestCase("A99AA", true)]
        [TestCase("A99 9AA", true)]
        [TestCase("A999AA", true)]
        [TestCase("AA9 9AA", true)]
        [TestCase("AA99AA", true)]
        [TestCase("AA99 9AA", true)]
        [TestCase("AA999AA", true)]
        [TestCase("SWIA 0AA", false)]
        [TestCase("SW1A OAA", false)]
        [TestCase("SW1A~0AA", false)]
        [TestCase("S11A", false)]
        /// <remarks>
        /// <a href="https://design-system.service.gov.uk/patterns/addresses/#allow-different-postcode-formats">GOV.UK guidance on postcode validation</a> says 
        /// you should let users enter postcodes that contain:
        /// 
        /// - upper and lower case letters
        /// - no spaces
        /// - additional spaces at the beginning, middle or end
        /// - punctuation like hyphens, brackets, dashes and full stops
        /// 
        /// However, including an n-dash in the regex stops it working, so that is not allowed
        /// </remarks>
        public void Validates_Postcode(string? input, bool expected)
        {
            // Arrange
            var attribute = new UkPostcodeAttribute();

            // Act
            var result = attribute.IsValid(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
