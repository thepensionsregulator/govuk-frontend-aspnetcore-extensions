using GovUk.Frontend.AspNetCore;

namespace ThePensionsRegulator.GovUk.Frontend.Configuration
{
    public class GovUkFrontendOptionsProvider
    {
        private readonly GovUkFrontendOptions _options;

        public GovUkFrontendOptionsProvider(Action<GovUkFrontendOptions> configureOptions)
        {
            _options = new GovUkFrontendOptions();
            configureOptions(_options);
        }

        public GovUkFrontendOptions Options { get => _options; }
    }
}
