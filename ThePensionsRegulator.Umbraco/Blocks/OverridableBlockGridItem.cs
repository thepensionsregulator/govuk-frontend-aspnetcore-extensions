using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public class OverridableBlockGridItem : BlockGridItem, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>
    {
        public static Func<IPublishedElement?, IOverridablePublishedElement?> DefaultPublishedElementFactory { get => publishedElement => publishedElement != null ? new OverridablePublishedElement(publishedElement) : null; }
        public static Func<IPublishedElement?, IOverridablePublishedElement?> NoopPublishedElementFactory { get => publishedElement => (IOverridablePublishedElement?)publishedElement; }

        public OverridableBlockGridItem(BlockGridItem item) : this(item, DefaultPublishedElementFactory) { }

        public OverridableBlockGridItem(BlockGridItem item, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory) :
#nullable disable
            base(item.ContentUdi, publishedElementFactory(item.Content), item.SettingsUdi, publishedElementFactory(item.Settings))
#nullable enable
        {
            Areas = item.Areas;
            AreaGridColumns = item.AreaGridColumns;
            GridColumns = item.GridColumns;
            ColumnSpan = item.ColumnSpan;
            RowSpan = item.RowSpan;
        }

        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        public new IOverridablePublishedElement Settings { get => (IOverridablePublishedElement)base.Settings; }
    }
}
