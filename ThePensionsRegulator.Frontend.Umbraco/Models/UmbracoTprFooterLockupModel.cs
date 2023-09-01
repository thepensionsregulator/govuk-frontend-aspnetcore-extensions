using System;
using ThePensionsRegulator.Frontend.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class UmbracoTprFooterLockupModel : TprFooterLockupModel
    {
        private readonly IPublishedContent _settings;

        public UmbracoTprFooterLockupModel(IPublishedContent settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
        public override string BackToTopText => string.IsNullOrEmpty(_settings.Value<string>("tprBackToTopText")) ? "Back to top" : _settings.Value<string>("tprBackToTopText")!;
        public override string? LogoAlternativeText => _settings.Value<string>("tprFooterLogoAlt");
        public override string? LogoHref => _settings.Value<Link>("tprFooterLogoHref")?.Url;
        public override string? Copyright => _settings.Value<string?>("tprFooterCopyright")?.Replace("{{year}}", DateTimeOffset.UtcNow.Year.ToString());
        public override string? FooterBarContent => _settings.Value<IHtmlEncodedString>("tprFooterContent")?.ToHtmlString();
    }
}
