using Microsoft.AspNetCore.Http;

namespace GovUk.Frontend.AspNetCore.Extensions.Caching
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