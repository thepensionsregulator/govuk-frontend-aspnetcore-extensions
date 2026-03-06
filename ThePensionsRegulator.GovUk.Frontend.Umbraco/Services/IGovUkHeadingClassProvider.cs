using ThePensionsRegulator.GovUk.Frontend.Typography;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public interface IGovUkHeadingClassProvider
    {
        HeadingClasses HeadingClasses(IPublishedContent page);
    }
}