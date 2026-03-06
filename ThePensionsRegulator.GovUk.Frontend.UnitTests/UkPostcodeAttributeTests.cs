using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests
{
    public class UkPostcodeAttributeTests
    {
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("bn74dw", true)]
        [InlineData("bn7 4dw", true)]
        [InlineData("BN7 4DW", true)]
        [InlineData("BN77 4DW", true)]
        [InlineData("BN74DW", true)]
        [InlineData("BN774DW", true)]
        [InlineData("   BN7 4DW   ", true)]
        [InlineData("BN7 -4DW", true)]
        [InlineData("(BN7)4DW.", true)]
        [InlineData("BN7  4DW", true)]
        [InlineData("- BN-7-4-DW-", true)] // hyphen
        [InlineData("BN7–4DW", false)] // n-dash
        [InlineData(" BN 77 4 DW ", true)]
        [InlineData("AA9A 9AA", true)]
        [InlineData("AA9A9AA", true)]
        [InlineData("A9A 9AA", true)]
        [InlineData("A9A9AA", true)]
        [InlineData("A9 9AA", true)]
        [InlineData("A99AA", true)]
        [InlineData("A99 9AA", true)]
        [InlineData("A999AA", true)]
        [InlineData("AA9 9AA", true)]
        [InlineData("AA99AA", true)]
        [InlineData("AA99 9AA", true)]
        [InlineData("AA999AA", true)]
        [InlineData("SWIA 0AA", false)]
        [InlineData("SW1A OAA", false)]
        [InlineData("SW1A~0AA", false)]
        [InlineData("S11A", false)]
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
            Assert.Equal(expected, result);
        }
    }
}
