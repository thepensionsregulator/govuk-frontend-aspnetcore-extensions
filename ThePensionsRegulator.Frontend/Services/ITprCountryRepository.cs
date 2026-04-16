using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.Services
{
    public interface ITprCountryRepository
    {
        /// <summary>
        /// Returns a dictionary of countries and country codes
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, int> GetCountries();
    }
}
