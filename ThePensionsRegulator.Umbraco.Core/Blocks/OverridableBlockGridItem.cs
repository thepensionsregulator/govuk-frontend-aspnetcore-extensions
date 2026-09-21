using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    public class OverridableBlockGridItem : BlockGridItem, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>
    {
        /// <summary>
        /// Creates block grid items for compatibility scenarios where no <see cref="IOverridablePublishedElementFactory"/> is supplied.
        /// </summary>
        [Obsolete($"Use a factory which accepts an {nameof(IOverridablePublishedElementFactory)} or {nameof(Func<IPublishedElement?, IOverridablePublishedElement?>)} to support overriding property values.")]
        public static Func<IPublishedElement?, IOverridablePublishedElement?> DefaultPublishedElementFactory { get => publishedElement => publishedElement != null ? new OverridablePublishedElement(publishedElement, StaticServiceProvider.Instance.GetRequiredService<IOverridablePublishedElementValueStore>()) : null; }

        /// <summary>
        /// Leaves published elements unchanged. This is useful when the supplied elements already implement <see cref="IOverridablePublishedElement"/>, such as in unit tests.
        /// </summary>
        public static Func<IPublishedElement?, IOverridablePublishedElement?> NoopPublishedElementFactory { get => publishedElement => (IOverridablePublishedElement?)publishedElement; }

        private List<OverridableBlockGridArea> _areas = new();

        [Obsolete($"Use a factory which accepts an {nameof(IOverridablePublishedElementFactory)} or {nameof(Func<IPublishedElement?, IOverridablePublishedElement?>)} to support overriding property values.")]
        public OverridableBlockGridItem(BlockGridItem item) : this(item, DefaultPublishedElementFactory) { }

        /// <summary>
        /// Creates an <see cref="OverridableBlockGridItem"/> from a read-only <see cref="BlockGridItem"/>.
        /// </summary>
        /// <param name="item">The block grid item to make overridable.</param>
        /// <param name="publishedElementFactory">A factory to wrap published elements (content and settings) to support overriding property values. The factory must use a request-scoped value store.</param>
        public OverridableBlockGridItem(BlockGridItem item, IOverridablePublishedElementFactory publishedElementFactory)
#nullable disable
            : base(item.ContentKey, publishedElementFactory.Create(item.Content), item.SettingsKey, publishedElementFactory.Create(item.Settings))
#nullable enable
        {
            Areas = item.Areas.Select(area => new OverridableBlockGridArea(area, area.Alias, area.RowSpan, area.ColumnSpan, publishedElementFactory)).ToList();
            AreaGridColumns = item.AreaGridColumns;
            GridColumns = item.GridColumns;
            ColumnSpan = item.ColumnSpan;
            RowSpan = item.RowSpan;
        }

        /// <summary>
        /// Creates an <see cref="OverridableBlockGridItem"/> from a read-only <see cref="BlockGridItem"/>.
        /// </summary>
        /// <param name="item">The block grid item to make overridable.</param>
        /// <param name="publishedElementFactory">A factory to wrap published elements (content and settings) to support overriding property values. The factory must use a request-scoped value store.</param>
        public OverridableBlockGridItem(BlockGridItem item, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory) :
            this(item, new DelegatingOverridablePublishedElementFactory(publishedElementFactory))
        { }

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
