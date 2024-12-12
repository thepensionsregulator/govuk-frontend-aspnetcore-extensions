using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public class BlockListViewModel : List<BlockListItem>
    {
        public OverridableBlockListModel? BlockList { get; private init; }

        public BlockListViewModel(IEnumerable<BlockListItem> blocks) : base(blocks)
        {

            if (blocks is OverridableBlockListModel blockList) { BlockList = blockList; }
        }

        /// <summary>
        /// Gets or sets whether a width container should be rendered for these blocks, if rendering width containers is enabled.
        /// </summary>
        public bool RenderWidthContainer { get; set; } = true;
    }
}
