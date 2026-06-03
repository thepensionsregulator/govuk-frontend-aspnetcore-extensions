using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.Extensions.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using ThePensionsRegulator.Frontend.Security;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprFrontend(this IServiceCollection services) => services.AddTprFrontend(options => { }, options => { });

        public static IServiceCollection AddTprFrontend(
            this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions) => AddTprFrontend(services, configureGovUkOptions, options => { });

        public static IServiceCollection AddTprFrontend(
            this IServiceCollection services,
            Action<TprFrontendOptions> configureTprOptions) => AddTprFrontend(services, options => { }, configureTprOptions);

        public static IServiceCollection AddTprFrontend(
            this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions,
            Action<TprFrontendOptions> configureTprOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();
            services.AddTransient<IConsentCookieReader, TprConsentCookieReader>();

            services.AddGovUkFrontendExtensions(configureGovUkOptions);

            var tprFrontendOptions = new TprFrontendOptions();
            if (configureTprOptions is not null) { configureTprOptions(tprFrontendOptions); }
            services.AddTransient((services) => Options.Create(tprFrontendOptions));

            return services;
        }
    }
}
