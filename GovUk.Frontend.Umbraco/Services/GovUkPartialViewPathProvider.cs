using System;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    /// <inheritdoc/>
    public class GovUkPartialViewPathProvider : IPartialViewPathProvider
    {
        /// <inheritdoc/>
        public bool IsProvider(IBlockReference<IPublishedElement, IPublishedElement> block) => block?.Content?.ContentType?.Alias?.StartsWith("GOVUK", StringComparison.OrdinalIgnoreCase) ?? false;
        /// <inheritdoc/>
        public string BuildPartialViewPath(IBlockReference<IPublishedElement, IPublishedElement> block) => "GOVUK/" + block?.Content?.ContentType?.Alias;

    }
}
