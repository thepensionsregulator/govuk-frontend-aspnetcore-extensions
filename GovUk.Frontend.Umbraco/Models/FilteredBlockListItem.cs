using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Models
{
    public class FilteredBlockListItem : BlockListItem
    {
        public FilteredBlockListItem(BlockListItem item, Func<IEnumerable<BlockListItem>, IEnumerable<BlockListItem>>? filter) :
            base(item.ContentUdi, new OverridablePublishedElement(item.Content), item.SettingsUdi, new OverridablePublishedElement(item.Settings))
        {
            Filter = filter ?? (x => x);
        }

        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        public new IOverridablePublishedElement Settings { get => (IOverridablePublishedElement)base.Settings; }

        public Func<IEnumerable<BlockListItem>, IEnumerable<BlockListItem>> Filter { get; private set; }
    }
}
