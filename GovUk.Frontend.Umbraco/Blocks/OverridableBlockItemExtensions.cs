using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
	public static class OverridableBlockItemExtensions
	{
		/// <summary>
		/// For a block with a "cssClasses" property in its settings, gets the classes as a list.
		/// </summary>
		/// <param name="block">A block with a "cssClasses" property in its settings.</param>
		/// <returns>An overridable list of classes.</returns>
		public static TokenList ClassList(this IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
		{
			return new TokenList(block.Settings, PropertyAliases.CssClasses);
		}

		/// <summary>
		/// For a block with a "cssClassesForRow" property in its settings, gets the classes as a list.
		/// </summary>
		/// <param name="block">A block with a "cssClasses" property in its settings.</param>
		/// <returns>An overridable list of classes.</returns>
		public static TokenList GridRowClassList(this IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
		{
			return new TokenList(block.Settings, PropertyAliases.CssClassesForRow);
		}

		/// <summary>
		/// For a block with a "cssClassesForColumn" property in its settings, gets the classes as a list.
		/// </summary>
		/// <param name="block">A block with a "cssClasses" property in its settings.</param>
		/// <returns>An overridable list of classes.</returns>
		public static TokenList GridColumnClassList(this IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
		{
			return new TokenList(block.Settings, PropertyAliases.CssClassesForColumn);
		}
	}
}
