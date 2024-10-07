using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;

namespace GovUk.Frontend.Umbraco.PropertyEditors
{
    /// <summary>
    /// Register the built-in rich text property editor to participate in formatting of rich text
    /// property values by one or more registered <see cref="IPropertyValueFormatter"/> instances.
    /// </summary>
    public class DefaultRichTextPropertyEditorAliasProvider : IRichTextPropertyEditorAliasProvider
    {
        /// <inheritdoc/>
        public IEnumerable<string> PropertyEditorAliases()
        {
            return [Constants.PropertyEditors.Aliases.TinyMce];
        }
    }
}
