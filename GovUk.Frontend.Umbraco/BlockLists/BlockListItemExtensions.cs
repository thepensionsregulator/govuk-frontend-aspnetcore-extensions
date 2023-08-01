using ThePensionsRegulator.Umbraco;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.BlockLists
{
	public static class BlockListItemExtensions
	{
		/// <summary>
		/// For a block with a "cssClasses" property in its settings, gets the classes as a list.
		/// </summary>
		/// <param name="blockListItem">A block with a "cssClasses" property in its settings.</param>
		/// <returns>An overridable list of classes.</returns>
		public static TokenList ClassList(this BlockListItem blockListItem)
		{
			return new TokenList(blockListItem.Settings, PropertyAliases.CssClasses);
		}

		/// <summary>
		/// For a block with a "cssClassesForRow" property in its settings, gets the classes as a list.
		/// </summary>
		/// <param name="blockListItem">A block with a "cssClasses" property in its settings.</param>
		/// <returns>An overridable list of classes.</returns>
		public static TokenList ClassListForGridRow(this BlockListItem blockListItem)
		{
			return new TokenList(blockListItem.Settings, PropertyAliases.CssClassesForRow);
		}

		/// <summary>
		/// For a block with a "cssClassesForColumn" property in its settings, gets the classes as a list.
		/// </summary>
		/// <param name="blockListItem">A block with a "cssClasses" property in its settings.</param>
		/// <returns>An overridable list of classes.</returns>
		public static TokenList ClassListForGridColumn(this BlockListItem blockListItem)
		{
			return new TokenList(blockListItem.Settings, PropertyAliases.CssClassesForColumn);
		}
	}
}
