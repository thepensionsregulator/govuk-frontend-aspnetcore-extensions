using Microsoft.AspNetCore.Http;

namespace ThePensionsRegulator.GovUk.Frontend.Caching
{
    /// <summary>
    /// Middleware that adds long-term caching headers to static files.
    /// The files are safe to cache because they use cache-busting via query string parameters.
    /// </summary>
    /// <remarks>
    /// This must be called before UseStaticFiles() so it can intercept requests.
    /// UseStaticFiles() short-circuits the pipeline, so middleware after it won't execute.
    /// </remarks>
    public class CacheStaticFilesMiddleware(RequestDelegate _next, IEnumerable<IStaticFileCachePolicy> _pathProviders)
    {
        private const int CacheDurationDays = 365;

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if any policy says this request should be cached
            if (_pathProviders.Any(p => p.IsImmutable(context.Request.Path.Value ?? string.Empty, context.Request.Query)))
            {
                // Set cache control headers for 1 year
                context.Response.Headers.CacheControl = $"public, max-age={CacheDurationDays * 24 * 60 * 60}, immutable";
            }

            await _next(context);
        }
    }
}