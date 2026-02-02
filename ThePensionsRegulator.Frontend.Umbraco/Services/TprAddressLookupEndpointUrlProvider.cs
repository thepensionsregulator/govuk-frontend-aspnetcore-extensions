using Microsoft.Extensions.Configuration;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprAddressLookupEndpointUrlProvider : ITprAddressLookupEndpointUrlProvider
    {
        IConfigurationSection _addressLookupSection;

        public TprAddressLookupEndpointUrlProvider(IConfiguration configuration)
        {
            _addressLookupSection = configuration.GetSection("AddressLookup");
        }

        public string GetAddressLookupEndpoint()
        {
            var apiEndpoint = _addressLookupSection["ApiEndpoint"] ?? string.Empty;

            return apiEndpoint;
        }
    }
}
