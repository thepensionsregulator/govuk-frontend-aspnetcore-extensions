using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.Extensions.Options;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprBoxViewInterceptor : IBlockViewInterceptor
    {
        private readonly GovUkFrontendUmbracoOptions _options;

        public TprBoxViewInterceptor(IOptions<GovUkFrontendUmbracoOptions> options)
        {
            _options = options.Value;
        }

        public void InterceptBlockView(BlockViewModel blockViewModel)
        {
            if (!_options.RenderWidthContainerForBlocks) { return; }

            var isFullWidthBox = blockViewModel.Block.Content.ContentType.Alias == ElementTypeAliases.TprBox &&
                blockViewModel.Block.Settings.Value<string>(PropertyAliases.TprBoxStyle) == TprBoxStyles.FullWidth;

            if (isFullWidthBox)
            {
                blockViewModel.RenderWidthContainer = false;
            }
        }
    }
}
