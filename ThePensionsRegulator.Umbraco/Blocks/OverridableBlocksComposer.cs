using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public class OverridableBlocksComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.PropertyValueConverters().Remove<BlockListPropertyValueConverter>();
            builder.PropertyValueConverters().Remove<BlockGridPropertyValueConverter>();
        }
    }
}
