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
        /// In a block grid, is this block within an area rather than the top-level grid?
        /// </summary>
        public bool IsInGridArea { get; set; }

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
        public string RowClasses { get; set; } = HtmlClassNames.Row;

        /// <summary>
        /// HTML classes to apply to the grid column.
        /// </summary>
        public string ColumnClasses { get; set; } = HtmlClassNames.Column;

        /// <summary>
        /// HTML classes to apply to a fieldset in an error state.
        /// </summary>
        public string? FieldsetErrorClasses { get; set; }

        /// <summary>
        /// Should a div be rendered around this block to contain its width?
        /// </summary>
        public bool RenderWidthContainer { get; set; }

        /// <summary>
        /// If <see cref="RenderWidthContainer"/> is <c>true</c>, what HTML class(es) should be applied to the width container?
        /// </summary>
        public string WidthContainerClasses { get; set; } = HtmlClassNames.WidthContainer;
    }
}
