using Moq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.BlockLists;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Core = Umbraco.Cms.Core;

namespace GovUk.Frontend.Umbraco.Testing
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
        public static OverridableBlockListModel CreateOverridableBlockListModel(BlockListItem blockListItem)
        {
            return CreateOverridableBlockListModel(new[] { blockListItem });
        }

        /// <summary>
        /// Create an <see cref="OverridableBlockListModel"/> containing multiple instances of <see cref="BlockListItem"/> which can have their property values overridden at runtime.
        /// </summary>
        public static OverridableBlockListModel CreateOverridableBlockListModel(IEnumerable<BlockListItem> blockListItems)
        {
            return new OverridableBlockListModel(blockListItems, null, OverridableBlockListItem.NoopPublishedElementFactory);
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockListItem"/> with the specified content, and no settings.
        /// </summary>
        public static BlockListItem CreateBlock(IPublishedElement content)
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return new BlockListItem(Core.Udi.Create(Core.Constants.UdiEntityType.Element, Guid.NewGuid()), content, null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        /// <summary>
        /// Create a read-only Umbraco <see cref="BlockListItem"/> with the specified content and settings.
        /// </summary>
        public static BlockListItem CreateBlock(IPublishedElement content, IPublishedElement settings)
        {
            return new BlockListItem(Core.Udi.Create(Core.Constants.UdiEntityType.Element, Guid.NewGuid()), content,
                                     Core.Udi.Create(Core.Constants.UdiEntityType.Element, Guid.NewGuid()), settings);
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
