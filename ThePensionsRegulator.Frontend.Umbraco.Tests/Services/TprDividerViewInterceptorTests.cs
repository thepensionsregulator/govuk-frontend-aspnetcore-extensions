using Moq;
using Umbraco.Cms.Core.Models.PublishedContent;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using GovUkElementTypeAliases = ThePensionsRegulator.GovUk.Frontend.Umbraco.ElementTypeAliases;
using GovUkPropertyAliases = ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyAliases;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprDividerViewInterceptorTests
    {
        [Theory]
        [InlineData(TprElementTypeAliases.Box, TprElementTypeAliases.BoxSettings)]
        [InlineData(GovUkElementTypeAliases.GridOneQuarter, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridOneThird, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridOneHalf, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridTwoThirds, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridThreeQuarters, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridFullWidth, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridOneQuarterThreeQuarters, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridOneThirdTwoThirds, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridTwoEqualColumns, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridTwoThirdsOneThird, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridThreeQuartersOneQuarter, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridThreeEqualColumns, GovUkElementTypeAliases.GridThreeColumnLayoutSettings)]
        [InlineData(GovUkElementTypeAliases.GridFourEqualColumns, GovUkElementTypeAliases.GridFourColumnLayoutSettings)]
        public void Layout_block_is_not_updated(string contentAlias, string settingsAlias)
        {
            // Arrange
            var blockViewModel = CreateBlockView(contentAlias, settingsAlias, false);
            ((OverridableBlockGridItem)blockViewModel.CurrentBlock)
                .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));

            var interceptor = new TprDividerViewInterceptor(Mock.Of<IPublishedValueFallback>());

            // Act
            interceptor.InterceptBlockView(blockViewModel);

            // Assert
            Assert.False(blockViewModel.OpenGridRowAndColumn);
            Assert.False(blockViewModel.CloseGridRowAndColumn);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Grid_row_rendered_and_class_added_to_page_heading_if_missing(bool classWasAlreadyPresent)
        {
            // Arrange
            var blockViewModel = CreateBlockView(GovUkElementTypeAliases.PageHeading, GovUkElementTypeAliases.PageHeadingSettings, classWasAlreadyPresent, classWasAlreadyPresent ? TprClassNames.Divider : null);

            var interceptor = new TprDividerViewInterceptor(Mock.Of<IPublishedValueFallback>());

            // Act
            interceptor.InterceptBlockView(blockViewModel);

            // Assert
            Assert.Equal($"{GovUkClassNames.Row} {TprClassNames.Divider}", blockViewModel.RowClasses);
            Assert.True(blockViewModel.OpenGridRowAndColumn);
            Assert.True(blockViewModel.CloseGridRowAndColumn);
        }

        [Theory]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, true, true)]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, true, false)]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, false, false)]

        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, true, true)]
        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, true, false)]
        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, false, false)]

        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, true, true)]
        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, true, false)]
        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, false, false)]

        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, true, true)]
        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, true, false)]
        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, false, false)]

        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, false)]
        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, false, false)]

        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, false)]
        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, false, false)]

        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, false)]
        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, false, false)]

        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, false)]
        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, false, false)]
        public void If_component_includes_page_heading_grid_row_rendered_and_class_added_if_missing(string contentAlias, string settingsAlias, bool isFieldset, bool legendIsPageHeading, bool classWasAlreadyPresent)
        {
            // Arrange
            var isPageHeadingProperty = isFieldset ? GovUkPropertyAliases.FieldsetLegendIsPageHeading : GovUkPropertyAliases.LabelIsPageHeading;
            var dividerClass = isFieldset ? TprClassNames.DividerForFieldsetWithLegendAsPageHeading : TprClassNames.DividerForFormComponentWithLabelAsPageHeading;

            var blockViewModel = CreateBlockView(contentAlias, settingsAlias, classWasAlreadyPresent, classWasAlreadyPresent ? dividerClass : null);
            Mock.Get(blockViewModel.CurrentBlock.Settings!).SetupUmbracoBooleanPropertyValue(isPageHeadingProperty, legendIsPageHeading);

            var interceptor = new TprDividerViewInterceptor(Mock.Of<IPublishedValueFallback>());

            // Act
            interceptor.InterceptBlockView(blockViewModel);

            // Assert
            if (legendIsPageHeading)
            {
                Assert.Equal($"{GovUkClassNames.Row} {dividerClass}", blockViewModel.RowClasses);
                Assert.True(blockViewModel.OpenGridRowAndColumn);
                Assert.True(blockViewModel.CloseGridRowAndColumn);
            }
            else
            {
                Assert.Equal(GovUkClassNames.Row, blockViewModel.RowClasses);
                Assert.False(blockViewModel.OpenGridRowAndColumn);
                Assert.False(blockViewModel.CloseGridRowAndColumn);
            }
        }

        [Theory]
        [InlineData(GovUkElementTypeAliases.PageHeading, GovUkElementTypeAliases.PageHeadingSettings, false, false, true)]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, false, false)]
        public void Block_following_component_where_class_is_added_sets_OpenGridRowAndColumn_true(string contentAlias, string settingsAlias, bool hasLegendAsHeading, bool hasLabelAsHeading, bool expected)
        {
            // Arrange
            var isPageHeadingProperty = hasLegendAsHeading ? GovUkPropertyAliases.FieldsetLegendIsPageHeading : hasLabelAsHeading ? GovUkPropertyAliases.LabelIsPageHeading : null;

            var blockViewModel = CreateBlockView("content", "settings", false);
            blockViewModel.PreviousBlock = CreateBlock(contentAlias, settingsAlias);
            if (isPageHeadingProperty is not null)
            {
                Mock.Get(blockViewModel.PreviousBlock.Settings!).SetupUmbracoBooleanPropertyValue(isPageHeadingProperty, true);
            }

            var interceptor = new TprDividerViewInterceptor(Mock.Of<IPublishedValueFallback>());

            // Act
            interceptor.InterceptBlockView(blockViewModel);

            // Assert
            Assert.Equal(expected, blockViewModel.OpenGridRowAndColumn);
        }

        [Theory]
        [InlineData(GovUkElementTypeAliases.PageHeading, GovUkElementTypeAliases.PageHeadingSettings, false, false, true)]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, false, true)]
        [InlineData(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, false, false)]
        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, true)]
        [InlineData(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, false, false)]
        public void Block_preceding_component_where_class_is_added_sets_CloseGridRowAndColumn_true(string contentAlias, string settingsAlias, bool hasLegendAsHeading, bool hasLabelAsHeading, bool expected)
        {
            // Arrange
            var isPageHeadingProperty = hasLegendAsHeading ? GovUkPropertyAliases.FieldsetLegendIsPageHeading : hasLabelAsHeading ? GovUkPropertyAliases.LabelIsPageHeading : null;

            var blockViewModel = CreateBlockView("content", "settings", false);
            blockViewModel.NextBlock = CreateBlock(contentAlias, settingsAlias);
            if (isPageHeadingProperty is not null)
            {
                Mock.Get(blockViewModel.NextBlock.Settings!).SetupUmbracoBooleanPropertyValue(isPageHeadingProperty, true);
            }

            var interceptor = new TprDividerViewInterceptor(Mock.Of<IPublishedValueFallback>());

            // Act
            interceptor.InterceptBlockView(blockViewModel);

            // Assert
            Assert.Equal(expected, blockViewModel.CloseGridRowAndColumn);
        }

        private static BlockViewModel CreateBlockView(string contentAlias, string settingsAlias, bool renderGridRowInitialValue, string? existingRowClasses = null)
        {
            var block = CreateBlock(contentAlias, settingsAlias, existingRowClasses);
            var blockView = new BlockViewModel
            {
                CurrentBlock = block,
                OpenGridRowAndColumn = renderGridRowInitialValue,
                CloseGridRowAndColumn = renderGridRowInitialValue
            };
            if (existingRowClasses is not null) { blockView.RowClasses += $" {existingRowClasses}"; }
            return blockView;
        }

        private static OverridableBlockGridItem CreateBlock(string contentAlias, string settingsAlias, string? existingRowClasses = null)
        {
            var settings = UmbracoBlockGridFactory.CreateContentOrSettings(settingsAlias);
            if (existingRowClasses is not null) { settings.SetupUmbracoTextboxPropertyValue(GovUkPropertyAliases.CssClassesForRow, existingRowClasses); }
            return UmbracoBlockGridFactory.CreateOverridableBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings(contentAlias).Object,
                        settings.Object
                    );
        }
    }
}
