using GovUk.Frontend.AspNetCore.Extensions.Configuration;
using GovUk.Frontend.AspNetCore.Extensions.ModelBinding;
using GovUk.Frontend.AspNetCore.Extensions.Security;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontendExtensions(this IServiceCollection services)
        {
            return services.AddGovUkFrontendExtensions(_ => { });
        }

        public static IServiceCollection AddGovUkFrontendExtensions(
            this IServiceCollection services,
            Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddGovUkFrontend(configureOptions);
            services.AddTransient<IClientSideValidationHtmlEnhancer, ClientSideValidationHtmlEnhancer>();
            services.AddTransient<IModelPropertyResolver, ModelPropertyResolver>();
            services.AddScoped<INonceProvider, NonceProvider>();
            services.AddSingleton<IStartupFilter, EmbedContentFolderStartupFilter>();
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new NormalisedStringModelBinderProvider());
                options.ModelBinderProviders.Insert(0, new UkPostcodeModelBinderProvider());
            });
            services.AddSingleton(new GovUkFrontendAspNetCoreOptionsProvider(configureOptions));

            return services;
        }
    }
}
