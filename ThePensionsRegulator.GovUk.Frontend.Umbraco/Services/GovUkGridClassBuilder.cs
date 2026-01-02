namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class GovUkGridClassBuilder(IEnumerable<IDefaultColumnClassProvider> _columnClassProviders) : IGovUkGridClassBuilder
    {
        public string BuildGridRowClasses(string? customClass)
        {
            string rowClass = ($"{GovUkClassNames.Row} {customClass}").TrimEnd();
            return rowClass;
        }

        public string BuildGridColumnClasses(string? columnSizeClass, string? fromDesktopClass, string? customClass, string? forBlockOfContentTypeAlias = null, bool defaultToFullWidth = false)
        {
            if (!string.IsNullOrEmpty(columnSizeClass)) { columnSizeClass = $"{GovUkClassNames.Column}-{columnSizeClass}"; }
            if (!string.IsNullOrEmpty(fromDesktopClass)) { fromDesktopClass = $"{GovUkClassNames.Column}-{fromDesktopClass}-from-desktop"; }
            var columnClass = (columnSizeClass + " " + fromDesktopClass).Trim();
            if (string.IsNullOrEmpty(columnClass)) { columnClass = DefaultColumnClass(forBlockOfContentTypeAlias, defaultToFullWidth); }
            columnClass = ($"{GovUkClassNames.Column} {columnClass} {customClass}").TrimEnd(); // .govuk-grid-column is not part of the GOV.UK design system but it's useful to be able to target any column
            return columnClass;
        }

        private string DefaultColumnClass(string? forBlockOfContentTypeAlias, bool defaultToFullWidth)
        {
            if (defaultToFullWidth) { return GovUkClassNames.ColumnFullWidth; }

            if (forBlockOfContentTypeAlias is not null && _columnClassProviders is not null)
            {
                foreach (var provider in _columnClassProviders)
                {
                    if (provider.IsProvider(forBlockOfContentTypeAlias))
                    {
                        return provider.ColumnClasses;
                    }
                }
            }

            return GovUkClassNames.ColumnTwoThirdsFromDesktop;
        }
    }
}
