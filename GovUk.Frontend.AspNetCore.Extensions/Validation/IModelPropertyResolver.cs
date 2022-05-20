using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public interface IModelPropertyResolver
    {
        /// <summary>
        /// Resolves a property name to a <see cref="PropertyInfo"/> instance on the view model for the current page
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="modelPropertyName"></param>
        /// <returns></returns>
        PropertyInfo ResolveModelProperty(ViewContext viewContext, string modelPropertyName);
    }
}