using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Validation;

namespace ThePensionsRegulator.Frontend.Tests.Validation
{
    public class TprAddressModelStateExtensionsTests
    {
        [Fact]
        public void SetModelValues_Throws_WhenModelStateIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => TprAddressModelStateExtensions.SetModelValues(null!, new TprAddress()));
        }

        [Fact]
        public void SetModelValues_DoesNothing_WhenAddressIsNull()
        {
            var modelState = new ModelStateDictionary();

            modelState.SetModelValues(null);

            Assert.Empty(modelState);
        }

        [Fact]
        public void SetModelValues_SetsAllFields_WithoutPrefix()
        {
            var modelState = new ModelStateDictionary();
            var address = CreateAddress();

            modelState.SetModelValues(address);

            Assert.Equal(address.AddressLine1, modelState[nameof(TprAddress.AddressLine1)]!.AttemptedValue);
            Assert.Equal(address.AddressLine2, modelState[nameof(TprAddress.AddressLine2)]!.AttemptedValue);
            Assert.Equal(address.AddressLine3, modelState[nameof(TprAddress.AddressLine3)]!.AttemptedValue);
            Assert.Equal(address.PostTown, modelState[nameof(TprAddress.PostTown)]!.AttemptedValue);
            Assert.Equal(address.PostCounty, modelState[nameof(TprAddress.PostCounty)]!.AttemptedValue);
            Assert.Equal(address.PostCode, modelState[nameof(TprAddress.PostCode)]!.AttemptedValue);
            Assert.Equal("44", modelState[nameof(TprAddress.CountryId)]!.AttemptedValue);
            Assert.Equal(address.UPRNReference, modelState[nameof(TprAddress.UPRNReference)]!.AttemptedValue);
        }

        [Theory]
        [InlineData("address")]
        [InlineData("address.")]
        public void SetModelValues_SetsAllFields_WithPrefix(string keyPrefix)
        {
            var modelState = new ModelStateDictionary();
            var address = CreateAddress();

            modelState.SetModelValues(address, keyPrefix);

            Assert.Equal(address.AddressLine1, modelState[$"address.{nameof(TprAddress.AddressLine1)}"]!.AttemptedValue);
            Assert.Equal(address.AddressLine2, modelState[$"address.{nameof(TprAddress.AddressLine2)}"]!.AttemptedValue);
            Assert.Equal(address.AddressLine3, modelState[$"address.{nameof(TprAddress.AddressLine3)}"]!.AttemptedValue);
            Assert.Equal(address.PostTown, modelState[$"address.{nameof(TprAddress.PostTown)}"]!.AttemptedValue);
            Assert.Equal(address.PostCounty, modelState[$"address.{nameof(TprAddress.PostCounty)}"]!.AttemptedValue);
            Assert.Equal(address.PostCode, modelState[$"address.{nameof(TprAddress.PostCode)}"]!.AttemptedValue);
            Assert.Equal("44", modelState[$"address.{nameof(TprAddress.CountryId)}"]!.AttemptedValue);
            Assert.Equal(address.UPRNReference, modelState[$"address.{nameof(TprAddress.UPRNReference)}"]!.AttemptedValue);
        }

        [Fact]
        public void SetModelValues_DoesNotUsePrefix_WhenPrefixIsWhitespace()
        {
            var modelState = new ModelStateDictionary();
            var address = CreateAddress();

            modelState.SetModelValues(address, "   ");

            Assert.True(modelState.ContainsKey(nameof(TprAddress.AddressLine1)));
            Assert.False(modelState.ContainsKey($"address.{nameof(TprAddress.AddressLine1)}"));
        }

        [Fact]
        public void SetModelValues_SetsCountryIdToNull_WhenCountryIdIsNull()
        {
            var modelState = new ModelStateDictionary();
            var address = CreateAddress();
            address.CountryId = null;

            modelState.SetModelValues(address);

            Assert.True(modelState.ContainsKey(nameof(TprAddress.CountryId)));
            Assert.Null(modelState[nameof(TprAddress.CountryId)]!.AttemptedValue);
        }

        private static TprAddress CreateAddress() => new()
        {
            AddressLine1 = "1 High Street",
            AddressLine2 = "Business Park",
            AddressLine3 = "Area 51",
            PostTown = "Manchester",
            PostCounty = "Greater Manchester",
            PostCode = "M1 1AA",
            CountryId = 44,
            UPRNReference = "100021064336"
        };
    }
}
