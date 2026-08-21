namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    /// <inheritdoc />
    public class GovUkCaptionColumnClassProvider : IDefaultColumnClassProvider
    {
        /// <inheritdoc />
        public string ColumnClasses => GovUkClassNames.ColumnFullWidth;

        /// <inheritdoc />
        public bool IsProvider(string contentTypeAlias) => contentTypeAlias == ElementTypeAliases.Caption;
    }
}
