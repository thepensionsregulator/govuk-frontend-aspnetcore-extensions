namespace ThePensionsRegulator.Frontend.Services
{
    public interface ITprAddressLookupEndpointUrlProvider
    {
        public string GetAddressLookupSearchEndpoint();

        public string GetAddressLookupIdEndpoint();
    }
}
