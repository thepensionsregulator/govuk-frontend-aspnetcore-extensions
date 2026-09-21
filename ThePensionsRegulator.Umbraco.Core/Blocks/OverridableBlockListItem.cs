using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    public class OverridableBlockListItem : BlockListItem, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>
    {
        /// <summary>
        /// Creates block list items for compatibility scenarios where no <see cref="IOverridablePublishedElementFactory"/> is supplied.
        /// </summary>
        [Obsolete($"Use a factory which accepts an {nameof(IOverridablePublishedElementFactory)} or {nameof(Func<IPublishedElement?, IOverridablePublishedElement?>)} to support overriding property values.")]
        public static Func<IPublishedElement?, IOverridablePublishedElement?> DefaultPublishedElementFactory { get => publishedElement => publishedElement != null ? new OverridablePublishedElement(publishedElement) : null; }

        /// <summary>
        /// Leaves published elements unchanged. This is useful when the supplied elements already implement <see cref="IOverridablePublishedElement"/>, such as in unit tests.
        /// </summary>
        public static Func<IPublishedElement?, IOverridablePublishedElement?> NoopPublishedElementFactory { get => publishedElement => (IOverridablePublishedElement?)publishedElement; }

        [Obsolete($"Use a factory which accepts an {nameof(IOverridablePublishedElementFactory)} or {nameof(Func<IPublishedElement?, IOverridablePublishedElement?>)} to support overriding property values.")]
        public OverridableBlockListItem(BlockListItem item) : this(item, DefaultPublishedElementFactory) { }

        /// <summary>
        /// Creates an <see cref="OverridableBlockListItem"/> from a read-only <see cref="BlockListItem"/>.
        /// </summary>
        /// <param name="item">The block list item to make overridable.</param>
        /// <param name="publishedElementFactory">A factory to wrap published elements (content and settings) to support overriding property values. The factory must use a request-scoped value store.</param>
        public OverridableBlockListItem(BlockListItem item, IOverridablePublishedElementFactory publishedElementFactory)
            : this(item, publishedElementFactory.Create)
        {
        }

        /// <summary>
        /// Creates an <see cref="OverridableBlockListItem"/> from a read-only <see cref="BlockListItem"/>.
        /// </summary>
        /// <param name="item">The block list item to make overridable.</param>
        /// <param name="publishedElementFactory">A factory to wrap published elements (content and settings) to support overriding property values. The factory must use a request-scoped value store.</param>
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
