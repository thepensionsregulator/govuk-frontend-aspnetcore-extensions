using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFileTypeValidators(this IServiceCollection services)
        {
            services.AddScoped<IFileTypeValidator, Excel>();
            services.AddScoped<IFileTypeValidator, Pdf>();
            return services;
        }
    }
}