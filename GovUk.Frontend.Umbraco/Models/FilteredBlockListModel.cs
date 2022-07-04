using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Models
{
    public class FilteredBlockListModel
    {
        private readonly BlockListModel _model;

        public FilteredBlockListModel(BlockListModel model, Func<BlockListModel, IEnumerable<BlockListItem>>? filter = null)
        {
            _model = model;
            Filter = filter ?? (x => x);
        }

        public Func<BlockListModel, IEnumerable<BlockListItem>> Filter { get; private set; }

        public IEnumerable<BlockListItem> FilteredBlocks()
        {
            return Filter(_model);
        }

        public bool RenderGrid { get; set; } = true;
    }
}
