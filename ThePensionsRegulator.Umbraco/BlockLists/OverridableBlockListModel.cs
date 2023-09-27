using System.Collections;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.BlockLists
{
	/// <summary>
	/// An adapter for a <see cref="BlockListModel" /> which supports filtering out blocks and overriding property values
	/// </summary>
	[TypeConverter(typeof(OverridableBlockListTypeConverter))]
	public class OverridableBlockListModel : IEnumerable<OverridableBlockListItem>
	{
		private readonly List<OverridableBlockListItem> _items = new();
		private static Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>> DefaultFilter = (x => x);

		/// <summary>
		/// Creates a new <see cref="OverridableBlockListModel"/> with no items.
		/// </summary>
		public OverridableBlockListModel() : this(Array.Empty<BlockListItem>()) { }

		private IEnumerable<IPropertyValueFormatter>? _propertyValueFormatters;

		/// <summary>
		/// Creates a new <see cref="OverridableBlockListModel"/>
		/// </summary>
		/// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
		/// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
		/// <param name="publishedElementFactory">Factory method to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
		public OverridableBlockListModel(IEnumerable<BlockListItem> blockListItems, Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>>? filter = null, Func<IPublishedElement?, IOverridablePublishedElement?>? publishedElementFactory = null)
		{
			if (blockListItems is null)
			{
				throw new ArgumentNullException(nameof(blockListItems));
			}

			_filter = filter ?? DefaultFilter;
			var factory = publishedElementFactory ?? OverridableBlockListItem.DefaultPublishedElementFactory;

			// Take the IEnumerable<BlockListItem> (which is probably a BlockListModel) and convert each item to an OverridableBlockListItem,
			// and each nested block list to an OverridableBlockListModel populated with OverridableBlockListItems.
			foreach (var item in blockListItems)
			{
				var overridableItem = item as OverridableBlockListItem ?? new OverridableBlockListItem(item, factory);
				foreach (var prop in overridableItem.Content.Properties)
				{
					if (prop.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList)
					{
						var overriddenNestedBlockList = overridableItem.Content.Value<OverridableBlockListModel>(prop.Alias);
						if (overriddenNestedBlockList == null)
						{
							var nestedBlockList = overridableItem.Content.Value<BlockListModel>(prop.Alias);
							if (nestedBlockList != null)
							{
								overriddenNestedBlockList = new OverridableBlockListModel(nestedBlockList, _filter, factory);
							}
							else
							{
								overriddenNestedBlockList = new OverridableBlockListModel(Array.Empty<BlockListItem>(), _filter, factory);
							}
						}
						overridableItem.Content.OverrideValue(prop.Alias, overriddenNestedBlockList);
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

				foreach (var item in _items)
				{
					if (item.Content is OverridablePublishedElement content) { content.PropertyValueFormatters = PropertyValueFormatters; }
					if (item.Settings is OverridablePublishedElement settings) { settings.PropertyValueFormatters = PropertyValueFormatters; }
				}
			}
		}

		private Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>> _filter = DefaultFilter;

		/// <summary>
		/// The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.
		/// </summary>
		public Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>> Filter
		{
			get { return _filter; }
			set
			{
				_filter = value;

				CopyFilterToDecendantBlockLists(_items, _filter);
			}
		}

		private void CopyFilterToDecendantBlockLists(IEnumerable<OverridableBlockListItem> blockListItems, Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>> filter)
		{
			foreach (var blockListItem in blockListItems)
			{
				var models = blockListItem.Content.Properties
					.Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && x.HasValue())
					.Select(x => blockListItem.Content.Value<OverridableBlockListModel>(x.Alias))
					.OfType<OverridableBlockListModel>();
				foreach (var model in models)
				{
					model.Filter = filter;
					CopyFilterToDecendantBlockLists(model, filter);
				}
			}
		}

		/// <summary>
		/// Gets the block list with items not matching <see cref="Filter"/> removed.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<OverridableBlockListItem> FilteredBlocks()
		{
			return Filter(_items);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the unfiltered list of blocks
		/// </summary>
		public IEnumerator<OverridableBlockListItem> GetEnumerator()
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
		/// Gets or sets whether a default grid row and column should be rendered for this block list.
		/// </summary>
		public bool RenderGrid { get; set; } = true;

		/// <summary>
		/// Convert to a <see cref="BlockListModel" />
		/// </summary>
		/// <param name="model"></param>
		public static explicit operator BlockListModel(OverridableBlockListModel model)
		{
			var blockList = model.FilteredBlocks().ToList<BlockListItem>();
			return new BlockListModel(blockList);
		}

		/// <summary>
		/// Gets or sets a block from the unfiltered list of blocks
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public OverridableBlockListItem this[int index]
		{
			get => _items[index];
			set => _items[index] = value;
		}
	}
}
