using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using Moq;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using GovUkElementTypeAliases = GovUk.Frontend.Umbraco.ElementTypeAliases;
using GovUkPropertyAliases = GovUk.Frontend.Umbraco.PropertyAliases;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
	public class TprDividerViewInterceptorTests
	{
		[TestCase(TprElementTypeAliases.Box, TprElementTypeAliases.BoxSettings)]
		[TestCase(GovUkElementTypeAliases.GridOneQuarter, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridOneThird, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridOneHalf, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridTwoThirds, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridThreeQuarters, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridFullWidth, GovUkElementTypeAliases.GridSingleColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridOneQuarterThreeQuarters, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridOneThirdTwoThirds, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridTwoEqualColumns, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridTwoThirdsOneThird, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridThreeQuartersOneQuarter, GovUkElementTypeAliases.GridTwoColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridThreeEqualColumns, GovUkElementTypeAliases.GridThreeColumnLayoutSettings)]
		[TestCase(GovUkElementTypeAliases.GridFourEqualColumns, GovUkElementTypeAliases.GridFourColumnLayoutSettings)]
		public void Layout_block_is_not_updated(string contentAlias, string settingsAlias)
		{
			// Arrange
			var blockViewModel = CreateBlockView(contentAlias, settingsAlias, false);
			((OverridableBlockGridItem)blockViewModel.CurrentBlock)
				.AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));

			var interceptor = new TprDividerViewInterceptor();

			// Act
			interceptor.InterceptBlockView(blockViewModel);

			// Assert
			Assert.That(blockViewModel.OpenGridRowAndColumn, Is.False);
			Assert.That(blockViewModel.CloseGridRowAndColumn, Is.False);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void Grid_row_rendered_and_class_added_to_page_heading_if_missing(bool classWasAlreadyPresent)
		{
			// Arrange
			var blockViewModel = CreateBlockView(GovUkElementTypeAliases.PageHeading, GovUkElementTypeAliases.PageHeadingSettings, classWasAlreadyPresent, classWasAlreadyPresent ? TprClassNames.Divider : null);

			var interceptor = new TprDividerViewInterceptor();

			// Act
			interceptor.InterceptBlockView(blockViewModel);

			// Assert
			Assert.That(blockViewModel.RowClasses, Is.EqualTo($"{GovUkClassNames.Row} {TprClassNames.Divider}"));
			Assert.That(blockViewModel.OpenGridRowAndColumn, Is.True);
			Assert.That(blockViewModel.CloseGridRowAndColumn, Is.True);
		}

		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, true, true)]
		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, true, false)]
		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, false, false)]

		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, true, true)]
		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, true, false)]
		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, false, false)]

		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, true, true)]
		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, true, false)]
		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, false, false)]

		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, true, true)]
		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, true, false)]
		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, false, false)]

		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, false)]
		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, false, false)]

		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, false)]
		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, false, false)]

		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, false)]
		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, false, false)]

		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, false)]
		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, false, false)]
		public void If_component_includes_page_heading_grid_row_rendered_and_class_added_if_missing(string contentAlias, string settingsAlias, bool isFieldset, bool legendIsPageHeading, bool classWasAlreadyPresent)
		{
			// Arrange
			var isPageHeadingProperty = isFieldset ? GovUkPropertyAliases.FieldsetLegendIsPageHeading : GovUkPropertyAliases.LabelIsPageHeading;
			var dividerClass = isFieldset ? TprClassNames.DividerForFieldsetWithLegendAsPageHeading : TprClassNames.DividerForFormComponentWithLabelAsPageHeading;

			var blockViewModel = CreateBlockView(contentAlias, settingsAlias, classWasAlreadyPresent, classWasAlreadyPresent ? dividerClass : null);
			Mock.Get(blockViewModel.CurrentBlock.Settings).SetupUmbracoBooleanPropertyValue(isPageHeadingProperty, legendIsPageHeading);

			var interceptor = new TprDividerViewInterceptor();

			// Act
			interceptor.InterceptBlockView(blockViewModel);

			// Assert
			if (legendIsPageHeading)
			{
				Assert.That(blockViewModel.RowClasses, Is.EqualTo($"{GovUkClassNames.Row} {dividerClass}"));
				Assert.That(blockViewModel.OpenGridRowAndColumn, Is.True);
				Assert.That(blockViewModel.CloseGridRowAndColumn, Is.True);
			}
			else
			{
				Assert.That(blockViewModel.RowClasses, Is.EqualTo(GovUkClassNames.Row));
				Assert.That(blockViewModel.OpenGridRowAndColumn, Is.False);
				Assert.That(blockViewModel.CloseGridRowAndColumn, Is.False);
			}
		}

		[TestCase(GovUkElementTypeAliases.PageHeading, GovUkElementTypeAliases.PageHeadingSettings, false, false, true)]
		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, false, false)]
		public void Block_following_component_where_class_is_added_sets_OpenGridRowAndColumn_true(string contentAlias, string settingsAlias, bool hasLegendAsHeading, bool hasLabelAsHeading, bool expected)
		{
			// Arrange
			var isPageHeadingProperty = hasLegendAsHeading ? GovUkPropertyAliases.FieldsetLegendIsPageHeading : hasLabelAsHeading ? GovUkPropertyAliases.LabelIsPageHeading : null;

			var blockViewModel = CreateBlockView("content", "settings", false);
			blockViewModel.PreviousBlock = CreateBlock(contentAlias, settingsAlias);
			if (isPageHeadingProperty is not null)
			{
				Mock.Get(blockViewModel.PreviousBlock.Settings).SetupUmbracoBooleanPropertyValue(isPageHeadingProperty, true);
			}

			var interceptor = new TprDividerViewInterceptor();

			// Act
			interceptor.InterceptBlockView(blockViewModel);

			// Assert
			Assert.That(blockViewModel.OpenGridRowAndColumn, Is.EqualTo(expected));
		}

		[TestCase(GovUkElementTypeAliases.PageHeading, GovUkElementTypeAliases.PageHeadingSettings, false, false, true)]
		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.CheckboxesSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.DateInputSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.FieldsetSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, true, false, true)]
		[TestCase(GovUkElementTypeAliases.Radios, GovUkElementTypeAliases.RadiosSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.FileUploadSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.Select, GovUkElementTypeAliases.SelectSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextareaSettings, false, false, false)]
		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, true, true)]
		[TestCase(GovUkElementTypeAliases.TextInput, GovUkElementTypeAliases.TextInputSettings, false, false, false)]
		public void Block_preceding_component_where_class_is_added_sets_CloseGridRowAndColumn_true(string contentAlias, string settingsAlias, bool hasLegendAsHeading, bool hasLabelAsHeading, bool expected)
		{
			// Arrange
			var isPageHeadingProperty = hasLegendAsHeading ? GovUkPropertyAliases.FieldsetLegendIsPageHeading : hasLabelAsHeading ? GovUkPropertyAliases.LabelIsPageHeading : null;

			var blockViewModel = CreateBlockView("content", "settings", false);
			blockViewModel.NextBlock = CreateBlock(contentAlias, settingsAlias);
			if (isPageHeadingProperty is not null)
			{
				Mock.Get(blockViewModel.NextBlock.Settings).SetupUmbracoBooleanPropertyValue(isPageHeadingProperty, true);
			}

			var interceptor = new TprDividerViewInterceptor();

			// Act
			interceptor.InterceptBlockView(blockViewModel);

			// Assert
			Assert.That(blockViewModel.CloseGridRowAndColumn, Is.EqualTo(expected));
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
