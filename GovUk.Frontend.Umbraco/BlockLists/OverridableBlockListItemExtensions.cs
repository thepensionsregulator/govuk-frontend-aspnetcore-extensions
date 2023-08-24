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
        /// Replaces the checkboxes configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Checkboxes component.</param>
        /// <param name="items">The checkboxes.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideCheckboxes(this IOverridablePublishedElement blockContent, IEnumerable<CheckboxItemBase> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            GuardOverrideChildBlocks(nameof(OverrideCheckboxes), ElementTypeAliases.Checkboxes, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

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

            blockContent.OverrideValue(PropertyAliases.Checkboxes, new OverridableBlockListModel(blockListItems));
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
            GuardOverrideChildBlocks(nameof(OverrideRadioButtons), ElementTypeAliases.Radios, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

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

            blockContent.OverrideValue(PropertyAliases.RadioButtons, new OverridableBlockListModel(blockListItems));
        }

        /// <summary>
        /// Replaces the select options configured in Umbraco with those supplied as an argument.
        /// </summary>
        /// <param name="blockContent">The content of a block list item based on the GOV.UK Select component.</param>
        /// <param name="items">The select options to display.</param>
        /// <param name="publishedSnapshotAccessor">Accessor for a published snapshot, which is a point-in-time capture of the current state of everything that is "published".</param>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static void OverrideSelectOptions(this IOverridablePublishedElement blockContent, IEnumerable<SelectOption> items, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            GuardOverrideChildBlocks(nameof(OverrideSelectOptions), ElementTypeAliases.Select, blockContent.ContentType?.Alias, publishedSnapshotAccessor);

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

            blockContent.OverrideValue(PropertyAliases.SelectOptions, new OverridableBlockListModel(blockListItems));
        }

        private static OverridableBlockListItem CreateBlockListItem(
            string contentTypeAlias, Dictionary<string, object?> contentFields,
            string? settingsTypeAlias, Dictionary<string, object?>? settingsFields,
            IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            var content = new PublishedElement(PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, PublishedItemType.Content, contentTypeAlias)!, Guid.NewGuid(), contentFields, false);
            var hasSettings = !string.IsNullOrEmpty(settingsTypeAlias) && settingsFields is not null;
            var settings = hasSettings ? new PublishedElement(PublishedModelUtility.GetModelContentType(publishedSnapshotAccessor, PublishedItemType.Content, settingsTypeAlias!)!, Guid.NewGuid(), settingsFields!, false) : null;
            var blockListItem = new BlockListItem(
                                Udi.Create("element", Guid.NewGuid()),
                                content,
#nullable disable
                                hasSettings ? Udi.Create("element", Guid.NewGuid()) : null,
                                settings
#nullable enable
                            );

            return new OverridableBlockListItem(blockListItem);
        }

        private static void GuardOverrideChildBlocks(string methodName, string expectedAlias, string? actualAlias, IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            if (actualAlias != expectedAlias)
            {
                throw new ArgumentException($"{methodName} may only be called on a block of element type {expectedAlias}. Element type was {actualAlias}.");
            }

            if (publishedSnapshotAccessor is null)
            {
                throw new ArgumentNullException(nameof(publishedSnapshotAccessor));
            }
        }
    }
}
