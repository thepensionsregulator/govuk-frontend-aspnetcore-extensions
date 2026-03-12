using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ThePensionsRegulator.GovUk.Frontend.Caching;
using ThePensionsRegulator.GovUk.Frontend.Configuration;
using ThePensionsRegulator.GovUk.Frontend.ModelBinding;
using ThePensionsRegulator.GovUk.Frontend.Security;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprGovUkFrontend(this IServiceCollection services)
        {
            return services.AddGovUkFrontendExtensions(options => { });
        }

        public static IServiceCollection AddGovUkFrontendExtensions(
            this IServiceCollection services,
            Action<GovUkFrontendOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Action<GovUkFrontendOptions> configureOptionsWithDefaults = opt =>
            {
                opt.ErrorSummaryGeneration = ErrorSummaryGenerationOptions.None;
                opt.DefaultFileUploadJavaScriptEnhancements = true;
                configureOptions(opt);
            };

            services.AddGovUkFrontend(configureOptionsWithDefaults);
            services.AddTransient<IClientSideValidationHtmlEnhancer, ClientSideValidationHtmlEnhancer>();
            services.AddTransient<ModelPropertyResolverBase, ModelTypeAttributeModelPropertyResolver>();
            services.AddTransient<ModelPropertyResolverBase, DefaultModelPropertyResolver>();
            services.AddTransient<IModelPropertyResolverCollection, ModelPropertyResolverCollection>();
            services.AddScoped<INonceProvider, NonceProvider>();
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new NormalisedStringModelBinderProvider());
                options.ModelBinderProviders.Insert(0, new UkPostcodeModelBinderProvider());
            });
            services.AddSingleton(new GovUkFrontendOptionsProvider(configureOptionsWithDefaults));
            services.AddTransient<IStaticFileCachePolicy, GovUkStaticFileCachePolicy>();

            return services;
        }
    }
}