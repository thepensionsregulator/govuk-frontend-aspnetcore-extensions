using GovUk.Frontend.Umbraco.BlockLists;
using GovUk.Frontend.Umbraco.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.BlockLists;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.BlockLists
{
    [TestFixture]
    public class IOverridablePublishedElementExtensionsTests
    {
        [TestCase(ElementTypeAliases.Checkboxes, false)]
        [TestCase(ElementTypeAliases.Radios, true)]
        public void OverrideCheckboxes_throws_ArgumentException_if_block_is_not_Checkboxes_component(string elementTypeAlias, bool exceptionExpected)
        {
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideCheckboxes(Array.Empty<Checkbox>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
            else
            {
                Assert.DoesNotThrow(() => content.Object.OverrideCheckboxes(Array.Empty<Checkbox>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
        }

        [Test]
        public void OverrideCheckboxes_replaces_checkboxes()
        {
            // Arrange
            var testContext = new UmbracoTestContext()
                .SetupContentType(ElementTypeAliases.Checkbox)
                .SetupContentType(ElementTypeAliases.CheckboxesDivider)
                .SetupContentType(ElementTypeAliases.CheckboxSettings);

            var originalItems = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Checkbox)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CheckboxValue, "1")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CheckboxLabel, "Item 1")
                    .Object),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Checkbox)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CheckboxValue, "2")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CheckboxLabel, "Item 2")
                    .Object
                    )
            });

            var content = new OverridablePublishedElement(
                UmbracoContentFactory.CreateContent<IPublishedElement>(ElementTypeAliases.Checkboxes)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.Checkboxes, originalItems)
                    .Object);

            var replacement = new CheckboxItemBase[]
            {
                new Checkbox("3", "Item 3")
                {
                    Hint = new HtmlEncodedString("<i>Hint 3</i>"),
                    ConditionalBlocks = UmbracoBlockListFactory.CreateOverridableBlockListModel(Array.Empty<BlockListItem>()),
                    CssClasses = "item-3"
                },
                new CheckboxesDivider { Text = "divider" },
                new Checkbox("4", "Item 4")
            };

            // Act
            content.OverrideCheckboxes(replacement, testContext.PublishedSnapshotAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(PropertyAliases.Checkboxes);

            Assert.That(options, Is.Not.Null);
            Assert.That(options!.Count(), Is.EqualTo(replacement.Count()));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.CheckboxValue), Is.EqualTo("3"));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.CheckboxLabel), Is.EqualTo("Item 3"));
            Assert.That(options![0].Content.Value<IHtmlEncodedString>(PropertyAliases.Hint)?.ToHtmlString(), Is.EqualTo("<i>Hint 3</i>"));
            Assert.That(options![0].Content.Value<OverridableBlockListModel>(PropertyAliases.CheckboxConditionalBlocks), Is.Not.Null);
            Assert.That(options![0].Settings.Value<string>(PropertyAliases.CssClasses), Is.EqualTo("item-3"));
            Assert.That(options[1].Content.Value<string>(PropertyAliases.CheckboxesDividerText), Is.EqualTo("divider"));
            Assert.That(options[2].Content.Value<string>(PropertyAliases.CheckboxValue), Is.EqualTo("4"));
            Assert.That(options[2].Content.Value<string>(PropertyAliases.CheckboxLabel), Is.EqualTo("Item 4"));
        }

        [TestCase(ElementTypeAliases.Checkboxes, true)]
        [TestCase(ElementTypeAliases.Radios, false)]
        public void OverrideRadioButtons_throws_ArgumentException_if_block_is_not_Radios_component(string elementTypeAlias, bool exceptionExpected)
        {
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideRadioButtons(Array.Empty<RadioButton>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
            else
            {
                Assert.DoesNotThrow(() => content.Object.OverrideRadioButtons(Array.Empty<RadioButton>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
        }

        [Test]
        public void OverrideRadioButtons_replaces_radio_buttons()
        {
            // Arrange
            var testContext = new UmbracoTestContext()
                .SetupContentType(ElementTypeAliases.Radio)
                .SetupContentType(ElementTypeAliases.RadiosDivider)
                .SetupContentType(ElementTypeAliases.RadioSettings);

            var originalItems = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Radio)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.RadioButtonValue, "1")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.RadioButtonLabel, "Item 1")
                    .Object),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Radio)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.RadioButtonValue, "2")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.RadioButtonLabel, "Item 2")
                    .Object
                    )
            });

            var content = new OverridablePublishedElement(
                UmbracoContentFactory.CreateContent<IPublishedElement>(ElementTypeAliases.Radios)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.RadioButtons, originalItems)
                    .Object);

            var replacement = new RadioItemBase[]
            {
                new RadioButton("3", "Item 3")
                {
                    Hint = new HtmlEncodedString("<i>Hint 3</i>"),
                    ConditionalBlocks = UmbracoBlockListFactory.CreateOverridableBlockListModel(Array.Empty<BlockListItem>()),
                    CssClasses = "item-3"
                },
                new RadiosDivider { Text = "divider" },
                new RadioButton("4", "Item 4")
            };

            // Act
            content.OverrideRadioButtons(replacement, testContext.PublishedSnapshotAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(PropertyAliases.RadioButtons);

            Assert.That(options, Is.Not.Null);
            Assert.That(options!.Count(), Is.EqualTo(replacement.Count()));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.RadioButtonValue), Is.EqualTo("3"));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.RadioButtonLabel), Is.EqualTo("Item 3"));
            Assert.That(options![0].Content.Value<IHtmlEncodedString>(PropertyAliases.Hint)?.ToHtmlString(), Is.EqualTo("<i>Hint 3</i>"));
            Assert.That(options![0].Content.Value<OverridableBlockListModel>(PropertyAliases.RadioConditionalBlocks), Is.Not.Null);
            Assert.That(options![0].Settings.Value<string>(PropertyAliases.CssClasses), Is.EqualTo("item-3"));
            Assert.That(options[1].Content.Value<string>(PropertyAliases.RadiosDividerText), Is.EqualTo("divider"));
            Assert.That(options[2].Content.Value<string>(PropertyAliases.RadioButtonValue), Is.EqualTo("4"));
            Assert.That(options[2].Content.Value<string>(PropertyAliases.RadioButtonLabel), Is.EqualTo("Item 4"));
        }

        [TestCase(ElementTypeAliases.Select, false)]
        [TestCase(ElementTypeAliases.Radios, true)]
        public void OverrideSelectOptions_throws_ArgumentException_if_block_is_not_Select_component(string elementTypeAlias, bool exceptionExpected)
        {
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideSelectOptions(Array.Empty<SelectOption>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
            else
            {
                Assert.DoesNotThrow(() => content.Object.OverrideSelectOptions(Array.Empty<SelectOption>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
        }

        [Test]
        public void OverrideSelectOptions_replaces_options()
        {
            // Arrange
            var testContext = new UmbracoTestContext()
                .SetupContentType(ElementTypeAliases.SelectOption);

            var originalOptions = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.SelectOption)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SelectOptionValue, "1")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SelectOptionLabel, "Item 1")
                    .Object),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.SelectOption)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SelectOptionValue, "2")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SelectOptionLabel, "Item 2")
                    .Object
                    )
            });

            var content = new OverridablePublishedElement(
                UmbracoContentFactory.CreateContent<IPublishedElement>(ElementTypeAliases.Select)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.SelectOptions, originalOptions)
                    .Object);

            var replacement = new[]
            {
                new SelectOption("3", "Item 3"),
                new SelectOption("4", "Item 4")
            };

            // Act
            content.OverrideSelectOptions(replacement, testContext.PublishedSnapshotAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(PropertyAliases.SelectOptions);

            Assert.That(options, Is.Not.Null);
            Assert.That(options!.Count(), Is.EqualTo(replacement.Count()));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.SelectOptionValue), Is.EqualTo("3"));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.SelectOptionLabel), Is.EqualTo("Item 3"));
            Assert.That(options[1].Content.Value<string>(PropertyAliases.SelectOptionValue), Is.EqualTo("4"));
            Assert.That(options[1].Content.Value<string>(PropertyAliases.SelectOptionLabel), Is.EqualTo("Item 4"));
        }

        [TestCase(ElementTypeAliases.SummaryCard, false)]
        [TestCase(ElementTypeAliases.SummaryList, true)]
        public void OverrideSummaryCardActions_throws_ArgumentException_if_block_is_not_Summary_card_component(string elementTypeAlias, bool exceptionExpected)
        {
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideSummaryCardActions(Array.Empty<SummaryListAction>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
            else
            {
                Assert.DoesNotThrow(() => content.Object.OverrideSummaryCardActions(Array.Empty<SummaryListAction>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
        }

        [Test]
        public void OverrideSummaryCardActions_replaces_actions()
        {
            // Arrange
            var testContext = new UmbracoTestContext()
                .SetupContentType(ElementTypeAliases.SummaryListAction);

            var originalItems = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.SummaryListAction)
                        .SetupUmbracoMultiUrlPickerPropertyValue(PropertyAliases.SummaryListActionLink, new Link{ Url="https://example.org/one" })
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SummaryListActionLinkText, "Item 1")
                    .Object),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.SummaryListAction)
                        .SetupUmbracoMultiUrlPickerPropertyValue(PropertyAliases.SummaryListActionLink, new Link{ Url="https://example.org/two" })
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SummaryListItemValue, "Item 2")
                    .Object
                    )
            });

            var content = new OverridablePublishedElement(
                UmbracoContentFactory.CreateContent<IPublishedElement>(ElementTypeAliases.SummaryCard)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.SummaryCardActions, originalItems)
                    .Object);

            var replacement = new[]
            {
                new SummaryListAction(new Link{ Url="https://example.org/three" }, "Item 3"),
                new SummaryListAction(new Link{ Url="https://example.org/four" }, "Item 4")
            };

            // Act
            content.OverrideSummaryCardActions(replacement, testContext.PublishedSnapshotAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(PropertyAliases.SummaryCardActions);

            Assert.That(options, Is.Not.Null);
            Assert.That(options!.Count(), Is.EqualTo(replacement.Count()));
            Assert.That(options![0].Content.Value<Link>(PropertyAliases.SummaryListActionLink)?.Url, Is.EqualTo("https://example.org/three"));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.SummaryListActionLinkText), Is.EqualTo("Item 3"));
            Assert.That(options[1].Content.Value<Link>(PropertyAliases.SummaryListActionLink)?.Url, Is.EqualTo("https://example.org/four"));
            Assert.That(options[1].Content.Value<string>(PropertyAliases.SummaryListActionLinkText), Is.EqualTo("Item 4"));
        }


        [TestCase(ElementTypeAliases.SummaryCard, false)]
        [TestCase(ElementTypeAliases.SummaryList, false)]
        [TestCase(ElementTypeAliases.Radios, true)]
        public void OverrideSummaryListItems_throws_ArgumentException_if_block_is_not_Summary_card_or_Summary_list_component(string elementTypeAlias, bool exceptionExpected)
        {
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideSummaryListItems(Array.Empty<SummaryListItem>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
            else
            {
                Assert.DoesNotThrow(() => content.Object.OverrideSummaryListItems(Array.Empty<SummaryListItem>(), Mock.Of<IPublishedSnapshotAccessor>()));
            }
        }

        [TestCase(ElementTypeAliases.SummaryList, PropertyAliases.SummaryListItems)]
        [TestCase(ElementTypeAliases.SummaryCard, PropertyAliases.SummaryCardListItems)]
        public void OverrideSummaryListItems_replaces_items(string componentAlias, string listItemsPropertyAlias)
        {
            // Arrange
            var testContext = new UmbracoTestContext()
                .SetupContentType(ElementTypeAliases.SummaryListItem)
                .SetupContentType(ElementTypeAliases.SummaryListItemSettings)
                .SetupContentType(ElementTypeAliases.SummaryListAction);

            var originalItems = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.SummaryListItem)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SummaryListItemKey, "1")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SummaryListItemValue, "Item 1")
                    .Object),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.SummaryListItem)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SummaryListItemKey, "2")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.SummaryListItemValue, "Item 2")
                    .Object
                    )
            });

            var content = new OverridablePublishedElement(
                UmbracoContentFactory.CreateContent<IPublishedElement>(componentAlias)
                    .SetupUmbracoBlockListPropertyValue(listItemsPropertyAlias, originalItems)
                    .Object);

            var replacement = new[]
            {
                new SummaryListItem("3", new HtmlEncodedString("Item 3")),
                new SummaryListItem("4", new HtmlEncodedString("Item 4"))
            };
            replacement[0].Actions.Add(new SummaryListAction(new Link { Url = "https://example.org/test" }, "Example"));

            // Act
            content.OverrideSummaryListItems(replacement, testContext.PublishedSnapshotAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(listItemsPropertyAlias);

            Assert.That(options, Is.Not.Null);
            Assert.That(options!.Count(), Is.EqualTo(replacement.Count()));
            Assert.That(options![0].Content.Value<string>(PropertyAliases.SummaryListItemKey), Is.EqualTo("3"));
            Assert.That(options![0].Content.Value<IHtmlEncodedString>(PropertyAliases.SummaryListItemValue)?.ToHtmlString(), Is.EqualTo("Item 3"));

            var actions = options[0].Content.Value<OverridableBlockListModel>(PropertyAliases.SummaryListItemActions);
            Assert.That(actions, Is.Not.Null);
            Assert.That(actions!.Count(), Is.EqualTo(replacement[0].Actions.Count));
            Assert.That(actions![0].Content.Value<Link>(PropertyAliases.SummaryListActionLink)?.Url, Is.EqualTo("https://example.org/test"));
            Assert.That(actions![0].Content.Value<string>(PropertyAliases.SummaryListActionLinkText), Is.EqualTo("Example"));

            Assert.That(options[1].Content.Value<string>(PropertyAliases.SummaryListItemKey), Is.EqualTo("4"));
            Assert.That(options[1].Content.Value<IHtmlEncodedString>(PropertyAliases.SummaryListItemValue)?.ToHtmlString(), Is.EqualTo("Item 4"));
        }
    }
}