namespace GovUk.Frontend.Umbraco.Services
{
    /// <inheritdoc />
    public class TprSectionCardsColumnClassProvider : IDefaultColumnClassProvider
    {
        /// <inheritdoc />
        public string ColumnClasses => GovUkClassNames.ColumnFullWidth;

        /// <inheritdoc />
        public bool IsProvider(string contentTypeAlias) => contentTypeAlias == "tprSectionCards";
    }
}
