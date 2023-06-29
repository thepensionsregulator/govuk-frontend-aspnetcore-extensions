using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.BlockLists
{
    public class OverridableBlockListItem : BlockListItem
    {
        public static Func<IPublishedElement, IOverridablePublishedElement> DefaultPublishedElementFactory { get => publishedElement => new OverridablePublishedElement(publishedElement); }

        public OverridableBlockListItem(BlockListItem item) : this(item, DefaultPublishedElementFactory) { }

        public OverridableBlockListItem(BlockListItem item, Func<IPublishedElement, IOverridablePublishedElement> publishedElementFactory) :
            base(item.ContentUdi, publishedElementFactory(item.Content), item.SettingsUdi, publishedElementFactory(item.Settings))
        {

        }

        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        public new IOverridablePublishedElement Settings { get => (IOverridablePublishedElement)base.Settings; }
    }
}
