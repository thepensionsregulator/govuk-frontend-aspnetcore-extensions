using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.ModelBinding;
using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using GovUk.Frontend.Umbraco.Services;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.PropertyEditors;

namespace GovUk.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontendUmbraco(this IServiceCollection services)
        {
            // Avoid adding scripts which require 'unsafe-inline' in the content security policy
            return services.AddGovUkFrontendUmbraco(options => { options.AddImportsToHtml = false; });
        }

        public static IServiceCollection AddGovUkFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddGovUkFrontendExtensions(configureOptions);

            services.AddTransient<IUmbracoPublishedContentAccessor, UmbracoPublishedContentAccessor>();
            services.AddTransient<IUmbracoPaginationFactory, UmbracoPaginationFactory>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, ModelBindingMvcConfiguration>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, RemoveSettingsErrorsMvcConfiguration>();
            services.AddTransient<IPropertyValueFormatter, GovUkTypographyPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphInversePropertyValueFormatter>();
            services.AddTransient<IPartialViewPathProvider, GovUkPartialViewPathProvider>();

            return services;
        }
    }
}