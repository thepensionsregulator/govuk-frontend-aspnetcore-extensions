namespace ThePensionsRegulator.Frontend.Services
{
    public class DefaultTprCountryRepository : ITprCountryRepository
    {
        public Task<IDictionary<string, int>> GetCountries()
        {

            var dictionary = new Dictionary<string, int> { { $"Implement {nameof(ITprCountryRepository)} to provide a list of countries", 1 } };

            return Task.FromResult((IDictionary<string, int>)dictionary);
        }
    }
}
