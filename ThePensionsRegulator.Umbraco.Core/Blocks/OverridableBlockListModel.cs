using System.ComponentModel;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    /// <summary>
    /// An adapter for a <see cref="BlockListModel" /> which supports filtering out blocks and overriding property values
    /// </summary>
    [TypeConverter(typeof(OverridableBlockListTypeConverter))]
    public class OverridableBlockListModel : OverridableBlockModel<OverridableBlockListItem>
    {
        private IEnumerable<IPropertyValueFormatter>? _propertyValueFormatters;

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/> with no items.
        /// </summary>
        [Obsolete($"Use a constructor which accepts an {nameof(IPublishedValueFallback)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockListModel() : this(Array.Empty<BlockListItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/> with no items.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        [Obsolete($"Use a constructor which accepts an {nameof(IOverridablePublishedElementFactory)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockListModel(IPublishedValueFallback publishedValueFallback) : this(publishedValueFallback, Array.Empty<BlockListItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/> with no items.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        /// <param name="publishedElementFactory">Factory used to create elements using a request-scoped value store. Request scope is required to support overriding property values.</param>
        /// <param name="filterStoreAccessor">Accessor for the current request-scoped store for block filters.</param>
        public OverridableBlockListModel(IPublishedValueFallback publishedValueFallback, IOverridablePublishedElementFactory publishedElementFactory, IOverridableBlockModelFilterStoreAccessor filterStoreAccessor)
            : this(publishedValueFallback, publishedElementFactory, filterStoreAccessor, Array.Empty<BlockListItem>())
        {
        }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/>.
        /// </summary>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory used to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        [Obsolete($"Use a constructor which accepts an {nameof(IOverridablePublishedElementFactory)} to avoid depending on Umbraco's StaticServiceProvider.")]
        public OverridableBlockListModel(
            IEnumerable<BlockListItem> blockListItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(null, blockListItems, filter, publishedElementFactory is not null ? new DelegatingOverridablePublishedElementFactory(publishedElementFactory) : null, null);


        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/>.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory used to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        [Obsolete($"Use a constructor which accepts an {nameof(IOverridablePublishedElementFactory)} to avoid depending on Umbraco's StaticServiceProvider.")]

        public OverridableBlockListModel(
            IPublishedValueFallback publishedValueFallback,
            IEnumerable<BlockListItem> blockListItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(publishedValueFallback, blockListItems, filter, publishedElementFactory is not null ? new DelegatingOverridablePublishedElementFactory(publishedElementFactory) : null, null);

        /// <summary>
        /// Creates an <see cref="OverridableBlockListModel" /> whose content and settings elements are created using the supplied <see cref="IOverridablePublishedElementFactory">.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        /// <param name="publishedElementFactory">Factory used to create elements using a request-scoped value store. Request scope is required to support overriding property values.</param>
        /// <param name="filterStoreAccessor">Accessor for the current request-scoped store for block filters.</param>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        public OverridableBlockListModel(
            IPublishedValueFallback publishedValueFallback,
            IOverridablePublishedElementFactory publishedElementFactory,
            IOverridableBlockModelFilterStoreAccessor filterStoreAccessor,
            IEnumerable<BlockListItem> blockListItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null)
            => Initialise(
                publishedValueFallback,
                blockListItems,
                filter,
                publishedElementFactory,
                filterStoreAccessor);

        private void Initialise(IPublishedValueFallback? publishedValueFallback, IEnumerable<BlockListItem> blockListItems, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter, IOverridablePublishedElementFactory? publishedElementFactory, IOverridableBlockModelFilterStoreAccessor? filterStoreAccessor)
        {
            if (blockListItems is null)
            {
                throw new ArgumentNullException(nameof(blockListItems));
            }

            filterStoreAccessor ??= ResolveFromStaticServiceProvider<IOverridableBlockModelFilterStoreAccessor>();
            InitialiseFilterStoreAccessor(filterStoreAccessor);

            // Take the IEnumerable<BlockListItem> (which is probably a BlockListModel) and convert each item to an OverridableBlockListItem,
            // and each nested block list to an OverridableBlockListModel populated with OverridableBlockListItems.
            if (blockListItems.Any())
            {
                publishedValueFallback ??= ResolveFromStaticServiceProvider<IPublishedValueFallback>();
                publishedElementFactory ??= ResolveFromStaticServiceProvider<IOverridablePublishedElementFactory>();

                foreach (var item in blockListItems)
                {
                    var overridableItem = item as OverridableBlockListItem ?? new OverridableBlockListItem(item, publishedElementFactory);
                    foreach (var property in overridableItem.Content.Properties)
                    {
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
            }
        }

        /// <summary>
        /// Property value formatters which may be applied when a property is overridden with a new value.
        /// </summary>
        /// <remarks>
        /// This should remain internal and is intended to be set by <see cref="OverridableBlockListPropertyValueConverter"/> to pass down to each <see cref="OverridablePublishedElement"/>,
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
            }
        }

        /// <summary>
        /// Gets the block list with items not matching <see cref="Filter"/> removed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OverridableBlockListItem> FilteredBlocks()
        {
            var filtered = Items.Where(Filter);
            return filtered.Select(block => block as OverridableBlockListItem).OfType<OverridableBlockListItem>();
        }

        /// <summary>
        /// Convert to a <see cref="BlockListModel" />
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator BlockListModel(OverridableBlockListModel model)
        {
            var blockList = model.FilteredBlocks().ToList<BlockListItem>();
            return new BlockListModel(blockList);
        }
    }
}
