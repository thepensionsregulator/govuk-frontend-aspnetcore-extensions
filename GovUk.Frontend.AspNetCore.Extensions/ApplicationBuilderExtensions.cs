using GovUk.Frontend.AspNetCore.Extensions.Caching;
using Microsoft.AspNetCore.Builder;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGovUkFrontendExtensions(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware<CacheStaticFilesMiddleware>();
            app.UseStaticFiles();

            app.UseGovUkFrontend();

            return app;
        }
    }
}