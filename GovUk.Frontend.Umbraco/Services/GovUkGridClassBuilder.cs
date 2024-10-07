namespace GovUk.Frontend.Umbraco.Services
{
    public static class GovUkGridClassBuilder
    {
        public static string BuildGridRowClass(string? customClass)
        {
            string rowClass = ($"{HtmlClassNames.Row} {customClass}").TrimEnd();
            return rowClass;
        }
        public static string BuildGridColumnClass(string? columnSizeClass, string? fromDesktopClass, string? customClass, string? forBlockOfContentTypeAlias = null, bool defaultToFullWidth = false)
        {
            if (!string.IsNullOrEmpty(columnSizeClass)) { columnSizeClass = $"{HtmlClassNames.Column}-{columnSizeClass}"; }
            if (!string.IsNullOrEmpty(fromDesktopClass)) { fromDesktopClass = $"{HtmlClassNames.Column}-{fromDesktopClass}-from-desktop"; }
            var columnClass = (columnSizeClass + " " + fromDesktopClass).Trim();
            if (string.IsNullOrEmpty(columnClass)) { columnClass = DefaultColumnClass(forBlockOfContentTypeAlias, defaultToFullWidth); }
            columnClass = ($"{HtmlClassNames.Column} {columnClass} {customClass}").TrimEnd(); // .govuk-grid-column is not part of the GOV.UK design system but it's useful to be able to target any column
            return columnClass;
        }

        private static string DefaultColumnClass(string? forBlockOfContentTypeAlias, bool defaultToFullWidth)
        {
            return defaultToFullWidth || forBlockOfContentTypeAlias == ElementTypeAliases.Caption || forBlockOfContentTypeAlias == ElementTypeAliases.PageHeading ? HtmlClassNames.ColumnFullWidth : HtmlClassNames.ColumnTwoThirdsFromDesktop;
        }
    }
}
