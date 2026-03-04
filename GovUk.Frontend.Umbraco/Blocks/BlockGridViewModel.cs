using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Core.Blocks;
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

        /// <summary>
        /// Child columns default to 2/3 width as standard. Set this to <c>true</c> for sub-grids within parent grids.
        /// </summary>
        public bool ChildColumnsDefaultToFullWidth { get; set; }
    }
}
