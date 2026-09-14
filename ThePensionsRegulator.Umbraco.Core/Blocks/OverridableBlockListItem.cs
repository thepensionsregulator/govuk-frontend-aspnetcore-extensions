using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    public class OverridableBlockListItem : BlockListItem, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>
    {
        /// <summary>
        /// Creates block list items suitable for non-HTTP usage. For HTTP request handling, supply a factory which passes <see cref="IHttpContextAccessor" /> to <see cref="OverridablePublishedElement"/>.
        /// </summary>
        public static Func<IPublishedElement?, IOverridablePublishedElement?> DefaultPublishedElementFactory { get => publishedElement => publishedElement != null ? new OverridablePublishedElement(publishedElement) : null; }

        /// <summary>
        /// Leaves published elements unchanged. This is useful when the supplied elements already implement <see cref="IOverridablePublishedElement"/>, such as in unit tests.
        /// </summary>
        public static Func<IPublishedElement?, IOverridablePublishedElement?> NoopPublishedElementFactory { get => publishedElement => (IOverridablePublishedElement?)publishedElement; }

        public OverridableBlockListItem(BlockListItem item) : this(item, DefaultPublishedElementFactory) { }

        public OverridableBlockListItem(BlockListItem item, Func<IPublishedElement?, IOverridablePublishedElement?> publishedElementFactory) :
#nullable disable
            base(item.ContentKey, publishedElementFactory(item.Content), item.SettingsKey, publishedElementFactory(item.Settings))
#nullable enable
        {

        }

        public new IOverridablePublishedElement Content { get => (IOverridablePublishedElement)base.Content; }

        public new IOverridablePublishedElement? Settings { get => (IOverridablePublishedElement?)base.Settings; }
    }
}
