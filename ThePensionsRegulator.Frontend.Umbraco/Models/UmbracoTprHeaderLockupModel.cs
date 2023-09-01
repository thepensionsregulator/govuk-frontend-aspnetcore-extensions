using ThePensionsRegulator.Frontend.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class UmbracoTprHeaderLockupModel : TprHeaderLockupModel
    {
        private readonly IPublishedContent _settings;

        public UmbracoTprHeaderLockupModel(IPublishedContent settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }

        public override string? SkipLinkText => _settings.Value<string>("govukSkipLinkText");
        public override string? Phase => _settings.Value<string>("govukPhase");
        public override string? PhaseBannerText => _settings.Value<IHtmlEncodedString>("govukPhaseBannerText")?.ToHtmlString();
        public override string? LogoAlternativeText => _settings.Value<string>("tprHeaderLogoAlt");
        public override string? LogoHref => _settings.Value<Link>("tprHeaderLogoHref")?.Url;
        public override string? HeaderBarLabel => _settings.Value<string?>("tprHeaderLabel");
        public override string? HeaderBarContent => _settings.Value<IHtmlEncodedString>("tprHeaderContent")?.ToHtmlString();
        public override string? Context1 => _settings.Value<IHtmlEncodedString>("tprContext1")?.ToHtmlString();
        public override string? Context2 => _settings.Value<IHtmlEncodedString>("tprContext2")?.ToHtmlString();
        public override string? Context3 => _settings.Value<IHtmlEncodedString>("tprContext3")?.ToHtmlString();
    }
}
