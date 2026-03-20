using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Caching;
using ThePensionsRegulator.Frontend.Security;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.GovUk.Frontend;
using ThePensionsRegulator.GovUk.Frontend.Caching;
using ThePensionsRegulator.GovUk.Frontend.Security;

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
            services.AddTransient<IStaticFileCachePolicy, TprStaticFileCachePolicy>();

            services.AddTprGovUkFrontend(configureGovUkOptions);

            var tprFrontendOptions = new TprFrontendOptions();
            if (configureTprOptions is not null) { configureTprOptions(tprFrontendOptions); }
            services.AddTransient((services) => Options.Create(tprFrontendOptions));

            return services;
        }
    }
}
