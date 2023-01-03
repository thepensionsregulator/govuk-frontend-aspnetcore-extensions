using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// An adapter for a <see cref="BlockListModel" /> which supports filtering out blocks and overriding property values
    /// </summary>
    public class OverridableBlockListModel : IEnumerable<OverridableBlockListItem>
    {
        private readonly List<OverridableBlockListItem> _items = new();

        /// <summary>
        /// Creates a new <see cref="OverridableBlockListModel"/>
        /// </summary>
        /// <param name="blockListItems">A block list (typically a <see cref="BlockListModel"/>).</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <param name="publishedElementFactory">Factory method to create an <see cref="IPublishedElement"/> that supports overriding property values.</param>
        public OverridableBlockListModel(IEnumerable<BlockListItem> blockListItems, Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>>? filter = null, Func<IPublishedElement, IOverridablePublishedElement>? publishedElementFactory = null)
        {
            Filter = filter ?? (x => x);
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
                                overriddenNestedBlockList = new OverridableBlockListModel(nestedBlockList, Filter, factory);
                            }
                            else
                            {
                                overriddenNestedBlockList = new OverridableBlockListModel(Array.Empty<BlockListItem>(), Filter, factory);
                            }
                        }
                        overridableItem.Content.OverrideValue(prop.Alias, overriddenNestedBlockList);
                    }
                }
                _items.Add(overridableItem);
            }

        }

        /// <summary>
        /// The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.
        /// </summary>
        public Func<IEnumerable<OverridableBlockListItem>, IEnumerable<OverridableBlockListItem>> Filter { get; private set; }

        /// <summary>
        /// Gets the block list with items not matching <see cref="Filter"/> removed.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OverridableBlockListItem> FilteredBlocks()
        {
            return Filter(_items);
        }

        /// <inheritdoc />
        public IEnumerator<OverridableBlockListItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <inheritdoc />
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
    }
}
