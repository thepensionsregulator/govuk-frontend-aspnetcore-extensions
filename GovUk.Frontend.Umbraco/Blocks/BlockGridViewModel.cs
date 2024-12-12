using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public class BlockGridViewModel : List<BlockGridItem>
    {
        public OverridableBlockGridModel? BlockGrid { get; private init; }

        public BlockGridViewModel(IEnumerable<BlockGridItem> blocks) : base(blocks)
        {

            if (blocks is OverridableBlockGridModel blockGrid) { BlockGrid = blockGrid; }
        }

        /// <summary>
        /// Gets or sets whether a width container should be rendered for these blocks, if rendering width containers is enabled.
        /// </summary>
        public bool RenderWidthContainer { get; set; } = true;
    }
}
