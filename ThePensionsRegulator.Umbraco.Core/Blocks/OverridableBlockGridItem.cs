using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    public class OverridableBlockGridItem : BlockGridItem, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>
    {
        /// <summary>
        /// Creates block grid items suitable for non-HTTP usage. For HTTP request handling, supply a factory which passes <see cref="IHttpContextAccessor" /> to <see cref="OverridablePublishedElement"/>.
        /// </summary>
        public static Func<IPublishedElement?, IOverridablePublishedElement?> DefaultPublishedElementFactory { get => publishedElement => publishedElement != null ? new OverridablePublishedElement(publishedElement) : null; }

        /// <summary>
        /// Leaves published elements unchanged. This is useful when the supplied elements already implement <see cref="IOverridablePublishedElement"/>, such as in unit tests.
        /// </summary>
        public static Func<IPublishedElement?, IOverridablePublishedElement?> NoopPublishedElementFactory { get => publishedElement => (IOverridablePublishedElement?)publishedElement; }

        private List<OverridableBlockGridArea> _areas = new();

        public OverridableBlockGridItem(BlockGridItem item) : this(item, DefaultPublishedElementFactory) { }

        public OverridableBlockGridItem(BlockGridItem item, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory) :
#nullable disable
            base(item.ContentKey, publishedElementFactory(item.Content), item.SettingsKey, publishedElementFactory(item.Settings))
#nullable enable
        {
            Areas = item.Areas.Select(area => new OverridableBlockGridArea(area, area.Alias, area.RowSpan, area.ColumnSpan)).ToList();
            AreaGridColumns = item.AreaGridColumns;
            GridColumns = item.GridColumns;
            ColumnSpan = item.ColumnSpan;
            RowSpan = item.RowSpan;
        }

        /// <inheritdoc/>
        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        /// <inheritdoc/>
        public new IOverridablePublishedElement? Settings { get => (IOverridablePublishedElement?)base.Settings; }

        /// <inheritdoc/>
        public new IList<OverridableBlockGridArea> Areas
        {
            get
            {
                return _areas;
            }
            set
            {
                _areas = value.ToList();
            }
        }
    }
}
