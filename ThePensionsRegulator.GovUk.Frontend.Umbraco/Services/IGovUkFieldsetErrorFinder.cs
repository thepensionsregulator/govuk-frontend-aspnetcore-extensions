using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public interface IGovUkFieldsetErrorFinder
    {
        IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindErrors(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> fieldsetBlock, ModelStateDictionary modelState);
    }
}