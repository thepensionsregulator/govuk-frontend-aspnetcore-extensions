using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
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
            modelState.SetModelValue(prefix + nameof(TprAddress.PostCode), null, address.PostCode);
            modelState.SetModelValue(prefix + nameof(TprAddress.CountryId), null, address.CountryId?.ToString(CultureInfo.InvariantCulture));
            modelState.SetModelValue(prefix + nameof(TprAddress.UPRNReference), null, address.UPRNReference);
        }
    }
}
