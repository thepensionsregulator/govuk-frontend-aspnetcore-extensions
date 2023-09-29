using GovUk.Frontend.Umbraco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public static class IOverridablePublishedElementExtensions
    {
        /// <summary>
        /// Replaces the checkboxes configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Checkboxes component.</param>
        /// <param name="items">The checkboxes.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideCheckboxes(this IOverridablePublishedElement blockContent,
            IEnumerable<CheckboxItemBase> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            blockContent.OverrideCheckboxes(items, publishedSnapshotAccessor, null);
        }

        /// <summary>
        /// Replaces the checkboxes configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Checkboxes component.</param>
        /// <param name="items">The checkboxes.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideCheckboxes(this IOverridablePublishedElement blockContent,
            IEnumerable<CheckboxItemBase> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            Func<OverridableBlockListItem, bool>? filter)
        {
            GuardOverrideChildBlocks(nameof(OverrideCheckboxes), new List<string> { ElementTypeAliases.Checkboxes }, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var item in items)
            {
                if (item is Checkbox checkbox)
                {
                    var contentFields = new Dictionary<string, object?>()
                    {
                        { PropertyAliases.CheckboxLabel, checkbox.Label },
                        { PropertyAliases.CheckboxValue, checkbox.Value },
                        { PropertyAliases.Hint, checkbox.Hint },
                        { PropertyAliases.CheckboxConditionalBlocks, checkbox.ConditionalBlocks }
                    };

                    var settingsFields = new Dictionary<string, object?>()
                    {
                        { PropertyAliases.CssClasses, checkbox.CssClasses }
                    };

                    blockListItems.Add(CreateBlockListItem(ElementTypeAliases.Checkbox, contentFields, ElementTypeAliases.CheckboxSettings, settingsFields, publishedSnapshotAccessor));
                }
                else if (item is CheckboxesDivider divider)
                {
                    var contentFields = new Dictionary<string, object?>()
                    {
                        { PropertyAliases.CheckboxesDividerText, divider.Text }
                    };

                    blockListItems.Add(CreateBlockListItem(ElementTypeAliases.CheckboxesDivider, contentFields, null, null, publishedSnapshotAccessor));
                }
            }

            blockContent.OverrideValue(PropertyAliases.Checkboxes, new OverridableBlockListModel(blockListItems, filter));
        }

        /// <summary>
        /// Replaces the radio buttons configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Radios component.</param>
        /// <param name="items">The radio buttons.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideRadioButtons(this IOverridablePublishedElement blockContent, IEnumerable<RadioItemBase> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            blockContent.OverrideRadioButtons(items, publishedSnapshotAccessor, null);
        }

        /// <summary>
        /// Replaces the radio buttons configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Radios component.</param>
        /// <param name="items">The radio buttons.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideRadioButtons(this IOverridablePublishedElement blockContent,
            IEnumerable<RadioItemBase> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            Func<OverridableBlockListItem, bool>? filter)
        {
            GuardOverrideChildBlocks(nameof(OverrideRadioButtons), new List<string> { ElementTypeAliases.Radios }, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var item in items)
            {
                if (item is RadioButton radioButton)
                {
                    var contentFields = new Dictionary<string, object?>()
                    {
                        { PropertyAliases.RadioButtonLabel, radioButton.Label },
                        { PropertyAliases.RadioButtonValue, radioButton.Value },
                        { PropertyAliases.Hint, radioButton.Hint },
                        { PropertyAliases.RadioConditionalBlocks, radioButton.ConditionalBlocks }
                    };

                    var settingsFields = new Dictionary<string, object?>()
                    {
                        { PropertyAliases.CssClasses, radioButton.CssClasses }
                    };

                    blockListItems.Add(CreateBlockListItem(ElementTypeAliases.Radio, contentFields, ElementTypeAliases.RadioSettings, settingsFields, publishedSnapshotAccessor));
                }
                else if (item is RadiosDivider divider)
                {
                    var contentFields = new Dictionary<string, object?>()
                    {
                        { PropertyAliases.RadiosDividerText, divider.Text }
                    };

                    blockListItems.Add(CreateBlockListItem(ElementTypeAliases.RadiosDivider, contentFields, null, null, publishedSnapshotAccessor));
                }
            }

            blockContent.OverrideValue(PropertyAliases.RadioButtons, new OverridableBlockListModel(blockListItems, filter));
        }

        /// <summary>
        /// Replaces the select options configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Select component.</param>
        /// <param name="items">The select options.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSelectOptions(this IOverridablePublishedElement blockContent, IEnumerable<SelectOption> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            blockContent.OverrideSelectOptions(items, publishedSnapshotAccessor, null);
        }

        /// <summary>
        /// Replaces the select options configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Select component.</param>
        /// <param name="items">The select options.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSelectOptions(this IOverridablePublishedElement blockContent,
            IEnumerable<SelectOption> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            Func<OverridableBlockListItem, bool>? filter)
        {
            GuardOverrideChildBlocks(nameof(OverrideSelectOptions), new List<string> { ElementTypeAliases.Select }, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var item in items)
            {
                var contentFields = new Dictionary<string, object?>()
                {
                    { PropertyAliases.SelectOptionLabel, item.Label },
                    { PropertyAliases.SelectOptionValue, item.Value },
                };

                blockListItems.Add(CreateBlockListItem(ElementTypeAliases.SelectOption, contentFields, null, null, publishedSnapshotAccessor));
            }

            blockContent.OverrideValue(PropertyAliases.SelectOptions, new OverridableBlockListModel(blockListItems, filter));
        }

        /// <summary>
        /// Replaces the summary card actions configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Summary card component.</param>
        /// <param name="items">The summary card actions.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSummaryCardActions(this IOverridablePublishedElement blockContent, IEnumerable<SummaryListAction> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            blockContent.OverrideSummaryCardActions(items, publishedSnapshotAccessor, null);
        }

        /// <summary>
        /// Replaces the summary card actions configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Summary card component.</param>
        /// <param name="items">The summary card actions.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSummaryCardActions(this IOverridablePublishedElement blockContent,
            IEnumerable<SummaryListAction> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            Func<OverridableBlockListItem, bool>? filter)
        {
            GuardOverrideChildBlocks(nameof(OverrideSummaryListItems), new List<string> { ElementTypeAliases.SummaryCard }, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

            blockContent.OverrideValue(PropertyAliases.SummaryCardActions, CreateSummaryListActionBlocks(items, publishedSnapshotAccessor, filter));
        }

        /// <summary>
        /// Replaces the summary list items configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Summary list component.</param>
        /// <param name="items">The summary list items.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSummaryListItems(this IOverridablePublishedElement blockContent, IEnumerable<SummaryListItem> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            blockContent.OverrideSummaryListItems(items, publishedSnapshotAccessor, null);
        }

        /// <summary>
        /// Replaces the summary list items configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Summary list component.</param>
        /// <param name="items">The summary list items.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <param name="filter">The filter which will be applied to blocks when retrieved using <see cref="FilteredBlocks"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSummaryListItems(this IOverridablePublishedElement blockContent,
            IEnumerable<SummaryListItem> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            Func<OverridableBlockListItem, bool>? filter)
        {
            GuardOverrideChildBlocks(nameof(OverrideSummaryListItems), new List<string> { ElementTypeAliases.SummaryList, ElementTypeAliases.SummaryCard }, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var item in items)
            {
                var contentFields = new Dictionary<string, object?>()
                {
                    { PropertyAliases.SummaryListItemKey, item.Key },
                    { PropertyAliases.SummaryListItemValue, item.Value },
                    { PropertyAliases.SummaryListItemActions, CreateSummaryListActionBlocks(item.Actions, publishedSnapshotAccessor, filter) }
                };

                var settingsFields = new Dictionary<string, object?>()
                {
                    { PropertyAliases.CssClasses, item.CssClasses }
                };

                blockListItems.Add(CreateBlockListItem(ElementTypeAliases.SummaryListItem, contentFields, ElementTypeAliases.SummaryListItemSettings, settingsFields, publishedSnapshotAccessor));
            }

            var listItemPropertyAlias = blockContent.ContentType!.Alias == ElementTypeAliases.SummaryList ? PropertyAliases.SummaryListItems : PropertyAliases.SummaryCardListItems;
            blockContent.OverrideValue(listItemPropertyAlias, new OverridableBlockListModel(blockListItems, filter));
        }

        private static OverridableBlockListModel CreateSummaryListActionBlocks(IEnumerable<SummaryListAction> items,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            Func<OverridableBlockListItem, bool>? filter)
        {
            var blockListItems = new List<OverridableBlockListItem>();
            foreach (var item in items)
            {
                var actionFields = new Dictionary<string, object?>()
                {
                    { PropertyAliases.SummaryListActionLink, item.Link },
                    { PropertyAliases.SummaryListActionLinkText, item.LinkText }
                };

                blockListItems.Add(CreateBlockListItem(ElementTypeAliases.SummaryListAction, actionFields, null, null, publishedSnapshotAccessor));
            }

            return new OverridableBlockListModel(blockListItems, filter);
        }

        private static OverridableBlockListItem CreateBlockListItem(
            string contentTypeAlias, Dictionary<string, object?> contentProperties,
            string? settingsTypeAlias, Dictionary<string, object?>? settingsProperties,
            IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            // Create a new block list item with the supplied properties but null values. It's not appropriate to put the values into the block list item properties because
            // the supplied values will be of the type expected after property value conversion, but the properties should contain raw values before property value conversion.
            var hasSettings = !string.IsNullOrEmpty(settingsTypeAlias) && settingsProperties is not null;
            Dictionary<string, object?> originalContent = new(contentProperties.Select(x => new KeyValuePair<string, object?>(x.Key, null)));
            Dictionary<string, object?>? originalSettings = hasSettings ? new(settingsProperties!.Select(x => new KeyValuePair<string, object?>(x.Key, null))) : null;

            var content = new PublishedElement(PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, PublishedItemType.Content, contentTypeAlias)!, Guid.NewGuid(), originalContent, false);
            var settings = hasSettings ? new PublishedElement(PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, PublishedItemType.Content, settingsTypeAlias!)!, Guid.NewGuid(), originalSettings!, false) : null;

            var blockListItem = new BlockListItem(
                                Udi.Create("element", Guid.NewGuid()),
                                content,
#nullable disable
                                hasSettings ? Udi.Create("element", Guid.NewGuid()) : null,
                                settings
#nullable enable
                            );

            // Override the null values with the supplied values.
            var overridable = new OverridableBlockListItem(blockListItem);
            foreach (var field in contentProperties.Keys.Where(x => contentProperties[x] is not null))
            {
                overridable.Content.OverrideValue(field, contentProperties[field]!);
            }
            if (hasSettings)
            {
                foreach (var field in settingsProperties!.Keys.Where(x => settingsProperties[x] is not null))
                {
                    overridable.Settings.OverrideValue(field, settingsProperties[field]!);
                }
            }

            return overridable;
        }

        private static void GuardOverrideChildBlocks(string methodName, List<string> expectedAliases, string? actualAlias, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            if (actualAlias is null || !expectedAliases.Contains(actualAlias))
            {
                throw new ArgumentException($"{methodName} may only be called on a block of element types {string.Join(",", expectedAliases)}. Element type was {actualAlias}.");
            }

            if (publishedSnapshotAccessor is null)
            {
                throw new ArgumentNullException(nameof(publishedSnapshotAccessor));
            }
        }
    }
}