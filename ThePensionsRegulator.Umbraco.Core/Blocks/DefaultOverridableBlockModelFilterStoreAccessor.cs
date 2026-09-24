using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <inheritdoc/>
    public class DefaultOverridableBlockModelFilterStoreAccessor(IHttpContextAccessor _httpContextAccessor) : IOverridableBlockModelFilterStoreAccessor
    {
        /// <inheritdoc/>
        public IOverridableBlockModelFilterStore Get()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException($"No {nameof(HttpContext)} is available. {nameof(IOverridableBlockModelFilterStore)} can only be resolved by {nameof(DefaultOverridableBlockModelFilterStoreAccessor)} during an HTTP request.");

            return httpContext.RequestServices.GetRequiredService<IOverridableBlockModelFilterStore>();
        }
    }
}
