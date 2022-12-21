using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IUmbracoPublishedContentAccessor
    {
        /// <summary>
        /// The published content node matching the current request
        /// </summary>
        IPublishedContent PublishedContent { get; }

        /// <summary>
        /// Recursively find the first block in the published content that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        BlockListItem? FindBlockByBoundProperty(string propertyName);

        /// <summary>
        /// Recursively find the matching blocks that matches the predicate in all block lists
        /// </summary>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        IEnumerable<BlockListItem> FindBlocks(Func<BlockListItem, bool> matcher);

        /// <summary>
        /// Recursively find the first block in the published content that matches the predicate
        /// </summary>
        /// <param name="matcher"></param>
        /// <returns></returns>
        BlockListItem? FindBlock(Func<BlockListItem, bool> matcher);
    }
}
