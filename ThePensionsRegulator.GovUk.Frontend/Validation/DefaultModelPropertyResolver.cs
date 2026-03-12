using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    /// <summary>
    /// Resolves the default ASP.NET model for the page as the model to resolve properties from.
    /// </summary>
    public class DefaultModelPropertyResolver : ModelPropertyResolverBase
    {
        public override int Order => 100;

        public override Type? ResolveModelType(ViewContext viewContext) => viewContext.ViewData?.ModelMetadata?.ModelType;
    }
}
