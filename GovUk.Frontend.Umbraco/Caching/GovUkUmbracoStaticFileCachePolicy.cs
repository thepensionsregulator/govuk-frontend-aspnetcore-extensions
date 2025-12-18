using GovUk.Frontend.AspNetCore.Extensions.Caching;
using Microsoft.AspNetCore.Http;

namespace GovUk.Frontend.Umbraco.Caching
{
    public class GovUkUmbracoStaticFileCachePolicy : IStaticFileCachePolicy
    {
        public bool IsImmutable(string path, IQueryCollection query)
        {
            const string basePath = "/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/";

            return path.StartsWith($"{basePath}govuk-", StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith($"{basePath}package-version.generated-", StringComparison.OrdinalIgnoreCase) ||
                  (path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase) && path.Contains("-helper-", StringComparison.OrdinalIgnoreCase)) ||
                  (path.StartsWith("/govuk/govuk-frontend.css", StringComparison.OrdinalIgnoreCase) && query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"])) ||
                  (path.StartsWith("/css/govuk-umbraco-backoffice.css", StringComparison.OrdinalIgnoreCase) && query.ContainsKey("v") && !string.IsNullOrEmpty(query["v"]));
        }
    }
}