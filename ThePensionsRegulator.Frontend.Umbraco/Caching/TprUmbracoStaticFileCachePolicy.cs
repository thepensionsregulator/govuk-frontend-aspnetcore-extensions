using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.GovUk.Frontend.Caching;

namespace ThePensionsRegulator.Frontend.Umbraco.Caching
{
    public class TprUmbracoStaticFileCachePolicy : IStaticFileCachePolicy
    {
        public bool IsImmutable(string path, IQueryCollection query)
        {
            const string basePath = "/App_Plugins/ThePensionsRegulator.Frontend.Umbraco/";

            return path.StartsWith($"{basePath}tpr-", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith($"{basePath}package-version.generated-", StringComparison.OrdinalIgnoreCase) ||
                  (path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase) && path.Contains("-helper-", StringComparison.OrdinalIgnoreCase)) ||
                  (path.StartsWith("/ThePensionsRegulator.Frontend.Umbraco/", StringComparison.OrdinalIgnoreCase) && query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"]));
        }
    }
}