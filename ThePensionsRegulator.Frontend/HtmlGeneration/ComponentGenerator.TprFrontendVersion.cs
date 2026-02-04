namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal static readonly string TprFrontendVersion = typeof(ComponentGenerator).Assembly.GetName().Version?.ToString() ?? string.Empty;
    }
}
