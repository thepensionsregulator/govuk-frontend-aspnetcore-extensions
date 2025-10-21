using Microsoft.AspNetCore.Builder;
using System;

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

            app.UseGovUkFrontend();

            return app;
        }
    }
}