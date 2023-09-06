using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    /// <summary>
    /// Provides a relative or absolute path to a partial view based on the properties of an object.
    /// </summary>
    public interface IPartialViewPathProvider
    {
        /// <summary>
        /// Gets a value indicating whether the provider can provide a partial view path for a block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <returns>A value indicating whether the provider can provide a partial view path for a block.</returns>
        bool IsProvider(IBlockReference<IPublishedElement, IPublishedElement> block);

        /// <summary>
        /// Builds a partial view path based on the properties of the block. Check that <see cref="IsProvider(IBlockReference{IPublishedElement, IPublishedElement})" /> returns <c>true</c> for the block before calling this method.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <returns>A relative or absolute partial view path.</returns>
        string BuildPartialViewPath(IBlockReference<IPublishedElement, IPublishedElement> block);
    }
}
