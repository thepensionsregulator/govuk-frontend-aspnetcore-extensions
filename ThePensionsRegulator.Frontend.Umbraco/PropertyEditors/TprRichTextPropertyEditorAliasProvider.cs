using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Aliases = ThePensionsRegulator.Frontend.Umbraco.TprPropertyEditorAliases;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors
{
    /// <summary>
    /// Register TPR rich text property editors to participate in formatting of rich text
    /// property values by one or more registered <see cref="IPropertyValueFormatter"/> instances.
    /// </summary>

    public class TprRichTextPropertyEditorAliasProvider : IRichTextPropertyEditorAliasProvider
    {
        /// <inheritdoc/>
        public IEnumerable<string> PropertyEditorAliases()
        {
            return [Aliases.TprHeaderFooterRichText];
        }
    }
}
