using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DependencyInjection;
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
        [Obsolete("Use a constructor which specifies an IPublishedValueFallback.")]
        public OverridableBlockListModel() : this(Array.Empty<BlockListItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/> with no items.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        [Obsolete("Use a constructor which specifies an IHttpContextAccessor. Pass null for non-HTTP usage.")]
        public OverridableBlockListModel(IPublishedValueFallback publishedValueFallback) : this(publishedValueFallback, null, Array.Empty<BlockListItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/> with no items.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        /// <param name="httpContextAccessor">The accessor used to find the current request-scoped override store. Must not be <c>null</c> during an HTTP request; pass <c>null</c> for non-HTTP usage.</param>
        public OverridableBlockListModel(IPublishedValueFallback publishedValueFallback, IHttpContextAccessor? httpContextAccessor)
            : this(publishedValueFallback, httpContextAccessor, Array.Empty<BlockListItem>()) { }

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/>.
        /// </summary>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory method to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        [Obsolete("Use a constructor which specifies an IHttpContextAccessor. Pass null for non-HTTP usage.")]
        public OverridableBlockListModel(
            IEnumerable<BlockListItem> blockListItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(null, blockListItems, filter, publishedElementFactory);


        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/>.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory method to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        [Obsolete("Use the overload accepting an IHttpContextAccessor. Pass null for non-HTTP usage.")]
        public OverridableBlockListModel(
            IPublishedValueFallback publishedValueFallback,
            IEnumerable<BlockListItem> blockListItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(publishedValueFallback, blockListItems, filter, publishedElementFactory);

        /// <summary>
        /// Creates a block list model whose wrappers resolve override values from the current HTTP request. The <paramref name="httpContextAccessor"/> must not be <c>null</c> when this method is called during an HTTP request. Pass <c>null</c> only for non-HTTP usage.
        /// </summary>
        /// <param name="publishedValueFallback">An <see cref="IPublishedValueFallback"/> to use when retrieving property values for the block list items.</param>
        /// <param name="httpContextAccessor">Must not be <c>null</c> during an HTTP request, or values will be cached incorrectly. Pass <c>null</c> only for non-HTTP usage.</param>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        public OverridableBlockListModel(
            IPublishedValueFallback publishedValueFallback,
            IHttpContextAccessor? httpContextAccessor,
            IEnumerable<BlockListItem> blockListItems,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter = null,
            Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
            => Initialise(
                publishedValueFallback,
                blockListItems,
                filter,
                publishedElementFactory ?? (publishedElement => publishedElement is null ? null : new OverridablePublishedElement(publishedElement, httpContextAccessor)));

        private void Initialise(IPublishedValueFallback? publishedValueFallback, IEnumerable<BlockListItem> blockListItems, Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>? filter, Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory)
        {
            if (blockListItems is null)
            {
                throw new ArgumentNullException(nameof(blockListItems));
            }

            BaseFilter = filter ?? DefaultFilter;
            var factory = publishedElementFactory ?? OverridableBlockListItem.DefaultPublishedElementFactory;

            // Take the IEnumerable<BlockListItem> (which is probably a BlockListModel) and convert each item to an OverridableBlockListItem,
            // and each nested block list to an OverridableBlockListModel populated with OverridableBlockListItems.
            if (blockListItems.Any())
            {
                publishedValueFallback = publishedValueFallback ?? StaticServiceProvider.Instance.GetRequiredService<IPublishedValueFallback>();

                foreach (var item in blockListItems)
                {
                    var overridableItem = item as OverridableBlockListItem ?? new OverridableBlockListItem(item, factory);
                    foreach (var property in overridableItem.Content.Properties)
                    {
                        ConvertBlockModelPropertyToOverridable<BlockListModel, OverridableBlockListModel>(
                            publishedValueFallback,
                            Constants.PropertyEditors.Aliases.BlockList,
                            overridableItem,
                            property,
                            blockList => new OverridableBlockListModel(publishedValueFallback, null, blockList, BaseFilter, factory),
                            () => new OverridableBlockListModel(publishedValueFallback, null, Array.Empty<BlockListItem>(), BaseFilter, factory));
                    }
                    Items.Add(overridableItem);
                }
            }

            CopyFilterToDescendantBlockLists(Items, BaseFilter);
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
