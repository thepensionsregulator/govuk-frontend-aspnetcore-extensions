using GovUk.Frontend.Umbraco.Models;
using System;
using System.Collections.Generic;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.BlockLists;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;

namespace GovUk.Frontend.Umbraco.BlockLists
{
    public static class OverridableBlockListItemExtensions
    {
        /// <summary>
        /// Replaces the select options configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Select component.</param>
        /// <param name="items">The select options to display.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSelectOptions(this IOverridablePublishedElement blockContent, IEnumerable<SelectOption> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            if (blockContent.ContentType?.Alias != ElementTypeAliases.Select)
            {
                throw new ArgumentException($"{nameof(OverrideSelectOptions)} may only be called on a block of element type {ElementTypeAliases.Select}. Element type was {blockContent.ContentType?.Alias}.", nameof(blockContent));
            }

            if (publishedSnapshotAccessor is null)
            {
                throw new ArgumentNullException(nameof(publishedSnapshotAccessor));
            }

            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var item in items)
            {
                var fields = new Dictionary<string, object?>()
                {
                    {"label", item.Text },
                    {"value", item.Value },
                };

                var blockListItem = new BlockListItem(
                    Udi.Create("element", Guid.NewGuid()),
                    new PublishedElement(PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, PublishedItemType.Content, ElementTypeAliases.SelectOption)!, Guid.NewGuid(), fields, false),
#nullable disable
                    null,
                    null
#nullable enable
                );

                blockListItems.Add(new OverridableBlockListItem(blockListItem));
            }

            blockContent.OverrideValue(PropertyAliases.SelectOptions, new OverridableBlockListModel(blockListItems));
        }
    }
}
