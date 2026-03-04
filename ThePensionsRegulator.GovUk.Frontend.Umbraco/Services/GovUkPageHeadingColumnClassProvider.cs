namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    /// <inheritdoc />
    public class GovUkPageHeadingColumnClassProvider : IDefaultColumnClassProvider
    {
        /// <inheritdoc />
        public string ColumnClasses => GovUkClassNames.ColumnFullWidth;

        /// <inheritdoc />
        public bool IsProvider(string contentTypeAlias) => contentTypeAlias == ElementTypeAliases.PageHeading;
    }
}
