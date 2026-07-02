using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Blocks
{
    public class IOverridablePublishedElementExtensionsTests
    {
        [Theory]
        [InlineData(ElementTypeAliases.Checkboxes, false)]
        [InlineData(ElementTypeAliases.Radios, true)]
        public void OverrideCheckboxes_throws_ArgumentException_if_block_is_not_Checkboxes_component(string elementTypeAlias, bool exceptionExpected)
        {
            using var testContext = new UmbracoTestContext();
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideCheckboxes(Array.Empty<Checkbox>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
            }
            else
            {
                var exception = Record.Exception(() => content.Object.OverrideCheckboxes(Array.Empty<Checkbox>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
                Assert.Null(exception);
            }
        }

        [Fact]
        public void OverrideCheckboxes_replaces_checkboxes()
        {
            // Arrange
            using var testContext = new UmbracoTestContext()
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

            var content =
                UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(ElementTypeAliases.Checkboxes)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.Checkboxes, originalItems)
                    .Object;

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
            content.OverrideCheckboxes(replacement, testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.Checkboxes);

            Assert.NotNull(options);
            Assert.Equal(replacement.Count(), options!.Count());
            Assert.Equal("3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CheckboxValue));
            Assert.Equal("Item 3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CheckboxLabel));
            Assert.Equal("<i>Hint 3</i>", options![0].Content.Value<IHtmlEncodedString>(testContext.PublishedValueFallback.Object, PropertyAliases.Hint)?.ToHtmlString());
            Assert.NotNull(options![0].Content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.CheckboxConditionalBlocks));
            Assert.Equal("item-3", options![0].Settings?.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CssClasses));
            Assert.Equal("divider", options[1].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CheckboxesDividerText));
            Assert.Equal("4", options[2].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CheckboxValue));
            Assert.Equal("Item 4", options[2].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CheckboxLabel));
        }

        [Theory]
        [InlineData(ElementTypeAliases.Checkboxes, true)]
        [InlineData(ElementTypeAliases.Radios, false)]
        public void OverrideRadioButtons_throws_ArgumentException_if_block_is_not_Radios_component(string elementTypeAlias, bool exceptionExpected)
        {
            using var testContext = new UmbracoTestContext();
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideRadioButtons(Array.Empty<RadioButton>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
            }
            else
            {
                var exception = Record.Exception(() => content.Object.OverrideRadioButtons(Array.Empty<RadioButton>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
                Assert.Null(exception);
            }
        }

        [Fact]
        public void OverrideRadioButtons_replaces_radio_buttons()
        {
            // Arrange
            using var testContext = new UmbracoTestContext()
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

            var content = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(ElementTypeAliases.Radios)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.RadioButtons, originalItems)
                    .Object;

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
            content.OverrideRadioButtons(replacement, testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.RadioButtons);

            Assert.NotNull(options);
            Assert.Equal(replacement.Count(), options!.Count());
            Assert.Equal("3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.RadioButtonValue));
            Assert.Equal("Item 3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.RadioButtonLabel));
            Assert.Equal("<i>Hint 3</i>", options![0].Content.Value<IHtmlEncodedString>(testContext.PublishedValueFallback.Object, PropertyAliases.Hint)?.ToHtmlString());
            Assert.NotNull(options![0].Content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.RadioConditionalBlocks));
            Assert.Equal("item-3", options![0].Settings?.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.CssClasses));
            Assert.Equal("divider", options[1].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.RadiosDividerText));
            Assert.Equal("4", options[2].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.RadioButtonValue));
            Assert.Equal("Item 4", options[2].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.RadioButtonLabel));
        }

        [Theory]
        [InlineData(ElementTypeAliases.Select, false)]
        [InlineData(ElementTypeAliases.Radios, true)]
        public void OverrideSelectOptions_throws_ArgumentException_if_block_is_not_Select_component(string elementTypeAlias, bool exceptionExpected)
        {
            using var testContext = new UmbracoTestContext();
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideSelectOptions(Array.Empty<SelectOption>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
            }
            else
            {
                var exception = Record.Exception(() => content.Object.OverrideSelectOptions(Array.Empty<SelectOption>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
                Assert.Null(exception);
            }
        }

        [Fact]
        public void OverrideSelectOptions_replaces_options()
        {
            // Arrange
            using var testContext = new UmbracoTestContext()
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

            var content = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(ElementTypeAliases.Select)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.SelectOptions, originalOptions)
                    .Object;

            var replacement = new[]
            {
                new SelectOption("3", "Item 3"),
                new SelectOption("4", "Item 4")
            };

            // Act
            content.OverrideSelectOptions(replacement, testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.SelectOptions);

            Assert.NotNull(options);
            Assert.Equal(replacement.Count(), options!.Count());
            Assert.Equal("3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SelectOptionValue));
            Assert.Equal("Item 3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SelectOptionLabel));
            Assert.Equal("4", options[1].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SelectOptionValue));
            Assert.Equal("Item 4", options[1].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SelectOptionLabel));
        }

        [Theory]
        [InlineData(ElementTypeAliases.SummaryCard, false)]
        [InlineData(ElementTypeAliases.SummaryList, true)]
        public void OverrideSummaryCardActions_throws_ArgumentException_if_block_is_not_Summary_card_component(string elementTypeAlias, bool exceptionExpected)
        {
            using var testContext = new UmbracoTestContext();
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideSummaryCardActions(Array.Empty<SummaryListAction>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
            }
            else
            {
                var exception = Record.Exception(() => content.Object.OverrideSummaryCardActions(Array.Empty<SummaryListAction>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
                Assert.Null(exception);
            }
        }

        [Fact]
        public void OverrideSummaryCardActions_replaces_actions()
        {
            // Arrange
            using var testContext = new UmbracoTestContext()
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

            var content = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(ElementTypeAliases.SummaryCard)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.SummaryCardActions, originalItems)
                    .Object;

            var replacement = new[]
            {
                new SummaryListAction(new Link{ Url="https://example.org/three" }, "Item 3"),
                new SummaryListAction(new Link{ Url="https://example.org/four" }, "Item 4")
            };

            // Act
            content.OverrideSummaryCardActions(replacement, testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryCardActions);

            Assert.NotNull(options);
            Assert.Equal(replacement.Count(), options!.Count());
            Assert.Equal("https://example.org/three", options![0].Content.Value<Link>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListActionLink)?.Url);
            Assert.Equal("Item 3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListActionLinkText));
            Assert.Equal("https://example.org/four", options[1].Content.Value<Link>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListActionLink)?.Url);
            Assert.Equal("Item 4", options[1].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListActionLinkText));
        }


        [Theory]
        [InlineData(ElementTypeAliases.SummaryCard, false)]
        [InlineData(ElementTypeAliases.SummaryList, false)]
        [InlineData(ElementTypeAliases.Radios, true)]
        public void OverrideSummaryListItems_throws_ArgumentException_if_block_is_not_Summary_card_or_Summary_list_component(string elementTypeAlias, bool exceptionExpected)
        {
            using var testContext = new UmbracoTestContext();
            var content = UmbracoBlockListFactory.CreateContentOrSettings(elementTypeAlias);

            if (exceptionExpected)
            {
                Assert.Throws<ArgumentException>(() => content.Object.OverrideSummaryListItems(Array.Empty<SummaryListItem>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
            }
            else
            {
                var exception = Record.Exception(() => content.Object.OverrideSummaryListItems(Array.Empty<SummaryListItem>(), testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object));
                Assert.Null(exception);
            }
        }

        [Theory]
        [InlineData(ElementTypeAliases.SummaryList, PropertyAliases.SummaryListItems)]
        [InlineData(ElementTypeAliases.SummaryCard, PropertyAliases.SummaryCardListItems)]
        public void OverrideSummaryListItems_replaces_items(string componentAlias, string listItemsPropertyAlias)
        {
            // Arrange
            using var testContext = new UmbracoTestContext()
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

            var content = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(componentAlias)
                .SetupUmbracoBlockListPropertyValue(listItemsPropertyAlias, originalItems)
                .Object;

            var replacement = new[]
            {
                new SummaryListItem("3", new HtmlEncodedString("Item 3")),
                new SummaryListItem("4", new HtmlEncodedString("Item 4"))
            };
            replacement[0].Actions.Add(new SummaryListAction(new Link { Url = "https://example.org/test" }, "Example"));

            // Act
            content.OverrideSummaryListItems(replacement, testContext.PublishedContentTypeCache.Object, testContext.VariationContextAccessor.Object);

            // Assert
            var options = content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, listItemsPropertyAlias);

            Assert.NotNull(options);
            Assert.Equal(replacement.Count(), options!.Count());
            Assert.Equal("3", options![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListItemKey));
            Assert.Equal("Item 3", options![0].Content.Value<IHtmlEncodedString>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListItemValue)?.ToHtmlString());

            var actions = options[0].Content.Value<OverridableBlockListModel>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListItemActions);
            Assert.NotNull(actions);
            Assert.Equal(replacement[0].Actions.Count, actions!.Count());
            Assert.Equal("https://example.org/test", actions![0].Content.Value<Link>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListActionLink)?.Url);
            Assert.Equal("Example", actions![0].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListActionLinkText));

            Assert.Equal("4", options[1].Content.Value<string>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListItemKey));
            Assert.Equal("Item 4", options[1].Content.Value<IHtmlEncodedString>(testContext.PublishedValueFallback.Object, PropertyAliases.SummaryListItemValue)?.ToHtmlString());
        }
    }
}

