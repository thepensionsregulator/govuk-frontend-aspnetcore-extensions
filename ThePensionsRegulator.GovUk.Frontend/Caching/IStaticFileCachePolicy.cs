using Microsoft.AspNetCore.Http;

namespace ThePensionsRegulator.GovUk.Frontend.Caching
{
    /// <summary>
    /// Determines whether a static file request should be cached with long-term cache headers.
    /// </summary>
    public interface IStaticFileCachePolicy
    {
        /// <summary>
        /// Determines whether the specified request should be cached with long-term cache headers.
        /// Implementations should check both the path and query string (for cache-busting parameters).
        /// </summary>
        /// <param name="path">The request path (e.g., "/govuk/govuk-frontend.css" or "/_content/Package/file.css")</param>
        /// <param name="query">The query string from the request.</param>
        /// <returns>True if the request should be cached with long-term headers; otherwise, false.</returns>
        bool IsImmutable(string path, IQueryCollection query);

        /// <summary>
        /// Determines whether the specified request could be cached so long as the browser checks for updates.
        /// Updates are identified by a change in the ETag, which is added automatically by ASP.NET Core.
        /// </summary>
        /// <param name="path">The request path (e.g., "/govuk/govuk-frontend.css" or "/_content/Package/file.css")</param>
        /// <param name="query">The query string from the request.</param>
        /// <returns>True if the request should require revalidation; otherwise, false.</returns>
        bool MustRevalidate(string path, IQueryCollection query) => false;
    }
}
