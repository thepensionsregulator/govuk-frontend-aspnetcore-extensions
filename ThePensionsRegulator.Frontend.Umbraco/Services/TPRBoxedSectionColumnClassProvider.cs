namespace GovUk.Frontend.Umbraco.Services
{
    /// <inheritdoc />
    public class TPRBoxedSectionColumnClassProvider : IDefaultColumnClassProvider
    {
        /// <inheritdoc />
        public string ColumnClasses => GovUkClassNames.ColumnFullWidth;

        /// <inheritdoc />
        public bool IsProvider(string contentTypeAlias) => contentTypeAlias == "tprBoxedSection";
    }
}
