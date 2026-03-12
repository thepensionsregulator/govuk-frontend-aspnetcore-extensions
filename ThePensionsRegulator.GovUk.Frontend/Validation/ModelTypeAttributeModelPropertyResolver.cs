using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    /// <summary>
    /// Resolves the model for the page based on a <see cref="ModelTypeAttribute"/> added to the action method.
    /// </summary>
    public class ModelTypeAttributeModelPropertyResolver : ModelPropertyResolverBase
    {
        public override int Order => 50;

        /// <inheritdoc/>
        public override Type? ResolveModelType(ViewContext viewContext)
        {
            var actionMethod = (viewContext.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
            return (actionMethod?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
        }
    }
}
