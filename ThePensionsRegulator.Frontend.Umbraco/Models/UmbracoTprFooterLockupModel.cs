using GovUk.Frontend.Umbraco;
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Umbraco.Blocks;
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

            ThreeColumnLinks = InitializeThreeColumnLinks();
        }

        private IEnumerable<IEnumerable<TprFooterLink>> InitializeThreeColumnLinks()
        {
            var columns = new List<IEnumerable<TprFooterLink>>();
            var columnBlock = _settings?.Value<OverridableBlockGridModel>(TprPropertyAliases.FooterLinks)?.FirstOrDefault(i => i.Content.ContentType.Alias == ElementTypeAliases.GridThreeEqualColumns);
            if (columnBlock is null) { return columns; }

            foreach (var links in columnBlock.Areas)
            {
                var column = new List<TprFooterLink>();
                foreach (var link in links)
                {
                    var url = link.Content.Value<Link>("link")?.Url;
                    var text = link.Content.Value<string>("text");

                    if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(text))
                    {
                        column.Add(new TprFooterLink
                        {
                            Url = url,
                            Name = text
                        });
                    }
                }
                if (column.Any()) { columns.Add(column); }
            }
            return columns;
        }

        public override string BackToTopText => string.IsNullOrEmpty(_settings.Value<string>("tprBackToTopText")) ? "Back to top" : _settings.Value<string>("tprBackToTopText")!;
        public override string? LogoAlternativeText => _settings.Value<string>("tprFooterLogoAlt");
        public override string? LogoHref => _settings.Value<Link>("tprFooterLogoHref")?.Url;
        public override string? Copyright => _settings.Value<string?>("tprFooterCopyright")?.Replace("{{year}}", DateTimeOffset.UtcNow.Year.ToString());
        public override string? FooterBarContent => _settings.Value<IHtmlEncodedString>("tprFooterContent")?.ToHtmlString();
    }
}
