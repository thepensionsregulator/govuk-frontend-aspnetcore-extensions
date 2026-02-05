using GovUk.Frontend.AspNetCore.Extensions.Models;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPaginationFactory
    {
        PaginationModel CreateFromPaginationBlock(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block);
    }
}