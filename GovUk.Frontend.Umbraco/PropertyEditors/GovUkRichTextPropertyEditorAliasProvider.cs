using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Aliases = GovUk.Frontend.Umbraco.PropertyEditorAliases;

namespace GovUk.Frontend.Umbraco.PropertyEditors
{
    /// <summary>
    /// Register GOV.UK rich text property editors to participate in formatting of rich text
    /// property values by one or more registered <see cref="IPropertyValueFormatter"/> instances.
    /// </summary>

    internal class GovUkRichTextPropertyEditorAliasProvider : IRichTextPropertyEditorAliasProvider
    {
        /// <inheritdoc/>
        public IEnumerable<string> PropertyEditorAliases()
        {
            return [Aliases.GovUkInlineRichText, Aliases.GovUkInlineInverseRichText];
        }
    }
}
