using System;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPublishedContentAccessor
    {
        IPublishedContent PublishedContent { get; }
        BlockListItem? FindBlockByBoundProperty(string propertyName);
        BlockListItem? FindBlock(Func<BlockListItem, bool> matcher);
    }
}
