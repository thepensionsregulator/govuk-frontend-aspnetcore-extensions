using GovUk.Frontend.AspNetCore.Extensions.Models;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPaginationFactory
    {
        PaginationModel CreateFromPaginationBlock(BlockListItem block);
    }
}