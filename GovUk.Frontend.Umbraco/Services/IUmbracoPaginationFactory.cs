using GovUk.Frontend.AspNetCore.Extensions.Models;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPaginationFactory
    {
        PaginationModel CreateFromPaginationBlock(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block);
    }
}