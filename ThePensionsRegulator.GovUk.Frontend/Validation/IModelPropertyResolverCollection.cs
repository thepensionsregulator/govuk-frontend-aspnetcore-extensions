using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    public interface IModelPropertyResolverCollection : ICollection<ModelPropertyResolverBase>
    {
        /// <summary>
        /// Resolves the model type for the current request, and the property info for the specified property name on the model.
        /// </summary>
        /// <param name="viewContext">The current view context.</param>
        /// <param name="modelPropertyName">The name of the property to resolve.</param>
        /// <returns>The resolved model property.</returns>
        PropertyInfo ResolveModelProperty(ViewContext viewContext, string modelPropertyName);
    }
}