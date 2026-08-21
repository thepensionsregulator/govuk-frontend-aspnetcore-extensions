using Microsoft.AspNetCore.Builder;
using System;
using ThePensionsRegulator.GovUk.Frontend;

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

            app.UseTprGovUkFrontend();

            return app;
        }
    }
}