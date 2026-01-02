using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.GovUk.Frontend.Caching;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Caching
{
    public class GovUkUmbracoStaticFileCachePolicy : IStaticFileCachePolicy
    {
        public bool IsImmutable(string path, IQueryCollection query)
        {
            const string basePath = "/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/";

            return path.StartsWith($"{basePath}govuk-", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith($"{basePath}package-version.generated-", StringComparison.OrdinalIgnoreCase) ||
                  (path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase) && path.Contains("-helper-", StringComparison.OrdinalIgnoreCase)) ||
                  (path.StartsWith("/ThePensionsRegulator.GovUk.Frontend.Umbraco/", StringComparison.OrdinalIgnoreCase) && query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"])) ||
                  (path.StartsWith("/css/govuk-umbraco-backoffice.css", StringComparison.OrdinalIgnoreCase) && query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"]));
        }
    }
}