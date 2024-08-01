using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using Microsoft.Extensions.DependencyInjection;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFileTypeValidators(this IServiceCollection services)
        {
            var validators = new FileValidatorCollection().GetValidators();
            foreach (var v in validators)
            {
                services.AddScoped(typeof(IFileTypeValidator), v.GetType());
            }

            return services;
        }
    }
}