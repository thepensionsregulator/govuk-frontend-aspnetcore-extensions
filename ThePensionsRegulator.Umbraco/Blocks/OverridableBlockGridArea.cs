using Lucene.Net.Util;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public class OverridableBlockGridArea : OverridableBlockModel<OverridableBlockGridItem>
    {
        public OverridableBlockGridArea(IList<BlockGridItem> list, string alias, int rowSpan, int columnSpan) :
            this(list, alias, rowSpan, columnSpan, OverridableBlockGridItem.DefaultPublishedElementFactory)
        {
        }

        public OverridableBlockGridArea(IList<BlockGridItem> list, string alias, int rowSpan, int columnSpan, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory)
        {
            Alias = alias;
            RowSpan = rowSpan;
            ColumnSpan = columnSpan;
            Items.AddRange(list.Select(item => item as OverridableBlockGridItem ?? new OverridableBlockGridItem(item, publishedElementFactory)));
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
