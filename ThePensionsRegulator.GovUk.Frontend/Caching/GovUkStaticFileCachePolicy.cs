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
    }
}