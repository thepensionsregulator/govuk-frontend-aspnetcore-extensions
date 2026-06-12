using Moq;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// Create block list models and block list items
    /// </summary>
    public class UmbracoBlockListFactory
    {
        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockListModel"/> containing a single <see cref="BlockListItem"/>.
        /// </summary>
        public static BlockListModel CreateBlockListModel(BlockListItem blockListItem)
        {
            return CreateBlockListModel(new[] { blockListItem });
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockListModel"/> containing multiple instances of <see cref="BlockListItem"/>.
        /// </summary>
        public static BlockListModel CreateBlockListModel(IEnumerable<BlockListItem> blockListItems)
        {
            return new BlockListModel(blockListItems.ToList());
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListModel"/> containing a single <see cref="BlockListItem"/> which can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListModel CreateOverridableBlockListModel(IPublishedValueFallback publishedValueFallback, BlockListItem blockListItem)
        {
            return CreateOverridableBlockListModel(publishedValueFallback, new[] { blockListItem });
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListModel"/> containing a single <see cref="BlockListItem"/> which can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListModel CreateOverridableBlockListModel(BlockListItem blockListItem)
            => CreateOverridableBlockListModel(Mock.Of<IPublishedValueFallback>(), blockListItem);

        /// <summary>
        /// Create an <see cref="OverridableBlockListModel"/> containing multiple instances of <see cref="BlockListItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListModel CreateOverridableBlockListModel(IPublishedValueFallback publishedValueFallback, IEnumerable<BlockListItem> blockListItems)
        {
            return new OverridableBlockListModel(publishedValueFallback, blockListItems, null, OverridableBlockListItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListModel"/> containing multiple instances of <see cref="BlockListItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListModel CreateOverridableBlockListModel(IEnumerable<BlockListItem> blockListItems)
            => CreateOverridableBlockListModel(Mock.Of<IPublishedValueFallback>(), blockListItems);

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockListItem"/> with the specified content, and no settings.
        /// </summary>
        public static BlockListItem CreateBlock(IPublishedElement content)
        {
            return new BlockListItem(Guid.NewGuid(), content, null, null);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockListItem"/> with the specified content and settings.
        /// </summary>
        public static BlockListItem CreateBlock(IPublishedElement content, IPublishedElement settings)
        {
            return new BlockListItem(Guid.NewGuid(), content, Guid.NewGuid(), settings);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListItem"/> with the specified content, and no settings. The content can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListItem CreateOverridableBlock(IPublishedElement content)
        {
            return new OverridableBlockListItem(CreateBlock(content), OverridableBlockListItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListItem"/> with the specified content and settings. Both content and settings can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListItem CreateOverridableBlock(IPublishedElement content, IPublishedElement settings)
        {
            return new OverridableBlockListItem(CreateBlock(content, settings), OverridableBlockListItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListItem"/> with mocked content based on the specified content type alias, and no settings. The content can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListItem CreateOverridableBlock(string contentTypeAliasForContent)
        {
            return new OverridableBlockListItem(CreateBlock(CreateContentOrSettings(contentTypeAliasForContent).Object), OverridableBlockListItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListItem"/> with mocked content and settings based on the specified content type aliases. Both content and settings can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListItem CreateOverridableBlock(string contentTypeAliasForContent, string contentTypeAliasForSettings)
        {
            var content = CreateContentOrSettings(contentTypeAliasForContent);
            var settings = CreateContentOrSettings(contentTypeAliasForSettings);
            return CreateOverridableBlock(content.Object, settings.Object);
        }

        /// <summary>
        /// Mock an <see cref="IOverridablePublishedElement"/> suitable for use as either content or settings when using <see cref="CreateBlock"/> or <see cref="CreateOverridableBlock"/>.
        /// </summary>
        /// <param name="contentTypeAlias"></param>
        /// <returns></returns>
        public static Mock<IOverridablePublishedElement> CreateContentOrSettings(string? contentTypeAlias = null)
        {
            return UmbracoContentFactory.CreateContent<IOverridablePublishedElement>(contentTypeAlias);
        }
    }
}
