using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ThePensionsRegulator.Frontend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprFrontend(this IServiceCollection services)
        {
            return services.AddTprFrontend(options => { options.AddImportsToHtml = false; });
        }

        public static IServiceCollection AddTprFrontend(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddGovUkFrontendExtensions(options);
            return services;
        }
    }
}
