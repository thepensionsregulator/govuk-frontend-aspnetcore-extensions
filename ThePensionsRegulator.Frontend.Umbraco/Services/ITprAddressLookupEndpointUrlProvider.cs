namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface ITprAddressLookupEndpointUrlProvider
    {
        public string GetAddressLookupSearchEndpoint();

        public string GetAddressLookupIdEndpoint();
    }
}
