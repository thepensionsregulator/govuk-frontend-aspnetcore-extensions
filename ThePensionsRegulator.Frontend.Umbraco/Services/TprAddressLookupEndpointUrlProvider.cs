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

        public string GetAddressLookupSearchEndpoint()
        {
            return GetValue("SearchEndpoint");
        }
        
        public string GetAddressLookupIdEndpoint()
        {
            return GetValue("AddressByIdEndpoint");
        }

        private string GetValue(string section) 
        {
            return _addressLookupSection[section] ?? string.Empty;
        }
    }
}
