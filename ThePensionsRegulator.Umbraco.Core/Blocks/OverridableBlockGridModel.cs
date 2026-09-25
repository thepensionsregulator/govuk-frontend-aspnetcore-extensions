using System.ComponentModel;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <summary>
    /// An adapter for a <see cref="BlockGridModel" /> which supports filtering out blocks and overriding property values
    /// </summary>
    [TypeConverter(typeof(OverridableBlockGridTypeConverter))]
    public class OverridableBlockGridModel : OverridableBlockModel<OverridableBlockGridItem>
    {
        private IEnumerable<IPropertyValueFormatter>? _propertyValueFormatters;

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/> with no items.
        /// </summary>
        [Obsolete($"Use a constructor which accepts an {nameof(IPublishedValueFallback)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockGridModel() : this(Array.Empty<BlockGridItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/> with no items.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block grid items.</param>
        [Obsolete($"Use a constructor which accepts an {nameof(IOverridablePublishedElementFactory)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockGridModel(IPublishedValueFallback publishedValueFallback) : this(publishedValueFallback, Array.Empty<BlockGridItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/> with no items.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block grid items.</param>
        /// <param name="publishedElementFactory">Factory used to create elements using a request-scoped value store. Request scope is required to support overriding property values.</param>
        /// <param name="filterStoreAccessor">Accessor for the current request-scoped store for block filters.</param>
        public OverridableBlockGridModel(IPublishedValueFallback publishedValueFallback, IOverridablePublishedElementFactory publishedElementFactory, IOverridableBlockModelFilterStoreAccessor filterStoreAccessor)
            : this(publishedValueFallback, publishedElementFactory, filterStoreAccessor, Array.Empty<BlockGridItem>())
        {
        }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/>.
        /// </summary>
        /// <param name="blockGridItems">A block grid (typically a <see cref="BlockGridModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory used to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        [Obsolete($"Use a constructor which accepts an {nameof(IOverridablePublishedElementFactory)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockGridModel(
            IEnumerable<BlockGridItem> blockGridItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(null, blockGridItems, filter, publishedElementFactory is not null ? new DelegatingOverridablePublishedElementFactory(publishedElementFactory) : null);

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/>.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block grid items.</param>
        /// <param name="blockGridItems">A block grid (typically a <see cref="BlockGridModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory used to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        [Obsolete($"Use a constructor which accepts an {nameof(IOverridablePublishedElementFactory)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockGridModel(
            IPublishedValueFallback publishedValueFallback,
            IEnumerable<BlockGridItem> blockGridItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(publishedValueFallback, blockGridItems, filter, publishedElementFactory is not null ? new DelegatingOverridablePublishedElementFactory(publishedElementFactory) : null);

        /// <summary>
        /// Creates an <see cref="OverridableBlockGridModel"/> whose content and settings elements are created using the supplied <see cref="IOverridablePublishedElementFactory">.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block grid items.</param>
        /// <param name="publishedElementFactory">Factory used to create elements using a request-scoped value store. Request scope is required to support overriding property values.</param>
        /// <param name="blockGridItems">A block grid (typically a <see cref="BlockGridModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="filterStoreAccessor">Accessor for the current request-scoped store for block filters.</param>
        public OverridableBlockGridModel(
            IPublishedValueFallback publishedValueFallback,
            IOverridablePublishedElementFactory publishedElementFactory,
            IOverridableBlockModelFilterStoreAccessor filterStoreAccessor,
            IEnumerable<BlockGridItem> blockGridItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null)
            => Initialise(
                publishedValueFallback,
                blockGridItems,
                filter,
                publishedElementFactory,
                filterStoreAccessor);

        private void Initialise(IPublishedValueFallback? publishedValueFallback, IEnumerable<BlockGridItem> blockGridItems, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter, IOverridablePublishedElementFactory? publishedElementFactory, IOverridableBlockModelFilterStoreAccessor? filterStoreAccessor = null)
        {
            if (blockGridItems is null)
            {
                throw new ArgumentNullException(nameof(blockGridItems));
            }

            filterStoreAccessor ??= ResolveFromStaticServiceProvider<IOverridableBlockModelFilterStoreAccessor>();
            InitialiseFilterStoreAccessor(filterStoreAccessor);

            if (blockGridItems is BlockGridModel grid)
            {
                GridColumns = grid.GridColumns;
            }

            // Take the IEnumerable<BlockGridItem> (which is probably a BlockGridModel) and convert each item to an OverridableBlockGridItem,
            // and each nested block grid or block list to an OverridableBlockGridModel or OverridableBlockListModel
            // populated with OverridableBlockGridItems or OverridableBlockListItems.
            if (blockGridItems.Any())
            {
                publishedValueFallback ??= ResolveFromStaticServiceProvider<IPublishedValueFallback>();
                publishedElementFactory ??= ResolveFromStaticServiceProvider<IOverridablePublishedElementFactory>();

                foreach (var item in blockGridItems)
                {
                    var overridableItem = item as OverridableBlockGridItem ?? new OverridableBlockGridItem(item, publishedElementFactory, filterStoreAccessor);
                    foreach (var property in overridableItem.Content.Properties)
                    {
                        ConvertBlockModelPropertyToOverridable<BlockGridModel, OverridableBlockGridModel>(
                            publishedValueFallback,
                            Constants.PropertyEditors.Aliases.BlockGrid,
                            overridableItem,
                            property,
                            blockGrid => new OverridableBlockGridModel(publishedValueFallback, publishedElementFactory, filterStoreAccessor, (IEnumerable<BlockGridItem>)blockGrid, filter),
                            () => new OverridableBlockGridModel(publishedValueFallback, publishedElementFactory, filterStoreAccessor, Array.Empty<BlockGridItem>(), filter));
                        ConvertBlockModelPropertyToOverridable<BlockListModel, OverridableBlockListModel>(
                            publishedValueFallback,
                            Constants.PropertyEditors.Aliases.BlockList,
                            overridableItem,
                            property,
                            blockList => new OverridableBlockListModel(publishedValueFallback, publishedElementFactory, filterStoreAccessor, (IEnumerable<BlockListItem>)blockList, filter),
                            () => new OverridableBlockListModel(publishedValueFallback, publishedElementFactory, filterStoreAccessor, Array.Empty<BlockListItem>(), filter));
                    }
                    Items.Add(overridableItem);
                }
            }

            if (filter is not null)
            {
                BaseFilter = filter;
                CopyFilterToDescendantBlockLists(Items, BaseFilter);
                CopyFilterToAreas(Items.SelectMany(item => item.Areas), BaseFilter);
            }
        }

        /// <summary>
        /// Property value formatters which may be applied when a property is overridden with a new value.
        /// </summary>
        /// <remarks>
        /// This should remain internal and is intended to be set by <see cref="OverridableBlockGridPropertyValueConverter"/> to pass down to each <see cref="OverridablePublishedElement"/>,
        /// because the property value converter is the nearest place that can inject the property value formatters registered with the dependency injection container.
        /// </remarks>
        internal IEnumerable<IPropertyValueFormatter>? PropertyValueFormatters
        {
            get
            {
                return _propertyValueFormatters;
            }
            set
            {
                _propertyValueFormatters = value;

                foreach (var item in Items)
                {
                    if (item.Content is OverridablePublishedElement content) { content.PropertyValueFormatters = PropertyValueFormatters; }
                    if (item.Settings is OverridablePublishedElement settings) { settings.PropertyValueFormatters = PropertyValueFormatters; }

                    if (item is OverridableBlockGridItem gridItem)
                    {
                        foreach (var area in gridItem.Areas)
                        {
                            foreach (var areaItem in area)
                            {
                                if (areaItem.Content is OverridablePublishedElement areaItemContent) { areaItemContent.PropertyValueFormatters = PropertyValueFormatters; }
                                if (areaItem.Settings is OverridablePublishedElement areaItemSettings) { areaItemSettings.PropertyValueFormatters = PropertyValueFormatters; }
                            }
                        }
                    }
                }
            }
        }


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
                CopyFilterToAreas(Items.SelectMany(item => item.Areas), BaseFilter);
            }
        }

        private void CopyFilterToAreas(IEnumerable<OverridableBlockGridArea> areas, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> filter)
        {
            foreach (var area in areas)
            {
                area.Filter = filter;
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

        /// <summary>
        /// The number of columns in the grid.
        /// </summary>
        public int? GridColumns { get; set; }

        /// <summary>
        /// Convert to a <see cref="BlockGridModel" />
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator BlockGridModel(OverridableBlockGridModel model)
        {
            var blockGrid = model.FilteredBlocks().ToList<BlockGridItem>();
            return new BlockGridModel(blockGrid, model.GridColumns);
        }
    }
}
