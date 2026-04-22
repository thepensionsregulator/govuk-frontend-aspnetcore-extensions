using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    /// <summary>
    /// A collection of <see cref="IModelPropertyResolver"/> instances that are used to resolve the model type and property info for a specific property on the model for the current request.
    /// </summary>
    public class ModelPropertyResolverCollection : Collection<ModelPropertyResolverBase>, IModelPropertyResolverCollection
    {
        public ModelPropertyResolverCollection(IEnumerable<ModelPropertyResolverBase> _resolvers) : base()
        {
            foreach (var resolver in _resolvers) { this.Add(resolver); }
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Thrown if the model type could not be resolved, or the property could not be resolved on the model type.</exception>
        public PropertyInfo ResolveModelProperty(ViewContext viewContext, string modelPropertyName)
        {
            Type? modelType = null;
            ModelPropertyResolverBase? modelPropertyResolver = null;
            foreach (var resolver in this.OrderBy(r => r.Order))
            {
                modelType = resolver.ResolveModelType(viewContext);
                if (modelType is not null)
                {
                    modelPropertyResolver = resolver;
                    break;
                }
            }

            if (modelType is null || modelPropertyResolver is null)
            {
                throw new InvalidOperationException($"No {nameof(ModelPropertyResolverBase)} was able to detect the model type for the page.");
            }

            var modelProperty = modelPropertyResolver.ResolveModelProperty(modelType, modelPropertyName);
            if (modelProperty is null)
            {
                throw new InvalidOperationException($"{modelPropertyName} property was not found on resolved model type {modelType.FullName}.");
            }

            return modelProperty;
        }
    }
}
