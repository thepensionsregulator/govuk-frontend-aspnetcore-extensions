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
        /// The block before the one to render.
        /// </summary>
        public IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? PreviousBlock { get; set; }

        /// <summary>
        /// The block to render.
        /// </summary>
        public required IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> CurrentBlock { get; set; }

        /// <summary>
        /// The block after the one to render.
        /// </summary>
        public IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? NextBlock { get; set; }

        /// <summary>
        /// In a block grid, is this block within an area rather than the top-level grid?
        /// </summary>
        public bool IsInGridArea { get; set; }

        /// <summary>
        /// Should the grid row and column be rendered?
        /// </summary>
        public bool RenderGridRowAndColumn { get; set; }

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
        /// Should an opening &lt;div&gt; be rendered before this block to contain its width?
        /// </summary>
        public bool OpenWidthContainer { get; set; }

        /// <summary>
        /// Should a closing &lt;/div&gt; be rendered after this block to contain its width?
        /// </summary>
        public bool CloseWidthContainer { get; set; }

        /// <summary>
        /// If <see cref="OpenWidthContainer"/> is <c>true</c>, what HTML class(es) should be applied to the width container?
        /// </summary>
        public string WidthContainerClasses { get; set; } = HtmlClassNames.WidthContainer;
    }
}
