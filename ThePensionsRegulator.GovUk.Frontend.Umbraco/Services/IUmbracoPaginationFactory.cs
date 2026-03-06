using ThePensionsRegulator.GovUk.Frontend.Models;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPaginationFactory
    {
        PaginationModel CreateFromPaginationBlock(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block);
    }
}