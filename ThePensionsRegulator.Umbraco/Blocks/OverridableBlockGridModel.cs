using System.Collections;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    /// <summary>
    /// An adapter for a <see cref="BlockGridModel" /> which supports filtering out blocks and overriding property values
    /// </summary>
    [TypeConverter(typeof(OverridableBlockGridTypeConverter))]
    public class OverridableBlockGridModel : IEnumerable<OverridableBlockGridItem>
    {
        private readonly List<OverridableBlockGridItem> _items = new();
        private static Func<OverridableBlockGridItem, bool> DefaultFilter = x => true;

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/> with no items.
        /// </summary>
        public OverridableBlockGridModel() : this(Array.Empty<BlockGridItem>()) { }

        private IEnumerable<IPropertyValueFormatter>? _propertyValueFormatters;

        /// <summary>
        /// Creates a new <see cref="OverridableBlockGridModel"/>
        /// </summary>
        /// <param name="blockGridItems">A block grid (typically a <see cref="BlockGridModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory method to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        public OverridableBlockGridModel(IEnumerable<BlockGridItem> blockGridItems, Func<OverridableBlockGridItem, bool>? filter = null, Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
        {
            if (blockGridItems is null)
            {
                throw new ArgumentNullException(nameof(blockGridItems));
            }

            _filter = filter ?? DefaultFilter;
            var factory = publishedElementFactory ?? OverridableBlockListItem.DefaultPublishedElementFactory;

            // Take the IEnumerable<BlockListItem> (which is probably a BlockListModel) and convert each item to an OverridableBlockListItem,
            // and each nested block list to an OverridableBlockListModel populated with OverridableBlockListItems.
            foreach (var item in blockGridItems)
            {
                var overridableItem = item as OverridableBlockGridItem ?? new OverridableBlockGridItem(item, factory);
                foreach (var prop in overridableItem.Content.Properties)
                {
                    if (prop.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockGrid)
                    {
                        var overriddenNestedBlockGrid = overridableItem.Content.Value<OverridableBlockGridModel>(prop.Alias);
                        if (overriddenNestedBlockGrid == null)
                        {
                            var nestedBlockGrid = overridableItem.Content.Value<BlockGridModel>(prop.Alias);
                            if (nestedBlockGrid != null)
                            {
                                overriddenNestedBlockGrid = new OverridableBlockGridModel(nestedBlockGrid, _filter, factory);
                            }
                            else
                            {
                                overriddenNestedBlockGrid = new OverridableBlockGridModel(Array.Empty<BlockGridItem>(), _filter, factory);
                            }
                        }
                        overridableItem.Content.OverrideValue(prop.Alias, overriddenNestedBlockGrid);
                    }
                }
                _items.Add(overridableItem);
            }

            CopyFilterToDecendantBlockLists(_items, _filter);
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

                foreach (var item in _items)
                {
                    if (item.Content is OverridablePublishedElement content) { content.PropertyValueFormatters = PropertyValueFormatters; }
                    if (item.Settings is OverridablePublishedElement settings) { settings.PropertyValueFormatters = PropertyValueFormatters; }
                }
            }
        }

        private Func<OverridableBlockGridItem, bool> _filter = DefaultFilter;

        /// <summary>
        /// The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.
        /// </summary>
        public Func<OverridableBlockGridItem, bool> Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;

                CopyFilterToDecendantBlockLists(_items, _filter);
            }
        }

        private void CopyFilterToDecendantBlockLists(IEnumerable<OverridableBlockGridItem> blockGridItems, Func<OverridableBlockGridItem, bool> filter)
        {
            foreach (var blockGridItem in blockGridItems)
            {
                var models = blockGridItem.Content.Properties
                    .Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockGrid && x.HasValue())
                    .Select(x => blockGridItem.Content.Value<OverridableBlockGridModel>(x.Alias))
                    .OfType<OverridableBlockGridModel>();
                foreach (var model in models)
                {
                    model.Filter = filter;
                    CopyFilterToDecendantBlockLists(model, filter);
                }
            }
        }

        /// <summary>
        /// Gets the block grid with items not matching <see cref="Filter"/> removed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OverridableBlockGridItem> FilteredBlocks()
        {
            return _items.Where(Filter);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the unfiltered list of blocks
        /// </summary>
        public IEnumerator<OverridableBlockGridItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the unfiltered list of blocks
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
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

        /// <summary>
        /// Gets or sets a block from the unfiltered list of blocks.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public OverridableBlockGridItem this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
    }
}
