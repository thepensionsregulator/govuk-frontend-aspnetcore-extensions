using Microsoft.Extensions.DependencyInjection;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class UmbracoTprFooterLockupModel : TprFooterLockupModel
    {
        private readonly IPublishedContent _settings;
        private readonly IPublishedValueFallback _publishedValueFallback;

        [Obsolete("Use the constructor that accepts IPublishedValueFallback.")]
        public UmbracoTprFooterLockupModel(IPublishedContent settings)
            : this(settings, StaticServiceProvider.Instance.GetRequiredService<IPublishedValueFallback>())
        {
        }

        public UmbracoTprFooterLockupModel(IPublishedContent settings, IPublishedValueFallback publishedValueFallback)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _publishedValueFallback = publishedValueFallback ?? throw new ArgumentNullException(nameof(publishedValueFallback));

            ThreeColumnLinks = InitializeThreeColumnLinks();
        }

        private IEnumerable<IEnumerable<TprFooterLink>> InitializeThreeColumnLinks()
        {
            var columns = new List<IEnumerable<TprFooterLink>>();
            var columnBlock = _settings?.Value<OverridableBlockGridModel>(_publishedValueFallback, TprPropertyAliases.FooterLinks)?.FirstOrDefault(i => i.Content.ContentType.Alias == ElementTypeAliases.GridThreeEqualColumns);
            if (columnBlock is null) { return columns; }

            foreach (var links in columnBlock.Areas)
            {
                var column = new List<TprFooterLink>();
                foreach (var link in links)
                {
                    var url = link.Content.Value<Link>(_publishedValueFallback, "link")?.Url;
                    var text = link.Content.Value<string>(_publishedValueFallback, "text");

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

        public override string BackToTopText => string.IsNullOrEmpty(_settings.Value<string>(_publishedValueFallback, "tprBackToTopText")) ? "Back to top" : _settings.Value<string>(_publishedValueFallback, "tprBackToTopText")!;
        public override string? LogoAlternativeText => _settings.Value<string>(_publishedValueFallback, "tprFooterLogoAlt");
        public override string? LogoHref => _settings.Value<Link>(_publishedValueFallback, "tprFooterLogoHref")?.Url;
        public override string? Copyright => _settings.Value<string?>(_publishedValueFallback, "tprFooterCopyright")?.Replace("{{year}}", DateTimeOffset.UtcNow.Year.ToString());
        public override string? FooterBarContent => _settings.Value<IHtmlEncodedString>(_publishedValueFallback, "tprFooterContent")?.ToHtmlString();
    }
}
