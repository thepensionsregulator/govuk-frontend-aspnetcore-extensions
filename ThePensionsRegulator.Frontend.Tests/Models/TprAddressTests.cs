using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Tests.Models
{
    public class TprAddressTests
    {
        [Fact]
        public void PopulateAddress_MapsAllFields_AndNormalisesValues()
        {
            // Arrange
            var target = new TprAddress();
            ITprAddress source = new TprAddress
            {
                AddressLine1 = "  1 High Street  ",
                AddressLine2 = "  Business Park ",
                AddressLine3 = "  Building C  ",
                PostTown = "  Brighton  ",
                PostCounty = "  East Sussex ",
                PostCode = "  bn1 1aa  ",
                CountryId = 44,
                UPRNReference = "  UPRN-123  "
            };

            // Act
            target.PopulateAddress(source);

            // Assert
            Assert.Equal("1 High Street", target.AddressLine1);
            Assert.Equal("Business Park", target.AddressLine2);
            Assert.Equal("Building C", target.AddressLine3);
            Assert.Equal("Brighton", target.PostTown);
            Assert.Equal("East Sussex", target.PostCounty);
            Assert.Equal("BN1 1AA", target.PostCode);
            Assert.Equal(44, target.CountryId);
            Assert.Equal("UPRN-123", target.UPRNReference);
        }

        [Fact]
        public void PopulateAddress_KeepsNulls_AsNull()
        {
            // Arrange
            var target = new TprAddress
            {
                AddressLine1 = "existing",
                AddressLine2 = "existing",
                AddressLine3 = "existing",
                PostTown = "existing",
                PostCounty = "existing",
                PostCode = "existing",
                CountryId = 1,
                UPRNReference = "existing"
            };

            ITprAddress source = new TprAddress
            {
                AddressLine1 = null,
                AddressLine2 = null,
                AddressLine3 = null,
                PostTown = null,
                PostCounty = null,
                PostCode = null,
                CountryId = null,
                UPRNReference = null
            };

            // Act
            target.PopulateAddress(source);

            // Assert
            Assert.Null(target.AddressLine1);
            Assert.Null(target.AddressLine2);
            Assert.Null(target.AddressLine3);
            Assert.Null(target.PostTown);
            Assert.Null(target.PostCounty);
            Assert.Null(target.PostCode);
            Assert.Null(target.CountryId);
            Assert.Null(target.UPRNReference);
        }

        [Fact]
        public void PopulateAddress_UppercasesPostcode_WhenAlreadyTrimmed()
        {
            // Arrange
            var target = new TprAddress();
            ITprAddress source = new TprAddress { PostCode = "sw1a 1aa" };

            // Act
            target.PopulateAddress(source);

            // Assert
            Assert.Equal("SW1A 1AA", target.PostCode);
        }
    }
}
