using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ThePensionsRegulator.Umbraco.Core
{
    /// <summary>
    /// Default <see cref="IOverridablePublishedElementFactoryAccessor"/> which resolves the request-scoped
    /// <see cref="IOverridablePublishedElementFactory"/> via the current <see cref="HttpContext"/>'s request services.
    /// </summary>
    /// <remarks>
    /// Registered as a singleton so that other singleton services, such as property value converters, can safely
    /// depend on it without capturing a request-scoped instance.
    /// </remarks>
    public class DefaultOverridablePublishedElementFactoryAccessor(IHttpContextAccessor _httpContextAccessor) : IOverridablePublishedElementFactoryAccessor
    {
        /// <inheritdoc />
        public IOverridablePublishedElementFactory Get()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException($"No {nameof(HttpContext)} is available. {nameof(IOverridablePublishedElementFactory)} can only be resolved by {nameof(DefaultOverridablePublishedElementFactoryAccessor)} during an HTTP request.");

            return httpContext.RequestServices.GetRequiredService<IOverridablePublishedElementFactory>();
        }
    }
}
