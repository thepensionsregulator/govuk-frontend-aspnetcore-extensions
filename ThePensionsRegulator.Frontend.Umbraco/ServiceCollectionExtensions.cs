using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions.Security;
using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using ThePensionsRegulator.Frontend.Security;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.PropertyEditors;

namespace ThePensionsRegulator.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services)
        {
            return services.AddTprFrontendUmbraco(options => { }, options => { }, options => { });
        }

        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureGovUkOptions)
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
           Action<GovUkFrontendAspNetCoreOptions> configureGovUkOptions,
           Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
        {
            return services.AddTprFrontendUmbraco(configureGovUkOptions, configureGovUkUmbracoOptions, options => { });
        }

        public static IServiceCollection AddTprFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureGovUkOptions,
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
            Action<GovUkFrontendAspNetCoreOptions> configureGovUkOptions,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions,
            Action<TprFrontendOptions> configureTprOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // GovUk.Frontend.Umbraco
            services.AddGovUkFrontendUmbraco(configureGovUkOptions, configureGovUkUmbracoOptions);

            // ThePensionsRegulator.Frontend
            services.AddTransient<IConsentCookieReader, TprConsentCookieReader>();
            services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();

            var tprFrontendOptions = new TprFrontendOptions();
            if (configureTprOptions is not null) { configureTprOptions(tprFrontendOptions); }
            services.AddTransient((services) => Options.Create(tprFrontendOptions));

            // ThePensionsRegulator.Frontend.Umbraco
            services.AddTransient<IPropertyValueFormatter, HostNameInRichTextEditorPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, HostNameInMultiUrlPickerPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphsPropertyValueFormatter>();
            services.AddTransient<IPartialViewPathProvider, TprPartialViewPathProvider>();
            services.AddTransient<IRichTextPropertyEditorAliasProvider, TprRichTextPropertyEditorAliasProvider>();
            services.AddTransient<IBlockViewInterceptor, TprBoxViewInterceptor>();
            services.AddTransient<IBlockViewInterceptor, TprDividerViewInterceptor>();
            services.AddTransient<IDefaultColumnClassProvider, TPRSectionCardsColumnClassProvider>();
            services.AddTransient<IYouTubeVideoIdParser, YouTubeVideoIdParser>();
            services.AddTransient<ITprGlobalNavigationSerivce, TprGlobalNavigationService>();
            services.AddTransient<IContentUrlProvider, ContentUrlProvider>();
            services.AddTransient<IContentVisibilityChecker, ContentVisibilityChecker>();

            return services;
        }
    }
}