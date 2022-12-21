using GovUk.Frontend.Umbraco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Services
{
    public class UmbracoPublishedContentAccessor : IUmbracoPublishedContentAccessor
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IUmbracoContext _umbracoContext;
        private readonly IPublishedContent _publishedContent;

        public UmbracoPublishedContentAccessor(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor;
            if (!_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                throw new InvalidOperationException("Unable to get Umbraco context");
            }

            if (umbracoContext.PublishedRequest?.PublishedContent == null)
            {
                throw new InvalidOperationException("Unable to get Umbraco published content");
            }
            _umbracoContext = umbracoContext;
            _publishedContent = _umbracoContext.PublishedRequest.PublishedContent;
        }

        /// <inheritdoc />
        public IPublishedContent PublishedContent => _publishedContent;


        /// <inheritdoc />
        public BlockListItem? FindBlock(Func<BlockListItem, bool> matcher)
        {
            return FindBlock(PublishedContent, matcher);
        }

        /// <inheritdoc />
        public IEnumerable<BlockListItem> FindBlocks(Func<BlockListItem, bool> matcher)
        {
            var result = new List<BlockListItem>();
            var blockLists = GetBlockListModels(PublishedContent);

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var blocks = blockList.FindBlocks(matcher);
                    if (blocks.Any())
                    {
                        result.AddRange(blocks);
                    }
                }
            }
            return result;
        }


        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"></exception>
        public BlockListItem? FindBlockByBoundProperty(string propertyName)
        {
            return FindBlockByBoundProperty(PublishedContent, propertyName);
        }

        private BlockListItem? FindBlock(IPublishedContent content, Func<BlockListItem, bool> matcher)
        {
            var blockLists = GetBlockListModels(content);

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var block = blockList.FindBlock(matcher);
                    if (block != null)
                    {
                        return block;
                    }
                }
            }
            return null;
        }
        private BlockListItem? FindBlockByBoundProperty(IPublishedContent content, string propertyName)
        {
            var blockLists = GetBlockListModels(content);

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

        private IEnumerable<BlockListModel?> GetBlockListModels(IPublishedContent content)
        {
            if (content?.Properties is null)
            {
                throw new ArgumentNullException(nameof(content.Properties));
            }

            return content.Properties
                .Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && x.HasValue())
                .Select(x => x.Value<BlockListModel>(null));
        }
    }
}
