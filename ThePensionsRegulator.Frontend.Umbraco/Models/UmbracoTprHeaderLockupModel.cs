using Microsoft.Extensions.DependencyInjection;
using ThePensionsRegulator.Frontend.Models;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class UmbracoTprHeaderLockupModel : TprHeaderLockupModel
    {
        private readonly IPublishedContent _settings;
        private readonly IPublishedValueFallback _publishedValueFallback;

        [Obsolete("Use the constructor that accepts IPublishedValueFallback.")]
        public UmbracoTprHeaderLockupModel(IPublishedContent settings)
            : this(settings, StaticServiceProvider.Instance.GetRequiredService<IPublishedValueFallback>())
        {
        }

        public UmbracoTprHeaderLockupModel(IPublishedContent settings, IPublishedValueFallback publishedValueFallback)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
            _publishedValueFallback = publishedValueFallback ?? throw new System.ArgumentNullException(nameof(publishedValueFallback));
        }

        public override string SkipLinkText => string.IsNullOrEmpty(_settings.Value<string>(_publishedValueFallback, "govukSkipLinkText")) ? "Skip to main content" : _settings.Value<string>(_publishedValueFallback, "govukSkipLinkText")!;
        public override string? Phase => _settings.Value<string>(_publishedValueFallback, "govukPhase");
        public override string? PhaseBannerText => _settings.Value<IHtmlEncodedString>(_publishedValueFallback, "govukPhaseBannerText")?.ToHtmlString();
        public override string? LogoAlternativeText => _settings.Value<string>(_publishedValueFallback, "tprHeaderLogoAlt");
        public override string? LogoHref => _settings.Value<Link>(_publishedValueFallback, "tprHeaderLogoHref")?.Url;
        public override string? HeaderBarLabel => _settings.Value<string?>(_publishedValueFallback, "tprHeaderLabel");
        public override string? HeaderBarContent => _settings.Value<IHtmlEncodedString>(_publishedValueFallback, "tprHeaderContent")?.ToHtmlString();
        public override string? Context1 => _settings.Value<IHtmlEncodedString>(_publishedValueFallback, "tprContext1")?.ToHtmlString();
        public override string? Context2 => _settings.Value<IHtmlEncodedString>(_publishedValueFallback, "tprContext2")?.ToHtmlString();
        public override string? Context3 => _settings.Value<IHtmlEncodedString>(_publishedValueFallback, "tprContext3")?.ToHtmlString();
        public override bool ShowSearch => _settings.Value<bool?>(_publishedValueFallback, "tprHeaderShowSearch") ?? false;  
        public override string? HeaderSearchPlaceholderText => _settings.Value<string>(_publishedValueFallback, "tprHeaderSearchPlaceholderText");
        public override string? HeaderSearchAriaLabel => _settings.Value<string>(_publishedValueFallback, "tprHeaderSearchAriaLabel");
        public override bool ShowHeaderMenu => _settings.Value<bool>(_publishedValueFallback, "showHeaderMenu");
        public override string? HeaderMenuAriaLabel => _settings.Value<string>(_publishedValueFallback, "tprHeaderAriaLabelText");
        public override string? HeaderMenuItemAriaLabel => _settings.Value<string>(_publishedValueFallback, "tprHeaderMenuAriaLabel");
        public override string? HeaderMenuToggleOpen => _settings.Value<string>(_publishedValueFallback, "tprHeaderMenuToggleOpenText");
        public override string? HeaderMenuToggleClosed => _settings.Value<string>(_publishedValueFallback, "tprHeaderMenuToggleClosedText");
        public override string? HeaderMenuNoJSNavPage => _settings.Value<string>(_publishedValueFallback, "noJSNavPage");
    }
}
