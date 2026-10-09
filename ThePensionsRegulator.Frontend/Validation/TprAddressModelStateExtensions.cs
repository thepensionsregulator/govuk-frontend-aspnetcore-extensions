using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Validation
{
    public static class TprAddressModelStateExtensions
    {
        public static void SetModelValues(this ModelStateDictionary modelState, ITprAddress? address, string? keyPrefix = null)
        {
            ArgumentNullException.ThrowIfNull(modelState);
            if (address is null) return;

            var prefix = string.IsNullOrWhiteSpace(keyPrefix) ? string.Empty : keyPrefix!.TrimEnd('.') + ".";

            modelState.SetModelValue(prefix + nameof(TprAddress.AddressLine1), null, address.AddressLine1);
            modelState.SetModelValue(prefix + nameof(TprAddress.AddressLine2), null, address.AddressLine2);
            modelState.SetModelValue(prefix + nameof(TprAddress.AddressLine3), null, address.AddressLine3);
            modelState.SetModelValue(prefix + nameof(TprAddress.PostTown), null, address.PostTown);
            modelState.SetModelValue(prefix + nameof(TprAddress.PostCounty), null, address.PostCounty);
            modelState.SetModelValue(prefix + nameof(TprAddress.Postcode), null, address.Postcode);
            modelState.SetModelValue(prefix + nameof(TprAddress.CountryId), null, address.CountryId?.ToString(CultureInfo.InvariantCulture));
            modelState.SetModelValue(prefix + nameof(TprAddress.UPRNReference), null, address.UPRNReference);
        }

        public static void SetInitialValues(this ModelStateDictionary modelState, ITprAddress address, string? keyPrefix = null)
        {
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.AddressLine1), address.AddressLine1);
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.AddressLine2), address.AddressLine2);
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.AddressLine3), address.AddressLine3);
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.PostTown), address.PostTown);
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.PostCounty), address.PostCounty);
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.Postcode), address.Postcode);
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.CountryId), address.CountryId?.ToString(CultureInfo.InvariantCulture));
            modelState.SetInitialAddressValue(address, keyPrefix, nameof(TprAddress.UPRNReference), address.UPRNReference);
        }

        private static void SetInitialAddressValue(this ModelStateDictionary modelState, ITprAddress? address, string? keyPrefix, string field, string? attemptedValue)
        {
            var prefix = string.IsNullOrWhiteSpace(keyPrefix) ? string.Empty : keyPrefix!.TrimEnd('.') + ".";
            modelState.SetModelValue(prefix + field, null, attemptedValue);
            modelState.MarkFieldSkipped(prefix + field);
        }
    }
}
