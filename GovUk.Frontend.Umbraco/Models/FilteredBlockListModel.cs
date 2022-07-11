using System;
using System.Collections;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Models
{
    public class FilteredBlockListModel : IEnumerable<FilteredBlockListItem>
    {
        private readonly List<FilteredBlockListItem> _items = new();

        public FilteredBlockListModel(BlockListModel model, Func<IEnumerable<BlockListItem>, IEnumerable<BlockListItem>>? filter = null)
        {
            Filter = filter ?? (x => x);
            foreach (var item in model)
            {
                _items.Add(new FilteredBlockListItem(item, Filter));
            }
        }

        public Func<IEnumerable<BlockListItem>, IEnumerable<BlockListItem>> Filter { get; private set; }

        public IEnumerable<FilteredBlockListItem> FilteredBlocks()
        {
            return (IEnumerable<FilteredBlockListItem>)Filter(_items);
        }

        /// <inheritdoc />
        public IEnumerator<FilteredBlockListItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }

        public bool RenderGrid { get; set; } = true;
    }
}
