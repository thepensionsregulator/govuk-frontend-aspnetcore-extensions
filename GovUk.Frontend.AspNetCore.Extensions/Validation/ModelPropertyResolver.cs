using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Reflection;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class ModelPropertyResolver : IModelPropertyResolver
    {
        /// <inheritdoc/>
        public Type ResolveModelType(ViewContext viewContext)
        {
            var actionMethod = (viewContext.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo;
            var modelType = (actionMethod?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
            if (modelType == null) { modelType = viewContext.ViewData?.ModelMetadata?.ModelType; }
            if (modelType == null) { throw new InvalidOperationException($"Unable to detect the model type for the page to support client-side validation. To specify the model type you can decorate {(actionMethod != null ? actionMethod.DeclaringType?.FullName + "." + actionMethod.Name : "your controller action")} with a {nameof(ModelTypeAttribute)} identifying the type of your view model."); }
            return modelType;
        }

        /// <inheritdoc/>
        public PropertyInfo ResolveModelProperty(Type modelType, string modelPropertyName)
        {
            var modelProperty = modelType?.GetProperty(modelPropertyName);
            if (modelProperty == null) { throw new InvalidOperationException($"To support client-side validation add a property named {modelPropertyName} to type {modelType!.FullName}, or decorate your controller action with {nameof(ModelTypeAttribute)} to specify a different model type."); }

            return modelProperty;
        }
    }
}
