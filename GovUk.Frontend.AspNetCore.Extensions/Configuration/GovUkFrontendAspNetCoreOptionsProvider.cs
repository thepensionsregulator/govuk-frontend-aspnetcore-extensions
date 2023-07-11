using System;

namespace GovUk.Frontend.AspNetCore.Extensions.Configuration
{
    public class GovUkFrontendAspNetCoreOptionsProvider
    {
        private readonly GovUkFrontendAspNetCoreOptions _options;

        public GovUkFrontendAspNetCoreOptionsProvider(Action<GovUkFrontendAspNetCoreOptions> configureOptions)
        {
            _options = new GovUkFrontendAspNetCoreOptions();
            configureOptions(_options);
        }

        public GovUkFrontendAspNetCoreOptions Options { get => _options; }
    }
}
