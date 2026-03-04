using ThePensionsRegulator.GovUk.Frontend.Typography;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco
{
    /// <summary>
    /// Gets settings from well-known GOV.UK properties on an Umbraco content node.
    /// </summary>
    [Obsolete("Use IGovUkHeadingClassProvider")]
    public static class GovUkPageSetting
    {
        /// <summary>
        /// Gets the HTML classes which should be applied to each HTML heading level.
        /// </summary>
        /// <param name="page">The current Umbraco page.</param>
        /// <returns>HTML classes.</returns>
        public static HeadingClasses HeadingClasses(IPublishedContent page) => GovUkTypography.HeadingClasses(page.Value<string>(PropertyAliases.PageSettingHeadingScaleStart, fallback: Fallback.ToAncestors));
    }
}
