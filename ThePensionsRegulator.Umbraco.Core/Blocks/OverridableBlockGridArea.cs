using Lucene.Net.Util;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    public class OverridableBlockGridArea : OverridableBlockModel<OverridableBlockGridItem>
    {
        [Obsolete($"Use the constructor that accepts an {nameof(IOverridablePublishedElementFactory)} to support overriding property values.")]
        public OverridableBlockGridArea(IList<BlockGridItem> list, string alias, int rowSpan, int columnSpan) :
            this(list, alias, rowSpan, columnSpan, OverridableBlockGridItem.DefaultPublishedElementFactory)
        {
        }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridArea"/> with the specified blocks.
        /// </summary>
        /// <param name="list">The blocks to include in the grid area.</param>
        /// <param name="alias">The area alias.</param>
        /// <param name="rowSpan">The number of rows this area should span.</param>
        /// <param name="columnSpan">The number of columns this area should span.</param>
        /// <param name="publishedElementFactory">A factory to wrap published elements to support overriding property values. The factory must use a request-scoped value store.</param>
        public OverridableBlockGridArea(IEnumerable<BlockGridItem> list, string alias, int rowSpan, int columnSpan, IOverridablePublishedElementFactory publishedElementFactory)
        {
            Alias = alias;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
            Items.AddRange(list.Select(item => item as OverridableBlockGridItem ?? new OverridableBlockGridItem(item, publishedElementFactory)));
        }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridArea"/> with the specified blocks.
        /// </summary>
        /// <param name="list">The blocks to include in the grid area.</param>
        /// <param name="alias">The area alias.</param>
        /// <param name="rowSpan">The number of rows this area should span.</param>
        /// <param name="columnSpan">The number of columns this area should span.</param>
        /// <param name="publishedElementFactory">A factory to wrap published elements to support overriding property values. The factory must use a request-scoped value store.</param>
        public OverridableBlockGridArea(IEnumerable<BlockGridItem> list, string alias, int rowSpan, int columnSpan, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory) :
            this(list, alias, rowSpan, columnSpan, new DelegatingOverridablePublishedElementFactory(publishedElementFactory))
        {
        }


        /// <summary>
        /// The area alias.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// The number of rows this area should span.
        /// </summary>
        public int RowSpan { get; }

        /// <summary>
        /// The number of columns this area should span.
        /// </summary>
        public int ColumnSpan { get; }

        /// <summary>
        /// The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.
        /// </summary>
        public Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> Filter
        {
            get { return BaseFilter; }
            set
            {
                BaseFilter = value;

                CopyFilterToDescendantBlockLists(Items, BaseFilter);
            }
        }

        /// <summary>
        /// Gets the block grid with items not matching <see cref="Filter"/> removed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OverridableBlockGridItem> FilteredBlocks()
        {
            return Items.Where(Filter).Select(block => block as OverridableBlockGridItem).OfType<OverridableBlockGridItem>();
        }
    }
}
