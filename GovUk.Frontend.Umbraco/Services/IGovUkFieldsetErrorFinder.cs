using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IGovUkFieldsetErrorFinder
    {
        IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindErrors(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> fieldsetBlock, ModelStateDictionary modelState);
    }
}