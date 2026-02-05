using GovUk.Frontend.Umbraco.Blocks;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using GovUkElementTypeAliases = GovUk.Frontend.Umbraco.ElementTypeAliases;
using GovUkPropertyAliases = GovUk.Frontend.Umbraco.PropertyAliases;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
	/// <summary>
	/// Adds a decorative horizontal divider after the main heading on a page.
	/// </summary>
	public class TprDividerViewInterceptor : IBlockViewInterceptor
	{
		private List<string> _fieldsetAliases = [GovUkElementTypeAliases.Checkboxes, GovUkElementTypeAliases.DateInput, GovUkElementTypeAliases.Fieldset, GovUkElementTypeAliases.Radios];
		private List<string> _labelAliases = [GovUkElementTypeAliases.FileUpload, GovUkElementTypeAliases.Select, GovUkElementTypeAliases.Textarea, GovUkElementTypeAliases.TextInput];

		/// <inheritdoc/>
		public void InterceptBlockView(BlockViewModel blockViewModel)
		{
			if (IsLayoutBlock(blockViewModel.CurrentBlock)) { return; }

			var blockAlias = blockViewModel.CurrentBlock.Content.ContentType.Alias;
			if (blockAlias == GovUkElementTypeAliases.PageHeading)
			{
				ApplyClassToRowAndForceRowToRender(blockViewModel, TprClassNames.Divider);
			}

			if (_fieldsetAliases.Contains(blockAlias) && blockViewModel.CurrentBlock!.Settings.Value<bool>(GovUkPropertyAliases.FieldsetLegendIsPageHeading))
			{
				ApplyClassToRowAndForceRowToRender(blockViewModel, TprClassNames.DividerForFieldsetWithLegendAsPageHeading);
			}

			if (_labelAliases.Contains(blockAlias) && blockViewModel.CurrentBlock!.Settings.Value<bool>(GovUkPropertyAliases.LabelIsPageHeading))
			{
				ApplyClassToRowAndForceRowToRender(blockViewModel, TprClassNames.DividerForFormComponentWithLabelAsPageHeading);
			}

			if (AdjacentBlockRequiresClass(blockViewModel.PreviousBlock)) { blockViewModel.OpenGridRowAndColumn = true; }
			if (AdjacentBlockRequiresClass(blockViewModel.NextBlock)) { blockViewModel.CloseGridRowAndColumn = true; }
		}

		private bool AdjacentBlockRequiresClass(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? adjacentBlock)
		{
			var aliasOfAdjacentBlock = adjacentBlock?.Content.ContentType.Alias ?? string.Empty;
			return (aliasOfAdjacentBlock == GovUkElementTypeAliases.PageHeading
				|| (_fieldsetAliases.Contains(aliasOfAdjacentBlock) && adjacentBlock!.Settings.Value<bool>(GovUkPropertyAliases.FieldsetLegendIsPageHeading))
				|| (_labelAliases.Contains(aliasOfAdjacentBlock) && adjacentBlock!.Settings.Value<bool>(GovUkPropertyAliases.LabelIsPageHeading))
				);
		}

		/// <summary>
		/// Layout blocks are those which take control of rendering width containers, grid rows and columns in a block grid.
		/// </summary>
		private static bool IsLayoutBlock(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
		{
			return (block is OverridableBlockGridItem gridBlock) && gridBlock.Areas.Any();
		}

		private static void ApplyClassToRowAndForceRowToRender(BlockViewModel blockViewModel, string className)
		{
			var classList = blockViewModel.CurrentBlock.GridRowClassList();
			if (!classList.Contains(className))
			{
				blockViewModel.RowClasses += $" {className}";
				blockViewModel.OpenGridRowAndColumn = true;
				blockViewModel.CloseGridRowAndColumn = true;
			}
		}
	}
}
