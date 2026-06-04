using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Validation
{
    public static class TprAddressModelStateExtensions
    {
        public static void SetInitialAddressValues(
            this ModelStateDictionary modelState,
            ITprAddress? address,
            string? keyPrefix = null)
        {
            ArgumentNullException.ThrowIfNull(modelState);
            if (address is null) return;

            var prefix = string.IsNullOrWhiteSpace(keyPrefix) ? string.Empty : keyPrefix!.TrimEnd('.') + ".";

            modelState.SetInitialValue(prefix + nameof(TprAddress.AddressLine1), address.AddressLine1);
            modelState.SetInitialValue(prefix + nameof(TprAddress.AddressLine2), address.AddressLine2);
            modelState.SetInitialValue(prefix + nameof(TprAddress.AddressLine3), address.AddressLine3);
            modelState.SetInitialValue(prefix + nameof(TprAddress.PostTown), address.PostTown);
            modelState.SetInitialValue(prefix + nameof(TprAddress.PostCounty), address.PostCounty);
            modelState.SetInitialValue(prefix + nameof(TprAddress.PostCode), address.PostCode);
            modelState.SetInitialValue(prefix + nameof(TprAddress.CountryId), address.CountryId?.ToString(CultureInfo.InvariantCulture));
            modelState.SetInitialValue(prefix + nameof(TprAddress.UPRNReference), address.UPRNReference);
        }
    }
}
