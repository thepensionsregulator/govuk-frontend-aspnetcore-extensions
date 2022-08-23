using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Models
{
    public static class PublishedRequestExtensions
    {
        public static BlockListItem? FindBlock(this IPublishedRequest request, Func<BlockListItem, bool> matcher)
        {
            var blockLists = request.GetBlockListModels();

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var block = blockList.RecursivelyFindBlock(matcher);
                    if (block != null)
                    {
                        return block;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Recursively find the first block in a request that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="request"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BlockListItem? FindBlock(this IPublishedRequest request, string propertyName)
        {
            var blockLists = request.GetBlockListModels();

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var block = blockList.FindBlockByBoundProperty(propertyName);
                    if (block != null)
                    {
                        return block;
                    }
                }
            }
            return null;
        }

        private static IEnumerable<BlockListModel?> GetBlockListModels(this IPublishedRequest request)
        {
            if (request?.PublishedContent?.Properties is null)
            {
                throw new ArgumentNullException(nameof(request.PublishedContent.Properties));
            }

            return request.PublishedContent.Properties
                .Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && x.HasValue())
                .Select(x => x.Value<BlockListModel>(null));
        }
    }
}
