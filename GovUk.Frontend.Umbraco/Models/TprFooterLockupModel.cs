﻿using GovUk.Frontend.Umbraco.Typography;
using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Models
{
    public class TprFooterLockupModel
    {
        private readonly IPublishedContent _settings;

        public TprFooterLockupModel(IPublishedContent settings)
        {
            _settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        }
        public bool ShowBackToTop { get; init; } = true;
        public bool ShowFooter { get; init; } = true;
        public virtual string? BackToTopClass() => null;
        public virtual string? BackToTopText() => string.IsNullOrEmpty(_settings.Value<string>("tprBackToTopText")) ? "Back to top" : _settings.Value<string>("tprBackToTopText");
        public virtual string? FooterBarClass() => null;
        public virtual string? LogoAlternativeText() => _settings.Value<string>("tprFooterLogoAlt");
        public virtual string? LogoHref() => _settings.Value<Link>("tprFooterLogoHref")?.Url;
        public virtual string? Copyright() => _settings.Value<string?>("tprFooterCopyright")?.Replace("{{year}}", DateTimeOffset.UtcNow.Year.ToString());
        public virtual string? FooterBarContent() => GovUkTypography.RemoveWrappingParagraphs(_settings.Value<IHtmlEncodedString>("tprFooterContent")).ToHtmlString();
    }
}
