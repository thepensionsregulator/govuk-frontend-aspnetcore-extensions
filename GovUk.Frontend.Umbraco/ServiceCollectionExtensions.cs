using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.ModelBinding;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
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
            services.AddMvc(options =>
            {
                // Replace the custom date model binder from the base project with a copy that has
                // been modified to make the error messages configurable in Umbraco
                var govukDateBinder = options.ModelBinderProviders.FirstOrDefault(x => x.GetType().FullName == "GovUk.Frontend.AspNetCore.ModelBinding.DateInputModelBinderProvider");
                if (govukDateBinder != null) { options.ModelBinderProviders.Remove(govukDateBinder); }
                var govukOptions = new GovUkFrontendAspNetCoreOptions();
                configureOptions(govukOptions);
                options.ModelBinderProviders.Insert(0, new UmbracoDateInputModelBinderProvider(govukOptions, null));
            });

            return services;
        }
    }
}