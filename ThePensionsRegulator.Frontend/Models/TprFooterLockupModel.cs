using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprFooterLockupModel
    {
        public virtual bool ShowBackToTop { get; init; } = true;
        public virtual bool ShowFooter { get; init; } = true;
        public virtual string? BackToTopClass { get; init; }
        public virtual string BackToTopText { get; init; } = ComponentGenerator.BackToTopLinkDefaultContent;
        public virtual string? FooterBarClass { get; init; }
        public virtual string? LogoAlternativeText { get; init; } = ComponentGenerator.FooterLogoDefaultAlt;
        public virtual string? LogoHref { get; init; } = ComponentGenerator.FooterLogoDefaultHref;
        public virtual string? Copyright { get; init; }
        public virtual string? FooterBarContent { get; init; }
    }
}
