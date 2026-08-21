using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.GovUk.Frontend.Caching;

namespace ThePensionsRegulator.Frontend.Caching
{
    public class TprStaticFileCachePolicy : IStaticFileCachePolicy
    {
        public bool IsImmutable(string path, IQueryCollection query)
        {
            return (path.StartsWith("/ThePensionsRegulator.Frontend/", StringComparison.OrdinalIgnoreCase) &&
                   query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"])) ||
                   path.StartsWith("/favicon.ico");
        }
    }
}