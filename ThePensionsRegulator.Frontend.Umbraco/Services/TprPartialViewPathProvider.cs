using GovUk.Frontend.Umbraco.Services;
using System;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    /// <inheritdoc/>
    public class TprPartialViewPathProvider : IPartialViewPathProvider
    {
        /// <inheritdoc/>
        public bool IsProvider(IBlockReference<IPublishedElement, IPublishedElement> block) => block?.Content?.ContentType?.Alias?.StartsWith("TPR", StringComparison.OrdinalIgnoreCase) ?? false;
        /// <inheritdoc/>
        public string BuildPartialViewPath(IBlockReference<IPublishedElement, IPublishedElement> block) => "TPR/" + block?.Content?.ContentType?.Alias;

    }
}
