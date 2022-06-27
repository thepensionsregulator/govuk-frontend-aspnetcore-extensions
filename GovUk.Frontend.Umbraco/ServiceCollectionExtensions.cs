using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GovUk.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontendUmbracoExtensions(this IServiceCollection services)
        {
            return services.AddGovUkFrontendUmbracoExtensions(_ => { });
        }

        public static IServiceCollection AddGovUkFrontendUmbracoExtensions(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddGovUkFrontendExtensions(configureOptions);
            services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();

            return services;
        }
    }
}
