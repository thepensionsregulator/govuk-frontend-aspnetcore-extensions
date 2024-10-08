namespace ThePensionsRegulator.Umbraco.PropertyEditors
{
    /// <summary>
    /// Register one or more property editor aliases which should be regarded as rich text property editors, and participate in formatting of rich text
    /// property values by one or more registered <see cref="IPropertyValueFormatter"/> instances.
    /// </summary>
    public interface IRichTextPropertyEditorAliasProvider
    {
        /// <summary>
        /// Gets the property editor aliases which should be regarded as rich text property editors.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> PropertyEditorAliases();
    }
}
