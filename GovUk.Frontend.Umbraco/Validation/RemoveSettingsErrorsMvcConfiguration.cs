using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.Umbraco.Validation
{
    internal class RemoveSettingsErrorsMvcConfiguration : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.Filters.Add<RemoveSettingsErrorsActionFilter>();
        }
    }
}