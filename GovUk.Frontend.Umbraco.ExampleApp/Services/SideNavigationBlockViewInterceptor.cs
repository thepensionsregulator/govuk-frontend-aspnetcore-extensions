using GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Core;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class SideNavigationBlockViewInterceptor(IUmbracoPublishedContentAccessor _publishedContentAccessor) : IBlockViewInterceptor
    {
        public void InterceptBlockView(BlockViewModel blockViewModel)
        {
            if (_publishedContentAccessor.PublishedContent.ContentType.Alias == "sideNavigation")
            {
                blockViewModel.OpenWidthContainer = false;
                blockViewModel.CloseWidthContainer = false;
            }
        }
    }
}
