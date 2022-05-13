using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Models
{
    public class FilteredBlockListItem
    {
        public FilteredBlockListItem(BlockListItem item, Func<BlockListModel, IEnumerable<BlockListItem>>? filter)
        {
            Item = item;
            Filter = filter ?? (x => x);
        }

        public Func<BlockListModel, IEnumerable<BlockListItem>> Filter { get; private set; }

        public BlockListItem Item { get; set; }
    }
}
