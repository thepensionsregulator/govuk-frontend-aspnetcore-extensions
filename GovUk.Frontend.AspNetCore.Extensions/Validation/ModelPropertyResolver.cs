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
            var modelProperty = IterateOverProperties(modelType, modelPropertyName);
            if (modelProperty == null) { throw new InvalidOperationException($"To support client-side validation add a property named {modelPropertyName} to type {modelType!.FullName}, or decorate your controller action with {nameof(ModelTypeAttribute)} to specify a different model type."); }

            return modelProperty;
        }

        private PropertyInfo? IterateOverProperties(Type modelType, string modelPropertyName, string parentPropertyName = "")
        {
            PropertyInfo? modelProperty = null;

            if (!string.IsNullOrWhiteSpace(modelPropertyName))
            {
                modelProperty = modelType?.GetProperty(modelPropertyName);
                if (modelProperty == null)
                {
                    foreach (var property in modelType!.GetProperties())
                    {
                        if (property.PropertyType == modelType) break; // Prevent Stack overflow

                        // check for IList or Array type fields.
                        var splitFront = modelPropertyName.Split("[")[0]; // Grab the first part
                        if (property.Name == splitFront)
                        {
                            var splitBack = modelPropertyName.Split("].", 2)[1]; // Grab the second half

                            // The 'T' in IList<T> comes from GenericTypeArguments[]. Only dealing with one for now.
                            var resList = IterateOverProperties(property.PropertyType.GenericTypeArguments[0], splitBack);
                            if (resList != null)
                            {
                                modelProperty = resList;
                                break;
                            }
                        }

                        // Would this property match modelPropertyName?
                        var pp = parentPropertyName == "" ? property.Name : parentPropertyName + ("." + property.Name); 
                        if (pp == modelPropertyName) return property;

                        // If no, iterate down.
                        var res = IterateOverProperties(property.PropertyType, modelPropertyName, pp);
                        if (res != null)
                        {
                            modelProperty = res;
                            break;
                        }                       
                    }
                }
            }

            return modelProperty;

        }
    }
}
