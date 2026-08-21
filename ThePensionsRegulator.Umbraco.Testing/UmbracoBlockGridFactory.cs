using Moq;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// Create block grid models and block grid items
    /// </summary>
    public class UmbracoBlockGridFactory
    {
        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridModel"/> containing a single <see cref="BlockGridItem"/>.
        /// </summary>
        public static BlockGridModel CreateBlockGridModel(BlockGridItem blockGridItem)
        {
            return CreateBlockGridModel([blockGridItem]);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridModel"/> containing multiple instances of <see cref="BlockGridItem"/>.
        /// </summary>
        public static BlockGridModel CreateBlockGridModel(IEnumerable<BlockGridItem> blockGridItems, int? gridColumns = null)
        {
            return new BlockGridModel(blockGridItems.ToList(), gridColumns);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridArea"/> containing a single <see cref="BlockGridItem"/>.
        /// </summary>
        public static BlockGridArea CreateBlockGridArea(BlockGridItem blockGridItem, string alias, int rowSpan = 1, int columnSpan = 1)
        {
            return CreateBlockGridArea([blockGridItem], alias, rowSpan, columnSpan);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridArea"/> containing multiple instances of <see cref="BlockGridItem"/>.
        /// </summary>
        public static BlockGridArea CreateBlockGridArea(IEnumerable<BlockGridItem> blockGridItems, string alias, int rowSpan = 1, int columnSpan = 1)
        {
            return new BlockGridArea(blockGridItems.ToList(), alias, rowSpan, columnSpan);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridModel"/> containing a single <see cref="BlockGridItem"/> which can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridModel CreateOverridableBlockGridModel(IPublishedValueFallback publishedValueFallback, BlockGridItem blockGridItem)
        {
            return CreateOverridableBlockGridModel(publishedValueFallback, [blockGridItem]);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridModel"/> containing a single <see cref="BlockGridItem"/> which can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridModel CreateOverridableBlockGridModel(BlockGridItem blockGridItem)
        {
            return CreateOverridableBlockGridModel([blockGridItem]);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridModel"/> containing multiple instances of <see cref="BlockGridItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridModel CreateOverridableBlockGridModel(IPublishedValueFallback publishedValueFallback, IEnumerable<BlockGridItem> blockGridItems)
        {
            return new OverridableBlockGridModel(publishedValueFallback, blockGridItems, null, OverridableBlockGridItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridModel"/> containing multiple instances of <see cref="BlockGridItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridModel CreateOverridableBlockGridModel(IEnumerable<BlockGridItem> blockGridItems)
            => CreateOverridableBlockGridModel(Mock.Of<IPublishedValueFallback>(), blockGridItems);

        /// <summary>
        /// Create an <see cref="OverridableBlockGridArea"/> containing a single <see cref="BlockGridItem"/> which can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridArea CreateOverridableBlockGridArea(BlockGridItem blockGridItem, string alias, int rowSpan = 1, int columnSpan = 1)
        {
            return CreateOverridableBlockGridArea([blockGridItem], alias, rowSpan, columnSpan);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridArea"/> containing multiple instances of <see cref="BlockGridItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridArea CreateOverridableBlockGridArea(IEnumerable<BlockGridItem> blockGridItems, string alias, int rowSpan = 1, int columnSpan = 1)
        {
            return new OverridableBlockGridArea(blockGridItems, alias, rowSpan, columnSpan, OverridableBlockGridItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridItem"/> with the specified content, and no settings.
        /// </summary>
        public static BlockGridItem CreateBlock(IPublishedElement content)
        {
            return new BlockGridItem(Guid.NewGuid(), content, null, null);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridItem"/> with the specified content and settings.
        /// </summary>
        public static BlockGridItem CreateBlock(IPublishedElement content, IPublishedElement settings)
        {
            return new BlockGridItem(Guid.NewGuid(), content, Guid.NewGuid(), settings);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridItem"/> with the specified content, and no settings. The content can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridItem CreateOverridableBlock(IPublishedElement content)
        {
            return new OverridableBlockGridItem(CreateBlock(content), OverridableBlockGridItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridItem"/> with the specified content and settings. Both content and settings can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridItem CreateOverridableBlock(IPublishedElement content, IPublishedElement settings)
        {
            return new OverridableBlockGridItem(CreateBlock(content, settings), OverridableBlockGridItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridItem"/> with mocked content based on the specified content type alias, and no settings. The content can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridItem CreateOverridableBlock(string contentTypeAliasForContent)
        {
            return new OverridableBlockGridItem(CreateBlock(CreateContentOrSettings(contentTypeAliasForContent).Object), OverridableBlockGridItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridItem"/> with mocked content and settings based on the specified content type aliases. Both content and settings can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridItem CreateOverridableBlock(string contentTypeAliasForContent, string contentTypeAliasForSettings)
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
