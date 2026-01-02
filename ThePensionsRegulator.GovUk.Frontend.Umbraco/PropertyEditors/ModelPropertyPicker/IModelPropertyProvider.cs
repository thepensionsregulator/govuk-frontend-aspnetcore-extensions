namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ModelPropertyPicker
{
    /// <summary>
    /// Provides a mechanism for retrieving the C# property names defined for a model associated with a specified
    /// document type alias.
    /// </summary>
    public interface IModelPropertyProvider
    {
        /// <summary>
        /// Gets the C# property names for the model associated with the specified alias.
        /// </summary>
        /// <param name="documentTypeAlias">Umbraco document type alias</param>
        /// <returns></returns>
        IEnumerable<string> GetPropertyNames(string documentTypeAlias);
    }
}