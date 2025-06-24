using Microsoft.AspNetCore.Builder;

namespace GovUk.Frontend.Umbraco.ExampleApp.Middleware
{
    public static class SecurityHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}