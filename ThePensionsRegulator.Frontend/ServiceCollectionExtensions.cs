using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.Extensions.Security;
using Microsoft.Extensions.DependencyInjection;
using System;
using ThePensionsRegulator.Frontend.Security;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprFrontend(this IServiceCollection services)
        {
            return services.AddTprFrontend(options => { });
        }

        public static IServiceCollection AddTprFrontend(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();
            services.AddTransient<IConsentCookieReader, TprConsentCookieReader>();

            services.AddGovUkFrontendExtensions(options);
            return services;
        }
    }
}
