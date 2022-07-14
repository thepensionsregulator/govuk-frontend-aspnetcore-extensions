using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Models
{
    public class OverridableBlockListItem : BlockListItem
    {
        public OverridableBlockListItem(BlockListItem item) :
            base(item.ContentUdi, new OverridablePublishedElement(item.Content), item.SettingsUdi, new OverridablePublishedElement(item.Settings))
        {
        }

        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        public new IOverridablePublishedElement Settings { get => (IOverridablePublishedElement)base.Settings; }
    }
}
