using Microsoft.AspNetCore.Http;

namespace ThePensionsRegulator.GovUk.Frontend.Caching
{
    public class GovUkStaticFileCachePolicy : IStaticFileCachePolicy
    {
        public bool IsImmutable(string path, IQueryCollection query)
        {
            return path.StartsWith("/ThePensionsRegulator.GovUk.Frontend/", StringComparison.OrdinalIgnoreCase) &&
                   query.ContainsKey("v") &&
                   !string.IsNullOrEmpty(query["v"]);
        }

        public bool MustRevalidate(string path, IQueryCollection query)
        {
            return path.StartsWith("/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/", StringComparison.OrdinalIgnoreCase) &&
                   !query.ContainsKey("v");
        }
    }
}