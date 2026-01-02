using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Caching;
using ThePensionsRegulator.Frontend.Security;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco.Caching;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Caching;
using ThePensionsRegulator.GovUk.Frontend.Security;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;

namespace ThePensionsRegulator.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services)
        {
            return services.AddTprFrontendUmbraco(options => { }, options => { }, options => { });
        }

        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions)
        {
            return services.AddTprFrontendUmbraco(configureGovUkOptions, options => { }, options => { });
        }

        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
        {
            return services.AddTprFrontendUmbraco(options => { }, configureGovUkUmbracoOptions);
        }

        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services,
            Action<TprFrontendOptions> configureTprOptions)
        {
            return services.AddTprFrontendUmbraco(options => { }, options => { }, configureTprOptions);
        }

        public static IServiceCollection AddTprFrontendUmbraco(
           this IServiceCollection services,
           Action<GovUkFrontendOptions> configureGovUkOptions,
           Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
        {
            return services.AddTprFrontendUmbraco(configureGovUkOptions, configureGovUkUmbracoOptions, options => { });
        }

        public static IServiceCollection AddTprFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions,
            Action<TprFrontendOptions> configureTprOptions)
        {
            return services.AddTprFrontendUmbraco(configureGovUkOptions, options => { }, configureTprOptions);
        }

        public static IServiceCollection AddTprFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions,
            Action<TprFrontendOptions> configureTprOptions)
        {
            return services.AddTprFrontendUmbraco(options => { }, configureGovUkUmbracoOptions, configureTprOptions);
        }

        public static IServiceCollection AddTprFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions,
            Action<TprFrontendOptions> configureTprOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // GovUk.Frontend.Umbraco
            services.AddTprGovUkFrontendUmbraco(configureGovUkOptions, configureGovUkUmbracoOptions);

            // ThePensionsRegulator.Frontend
            services.AddTransient<IConsentCookieReader, TprConsentCookieReader>();
            services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();
            services.AddTransient<IStaticFileCachePolicy, TprStaticFileCachePolicy>();

            var tprFrontendOptions = new TprFrontendOptions();
            if (configureTprOptions is not null) { configureTprOptions(tprFrontendOptions); }
            services.AddTransient((services) => Options.Create(tprFrontendOptions));

            // ThePensionsRegulator.Frontend.Umbraco
            services.AddTransient<IPropertyValueFormatter, HostNameInRichTextEditorPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, HostNameInMultiUrlPickerPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphsPropertyValueFormatter>();
            services.AddTransient<IPartialViewPathProvider, TprPartialViewPathProvider>();
            services.AddTransient<IBlockViewInterceptor, TprBoxViewInterceptor>();
            services.AddTransient<IBlockViewInterceptor, TprDividerViewInterceptor>();
            services.AddTransient<IDefaultColumnClassProvider, TprSectionCardsColumnClassProvider>();
            services.AddTransient<IYouTubeVideoIdParser, YouTubeVideoIdParser>();
            services.AddTransient<ITprGlobalNavigationService, TprGlobalNavigationService>();
            services.AddTransient<IStaticFileCachePolicy, TprUmbracoStaticFileCachePolicy>();

            return services;
        }
    }
}