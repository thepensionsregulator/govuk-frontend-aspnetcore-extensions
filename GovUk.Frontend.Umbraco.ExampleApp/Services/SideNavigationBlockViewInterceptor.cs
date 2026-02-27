using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Umbraco.Core;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class SideNavigationBlockViewInterceptor(IUmbracoPublishedContentAccessor _publishedContentAccessor, IOptions<AppConfig> _config) : IBlockViewInterceptor
    {
        public void InterceptBlockView(BlockViewModel blockViewModel)
        {
            if (_publishedContentAccessor.PublishedContent.ContentType.Alias == "sideNavigation" && _config.Value.TPRStyles)
            {
                blockViewModel.OpenWidthContainer = false;
                blockViewModel.CloseWidthContainer = false;
            }
        }
    }
}
