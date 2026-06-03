using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

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

            bool currentBlockIsFullWidthBox = BlockIsFullWidthBox(blockViewModel.CurrentBlock);

            if (currentBlockIsFullWidthBox)
            {
                blockViewModel.OpenWidthContainer = false;
                blockViewModel.CloseWidthContainer = false;
            }

            bool? previousBlockIsFullWidthBox = blockViewModel.PreviousBlock is not null ? BlockIsFullWidthBox(blockViewModel.PreviousBlock) : null;
            if ((previousBlockIsFullWidthBox ?? false) && !currentBlockIsFullWidthBox)
            {
                blockViewModel.OpenWidthContainer = true;
            }

            bool? nextBlockIsFullWidthBox = blockViewModel.NextBlock is not null ? BlockIsFullWidthBox(blockViewModel.NextBlock) : null;
            if ((nextBlockIsFullWidthBox ?? false) && !currentBlockIsFullWidthBox)
            {
                blockViewModel.CloseWidthContainer = true;
            }
        }

        private static bool BlockIsFullWidthBox(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
        {
            return block.Content.ContentType.Alias == TprElementTypeAliases.Box &&
                   block.Settings.Value<string>(TprPropertyAliases.BoxStyle) == TprBoxStyles.FullWidth;
        }
    }
}
