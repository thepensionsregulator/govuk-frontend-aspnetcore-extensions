using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public static class ServiceCollectionExtension
    {
        public static ServiceCollection AddFileTypeValidators(this ServiceCollection services)
        {
            services.AddScoped<IFileTypeValidator, Excel>();
            return services;
        }
    }
}