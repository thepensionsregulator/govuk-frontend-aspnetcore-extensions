using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprHeaderLockupModel
    {
        public virtual bool ShowSkipLink { get; init; } = true;
        public virtual bool ShowPhaseBanner { get; init; } = true;
        public virtual bool ShowHeader { get; init; } = true;
        public virtual bool HeaderBarContentAllowHtml { get; init; } = true;
        public virtual bool Context1AllowHtml { get; init; } = true;
        public virtual bool Context2AllowHtml { get; init; } = true;
        public virtual bool Context3AllowHtml { get; init; } = true;
        public virtual string? SkipLinkClass { get; set; }
        public virtual string? SkipLinkHref { get; set; } = "#main";
        public virtual string SkipLinkText { get; set; } = "Skip to main content";
        public virtual string? PhaseBannerClass { get; set; }
        public virtual string? Phase { get; set; }
        public virtual string? PhaseBannerText { get; set; }
        public virtual string? HeaderBarClass { get; set; }
        public virtual string? LogoAlternativeText { get; set; } = ComponentGenerator.HeaderLogoDefaultAlt;
        public virtual string? LogoHref { get; set; } = ComponentGenerator.HeaderLogoDefaultHref;
        public virtual string? HeaderBarLabel { get; set; }
        public virtual string? HeaderBarContent { get; set; }
        public virtual string? ContextBarClass { get; set; }
        public virtual string? Context1 { get; set; }
        public virtual string? Context2 { get; set; }
        public virtual string? Context3 { get; set; }
    }
}
