using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.ObjectModel;
using System.Reflection;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests.Validation
{
    internal class FakeModelPropertyResolverCollection(Func<ViewContext, string, PropertyInfo> _resolveModelProperty)
        : Collection<ModelPropertyResolverBase>, IModelPropertyResolverCollection
    {
        public PropertyInfo ResolveModelProperty(ViewContext viewContext, string modelPropertyName) => _resolveModelProperty(viewContext, modelPropertyName);
    }
}
