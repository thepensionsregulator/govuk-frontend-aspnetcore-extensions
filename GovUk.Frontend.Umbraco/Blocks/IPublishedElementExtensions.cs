using ThePensionsRegulator.Umbraco;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public static class IPublishedElementExtensions
    {
        /// <summary>
        /// For the settings of a block with a "cssClasses" property, gets the classes as a list.
        /// </summary>
        /// <param name="blockSettings">The settings of a block with a "cssClasses" property.</param>
        /// <returns>An overridable list of classes.</returns>
        public static TokenList ClassList(this IPublishedElement blockSettings)
        {
            return new TokenList(blockSettings, PropertyAliases.CssClasses);
        }

        /// <summary>
        /// For the settings of a block with a "cssClassesForRow" property, gets the classes as a list.
        /// </summary>
        /// <param name="blockSettings">The settings of a block with a "cssClasses" property.</param>
        /// <returns>An overridable list of classes.</returns>
        public static TokenList GridRowClassList(this IPublishedElement blockSettings)
        {
            return new TokenList(blockSettings, PropertyAliases.CssClassesForRow);
        }

        /// <summary>
        /// For the settings of a block with a "cssClassesForColumn" property, gets the classes as a list.
        /// </summary>
        /// <param name="blockSettings">The settings of a block with a "cssClasses" property.</param>
        /// <returns>An overridable list of classes.</returns>
        public static TokenList GridColumnClassList(this IPublishedElement blockSettings)
        {
            return new TokenList(blockSettings, PropertyAliases.CssClassesForColumn);
        }
    }
}
