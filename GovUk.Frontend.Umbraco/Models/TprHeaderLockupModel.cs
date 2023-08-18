using GovUk.Frontend.Umbraco.Typography;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Models
{
    public class TprHeaderLockupModel
    {
        private readonly IPublishedContent _settings;

        public TprHeaderLockupModel(IPublishedContent settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }

        public bool ShowSkipLink { get; init; } = true;
        public bool ShowPhaseBanner { get; init; } = true;
        public bool ShowHeader { get; init; } = true;
        public bool HeaderBarContentAllowHtml { get; init; } = true;
        public bool Context1AllowHtml { get; init; } = true;
        public bool Context2AllowHtml { get; init; } = true;
        public bool Context3AllowHtml { get; init; } = true;
        public virtual string? SkipLinkClass() => null;
        public virtual string? SkipLinkHref() => "#main";
        public virtual string? SkipLinkText() => _settings.Value<string>("govukSkipLinkText");
        public virtual string? PhaseBannerClass() => null;
        public virtual string? Phase() => _settings.Value<string>("govukPhase");
        public virtual string? PhaseBannerText() => GovUkTypography.RemoveWrappingParagraph(_settings.Value<IHtmlEncodedString>("govukPhaseBannerText")).ToHtmlString();
        public virtual string? HeaderBarClass() => null;
        public virtual string? LogoAlternativeText() => _settings.Value<string>("tprHeaderLogoAlt");
        public virtual string? LogoHref() => _settings.Value<Link>("tprHeaderLogoHref")?.Url;
        public virtual string? HeaderBarLabel() => _settings.Value<string?>("tprHeaderLabel");
        public virtual string? HeaderBarContent() => GovUkTypography.RemoveWrappingParagraphs(_settings.Value<IHtmlEncodedString>("tprHeaderContent")).ToHtmlString();
        public virtual string? ContextBarClass() => null;
        public virtual string? Context1() => GovUkTypography.RemoveWrappingParagraphs(_settings.Value<IHtmlEncodedString>("tprContext1")).ToHtmlString();
        public virtual string? Context2() => GovUkTypography.RemoveWrappingParagraphs(_settings.Value<IHtmlEncodedString>("tprContext2")).ToHtmlString();
        public virtual string? Context3() => GovUkTypography.RemoveWrappingParagraphs(_settings.Value<IHtmlEncodedString>("tprContext3")).ToHtmlString();
    }
}
