using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThePensionsRegulator.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    internal interface IAddressFieldStateHelper
    {
        public AddressFieldState GetFieldState(
            string? modelPropertyName,
            string? umbracoLabel,
            string defaultLabel,
            ModelStateDictionary modelState);
    }
}
