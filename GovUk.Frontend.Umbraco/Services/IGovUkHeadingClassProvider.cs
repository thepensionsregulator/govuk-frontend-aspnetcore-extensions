using GovUk.Frontend.AspNetCore.Extensions.Typography;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Services
{
    public interface IGovUkHeadingClassProvider
    {
        HeadingClasses HeadingClasses(IPublishedContent page);
    }
}