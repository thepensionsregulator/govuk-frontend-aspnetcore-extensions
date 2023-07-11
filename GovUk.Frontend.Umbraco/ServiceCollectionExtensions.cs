using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.ModelBinding;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using ThePensionsRegulator.Umbraco;

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
            services.AddTransient<IUmbracoPublishedContentAccessor, UmbracoPublishedContentAccessor>();
            services.AddTransient<IUmbracoPaginationFactory, UmbracoPaginationFactory>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, ModelBindingMvcConfiguration>();

            return services;
        }
    }
}