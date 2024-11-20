using System;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
    /// <summary>
    /// Settings for rendering an individual block in a block grid or block list.
    /// </summary>
    public class BlockViewModel
    {
        /// <summary>
        /// The block to render.
        /// </summary>
        public required IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> Block { get; set; }

        /// <summary>
        /// In a block grid, this block contains at least one block area.
        /// </summary>
        public bool HasGridAreas { get; set; }

        /// <summary>
        /// In a block list, should the grid row and column be rendered, or only their children?
        /// </summary>
        public bool RenderGrid { get; set; }

        /// <summary>
        /// In a block list, does this block represent a grid row.
        /// </summary>
        [Obsolete("Multi-column layouts in block list are deprecated. Use block grid for multi-column layouts.")]
        public bool IsGridRow { get; set; }

        /// <summary>
        /// Should this grid row be merged into the previous grid row?
        /// </summary>
        public bool IsSameAsPrevious { get; set; }

        /// <summary>
        /// Should this grid row be merged into the next grid row?
        /// </summary>
        public bool IsSameAsNext { get; set; }

        /// <summary>
        /// HTML classes to apply to the grid row.
        /// </summary>
        public required string RowClasses { get; set; }

        /// <summary>
        /// HTML classes to apply to the grid column.
        /// </summary>
        public required string ColumnClasses { get; set; }

        /// <summary>
        /// HTML classes to apply to a fieldset in an error state.
        /// </summary>
        public string? FieldsetErrorClasses { get; set; }
    }
}
