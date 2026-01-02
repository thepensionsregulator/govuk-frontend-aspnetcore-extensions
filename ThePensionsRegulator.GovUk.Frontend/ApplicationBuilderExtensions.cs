using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Builder;
using ThePensionsRegulator.GovUk.Frontend.Caching;

namespace ThePensionsRegulator.GovUk.Frontend
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTprGovUkFrontend(this IApplicationBuilder app)
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