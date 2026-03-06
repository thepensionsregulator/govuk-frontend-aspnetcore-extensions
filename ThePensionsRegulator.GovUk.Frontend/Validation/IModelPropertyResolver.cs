using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    public interface IModelPropertyResolver
    {
        /// <summary>
        /// Resolves the view model type for the current page
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        Type ResolveModelType(ViewContext viewContext);

        /// <summary>
        /// Resolves a property name to a <see cref="PropertyInfo"/> instance on specified type
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="modelPropertyName"></param>
        /// <returns></returns>
        PropertyInfo ResolveModelProperty(Type modelType, string modelPropertyName);
    }
}