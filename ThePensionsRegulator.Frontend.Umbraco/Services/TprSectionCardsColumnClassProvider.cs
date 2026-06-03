using System;

namespace GovUk.Frontend.Umbraco.Services
{
    [Obsolete("Use TprSectionCardsColumnClassProvider")]
    public class TPRSectionCardsColumnClassProvider : TprSectionCardsColumnClassProvider { }

    /// <inheritdoc />
    public class TprSectionCardsColumnClassProvider : IDefaultColumnClassProvider
    {
        /// <inheritdoc />
        public string ColumnClasses => GovUkClassNames.ColumnFullWidth;

        /// <inheritdoc />
        public bool IsProvider(string contentTypeAlias) => contentTypeAlias == "tprSectionCards";
    }
}
