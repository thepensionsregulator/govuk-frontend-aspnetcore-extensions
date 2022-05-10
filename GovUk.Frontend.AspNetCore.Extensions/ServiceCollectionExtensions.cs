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
            services.AddSingleton<IStartupFilter, EmbedContentFolderStartupFilter>();

            return services;
        }
    }
}
