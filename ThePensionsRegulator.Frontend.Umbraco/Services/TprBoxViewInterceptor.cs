using Microsoft.Extensions.Options;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprBoxViewInterceptor : IBlockViewInterceptor
    {
        private readonly GovUkFrontendUmbracoOptions _options;
        private readonly IPublishedValueFallback _publishedValueFallback;

        public TprBoxViewInterceptor(IOptions<GovUkFrontendUmbracoOptions> options, IPublishedValueFallback publishedValueFallback)
        {
            _options = options.Value;
            _publishedValueFallback = publishedValueFallback;
        }

        public void InterceptBlockView(BlockViewModel blockViewModel)
        {
            if (!_options.RenderWidthContainerForBlocks) { return; }

            bool currentBlockIsFullWidthBox = BlockIsFullWidthBox(blockViewModel.CurrentBlock, _publishedValueFallback);

            if (currentBlockIsFullWidthBox)
            {
                blockViewModel.OpenWidthContainer = false;
                blockViewModel.CloseWidthContainer = false;
            }

            bool? previousBlockIsFullWidthBox = blockViewModel.PreviousBlock is not null ? BlockIsFullWidthBox(blockViewModel.PreviousBlock, _publishedValueFallback) : null;
            if ((previousBlockIsFullWidthBox ?? false) && !currentBlockIsFullWidthBox)
            {
                blockViewModel.OpenWidthContainer = true;
            }

            bool? nextBlockIsFullWidthBox = blockViewModel.NextBlock is not null ? BlockIsFullWidthBox(blockViewModel.NextBlock, _publishedValueFallback) : null;
            if ((nextBlockIsFullWidthBox ?? false) && !currentBlockIsFullWidthBox)
            {
                blockViewModel.CloseWidthContainer = true;
            }
        }

        private static bool BlockIsFullWidthBox(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block, IPublishedValueFallback publishedValueFallback)
        {
            return block.Content.ContentType.Alias == TprElementTypeAliases.Box &&
                   block.Settings?.Value<string>(publishedValueFallback, TprPropertyAliases.BoxStyle) == TprBoxStyles.FullWidth;
        }
    }
}
