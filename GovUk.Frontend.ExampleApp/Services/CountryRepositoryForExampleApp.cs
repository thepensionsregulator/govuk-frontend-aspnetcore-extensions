using System.Collections.Generic;
using ThePensionsRegulator.Frontend.Services;

namespace GovUk.Frontend.ExampleApp.Services
{
    public class CountryRepositoryForExampleApp : ITprCountryRepository
    {
        public IDictionary<string, int> GetCountries()
        {
            return new Dictionary<string, int>{ {"United Kingdom", 1}, { "France", 2 } }; 
        }
    }
}
