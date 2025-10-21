using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using System;

namespace ThePensionsRegulator.Frontend
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTprFrontend(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseGovUkFrontendExtensions();

            return app;
        }
    }
}