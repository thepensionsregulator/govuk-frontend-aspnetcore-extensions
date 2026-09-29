using ThePensionsRegulator.Frontend.Services;

namespace GovUk.Frontend.ExampleApp.Services
{
    public class ExampleTprCountryRepository : ITprCountryRepository
    {
        public Task<IDictionary<string, int>> GetCountries()
        {
            var countries = new Dictionary<string, int>
            {
                { "United Kingdom", 1 },
                { "United States", 2 },
                { "Canada", 3 },
                { "Australia", 4 },
                { "Germany", 5 }
            };

            return Task.FromResult((IDictionary<string, int>)countries);
        }
    }
}
