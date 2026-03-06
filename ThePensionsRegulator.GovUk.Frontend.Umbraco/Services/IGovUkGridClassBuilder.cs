namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public interface IGovUkGridClassBuilder
    {
        string BuildGridColumnClasses(string? columnSizeClass, string? fromDesktopClass, string? customClass, string? forBlockOfContentTypeAlias = null, bool defaultToFullWidth = false);
        string BuildGridRowClasses(string? customClass);
    }
}