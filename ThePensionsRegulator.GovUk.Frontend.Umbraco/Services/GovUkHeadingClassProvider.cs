using ThePensionsRegulator.GovUk.Frontend.Typography;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class GovUkHeadingClassProvider : IGovUkHeadingClassProvider
    {
        public HeadingClasses HeadingClasses(IPublishedContent page) => GovUkTypography.HeadingClasses(page.Value<string>(PropertyAliases.PageSettingHeadingScaleStart, fallback: Fallback.ToAncestors));
    }
}
