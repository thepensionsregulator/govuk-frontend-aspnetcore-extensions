using Moq;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Core = Umbraco.Cms.Core;

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
            return CreateBlockGridModel(new[] { blockGridItem });
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridModel"/> containing multiple instances of <see cref="BlockGridItem"/>.
        /// </summary>
        public static BlockGridModel CreateBlockGridModel(IEnumerable<BlockGridItem> blockGridItems, int? gridColumns = null)
        {
            return new BlockGridModel(blockGridItems.ToList(), gridColumns);
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridModel"/> containing a single <see cref="BlockGridItem"/> which can have its property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridModel CreateOverridableBlockGridModel(BlockGridItem blockGridItem)
        {
            return CreateOverridableBlockGridModel(new[] { blockGridItem });
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockGridModel"/> containing multiple instances of <see cref="BlockGridItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockGridModel CreateOverridableBlockGridModel(IEnumerable<BlockGridItem> blockGridItems)
        {
            return new OverridableBlockGridModel(blockGridItems, null, OverridableBlockGridItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridItem"/> with the specified content, and no settings.
        /// </summary>
        public static BlockGridItem CreateBlock(IPublishedElement content)
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return new BlockGridItem(Core.Udi.Create(Core.Constants.UdiEntityType.Element, Guid.NewGuid()), content, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockGridItem"/> with the specified content and settings.
        /// </summary>
        public static BlockGridItem CreateBlock(IPublishedElement content, IPublishedElement settings)
        {
            return new BlockGridItem(Core.Udi.Create(Core.Constants.UdiEntityType.Element, Guid.NewGuid()), content,
                                     Core.Udi.Create(Core.Constants.UdiEntityType.Element, Guid.NewGuid()), settings);
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
