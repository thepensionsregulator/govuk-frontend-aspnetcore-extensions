using GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Core;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public class SideNavigationBlockViewInterceptor(IUmbracoPublishedContentAccessor _publishedContentAccessor, IConfiguration _config) : IBlockViewInterceptor
    {
        public void InterceptBlockView(BlockViewModel blockViewModel)
        {
            var useTPRstyles = _config.GetValue<bool>("TPRStyles");
            if (_publishedContentAccessor.PublishedContent.ContentType.Alias == "sideNavigation" && useTPRstyles)
            {
                blockViewModel.OpenWidthContainer = false;
                blockViewModel.CloseWidthContainer = false;
            }
        }
    }
}
