using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Reflection;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    public abstract class ModelPropertyResolverBase
    {
        private const int maxDepth = 5;

        /// <summary>
        /// The order in which this resolver should be executed relative to other resolvers. Resolvers with a lower order will be executed first.
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// Resolves the view model type for the current page
        /// </summary>
        /// <param name="viewContext">The view context for the current request.</param>
        /// <returns>The type of the model, or <c>null</c> if the model type could not be resolved.</returns>
        public abstract Type? ResolveModelType(ViewContext viewContext);

        /// <summary>
        /// Resolves a property name to a <see cref="PropertyInfo"/> instance on specified type
        /// </summary>
        /// <param name="modelType">The type of the model expected to contain the property.</param>
        /// <param name="modelPropertyName">The name of the property to resolve.</param>
        /// <returns>A <see cref="PropertyInfo"/> instance representing the resolved property, or null if not found.</returns>
        public virtual PropertyInfo? ResolveModelProperty(Type modelType, string modelPropertyName) => IterateOverProperties(modelType, modelPropertyName);

        private PropertyInfo? IterateOverProperties(Type modelType, string modelPropertyName, string parentPropertyName = "", int depth = 0)
        {
            PropertyInfo? modelProperty = null;
            depth++;

            if (depth >= maxDepth) return null; //don't go any further

            if (!string.IsNullOrWhiteSpace(modelPropertyName))
            {
                modelProperty = modelType?.GetProperty(modelPropertyName);
                if (modelProperty == null)
                {
                    foreach (var property in modelType!.GetProperties())
                    {
                        if (property.PropertyType == modelType) break; // Prevent Stack overflow

                        // Only do this next bit for Enumerable type. Have to exclude string
                        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(String))
                        {
                            if (modelPropertyName.Contains("["))
                            {
                                // check for IList or Array type fields.
                                var splitFront = modelPropertyName.Split("[")[0]; // Grab the first part
                                if (property.Name == splitFront)
                                {
                                    // Property is IEnum<T>
                                    if (property.PropertyType.GenericTypeArguments.Any())
                                    {
                                        var splitBack = modelPropertyName.Split("].", 2)[1]; // Grab the second half

                                        // The 'T' in IList<T> comes from GenericTypeArguments[]. Only dealing with one for now.
                                        var resList = IterateOverProperties(property.PropertyType.GenericTypeArguments[0], splitBack, string.Empty, depth);
                                        if (resList != null)
                                        {
                                            modelProperty = resList;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        // Property is object[]
                                        return property;
                                    }
                                }
                            }
                        }

                        // Would this property match modelPropertyName?
                        var pp = parentPropertyName == "" ? property.Name : parentPropertyName + ("." + property.Name);
                        if (pp == modelPropertyName) return property;

                        // If no, iterate down.
                        var res = IterateOverProperties(property.PropertyType, modelPropertyName, pp, depth);
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