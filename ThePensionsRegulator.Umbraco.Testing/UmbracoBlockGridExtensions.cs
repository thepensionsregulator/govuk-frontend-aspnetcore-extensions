using ThePensionsRegulator.Umbraco.Core.Blocks;

namespace ThePensionsRegulator.Umbraco.Testing
{
    public static class UmbracoBlockGridExtensions
    {

        /// <summary>
        /// Fluent interface to add an <see cref="OverridableBlockGridArea"/> to a block in an overridable block grid.
        /// </summary>
        public static OverridableBlockGridItem AddArea(this OverridableBlockGridItem blockGridItem, OverridableBlockGridArea area)
        {
            blockGridItem.Areas.Add(area);
            return blockGridItem;
        }
    }
}
