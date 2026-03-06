namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    /// <summary>
    /// Provides default HTML classes for the grid column surrounding a specific block based on its content type, which will override the GOV.UK default of <c>.govuk-grid-column-two-thirds-from-desktop</c>.
    /// </summary>
    public interface IDefaultColumnClassProvider
    {
        /// <summary>
        /// Gets a value indicating whether the provider can provide default column classes for a block based on its content type.
        /// </summary>
        /// <param name="contentTypeAlias">The alias of the content type for the block content.</param>
        /// <returns>A value indicating whether the provider can provide default column classes for a block.</returns>
        bool IsProvider(string contentTypeAlias);

        /// <summary>
        /// Gets HTML classes for a grid column, which will override the GOV.UK default of <c>.govuk-grid-column-two-thirds-from-desktop</c>.
        /// Check that <see cref="IsProvider(string contentTypeAlias)" /> returns <c>true</c> for the block before using this property.
        /// </summary>
        /// <returns>A space-separated list of HTML classes, or an empty string.</returns>
        string ColumnClasses { get; }
    }
}
