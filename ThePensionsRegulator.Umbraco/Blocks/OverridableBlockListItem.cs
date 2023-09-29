using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public class OverridableBlockListItem : BlockListItem
    {
        public static Func<IPublishedElement?, IOverridablePublishedElement?> DefaultPublishedElementFactory { get => publishedElement => publishedElement != null ? new OverridablePublishedElement(publishedElement) : null; }
        public static Func<IPublishedElement?, IOverridablePublishedElement?> NoopPublishedElementFactory { get => publishedElement => (IOverridablePublishedElement?)publishedElement; }

        public OverridableBlockListItem(BlockListItem item) : this(item, DefaultPublishedElementFactory) { }

        public OverridableBlockListItem(BlockListItem item, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory) :
#nullable disable
            base(item.ContentUdi, publishedElementFactory(item.Content), item.SettingsUdi, publishedElementFactory(item.Settings))
#nullable enable
        {

        }

        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        public new IOverridablePublishedElement Settings { get => (IOverridablePublishedElement)base.Settings; }
    }
}
