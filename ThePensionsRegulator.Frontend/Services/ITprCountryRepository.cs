namespace ThePensionsRegulator.Frontend.Services
{
    public interface ITprCountryRepository
    {
        /// <summary>
        /// Returns a dictionary of countries and country codes
        /// </summary>
        /// <returns></returns>
        public Task<IDictionary<string, int>> GetCountries();
    }
}
