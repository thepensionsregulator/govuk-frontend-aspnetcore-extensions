using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.PropertyEditors;

namespace ThePensionsRegulator.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services)
        {
            // Avoid adding scripts which require 'unsafe-inline' in the content security policy
            return services.AddTprFrontendUmbraco(options => { options.AddImportsToHtml = false; });
        }

        public static IServiceCollection AddTprFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddGovUkFrontendUmbraco(configureOptions);

            services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();
            services.AddTransient<IPropertyValueFormatter, HostNameInRichTextEditorPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, HostNameInMultiUrlPickerPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphsPropertyValueFormatter>();
            services.AddTransient<IPartialViewPathProvider, TprPartialViewPathProvider>();

            return services;
        }
    }
}