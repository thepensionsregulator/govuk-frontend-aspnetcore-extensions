using GovUk.Frontend.AspNetCore.Extensions.Caching;
using Microsoft.AspNetCore.Http;

namespace ThePensionsRegulator.Frontend.Caching
{
    public class TprStaticFileCachePolicy : IStaticFileCachePolicy
    {
        public bool IsImmutable(string path, IQueryCollection query)
        {
            return path.StartsWith("/_content/ThePensionsRegulator.Frontend/", StringComparison.OrdinalIgnoreCase) &&
                   query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"]);
        }
    }
}