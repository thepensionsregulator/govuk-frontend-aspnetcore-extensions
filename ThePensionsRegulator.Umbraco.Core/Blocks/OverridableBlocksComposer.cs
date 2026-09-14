using Microsoft.Extensions.DependencyInjection;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace ThePensionsRegulator.Umbraco.Core.Blocks
{
    public class OverridableBlocksComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            // Block values are cached by Umbraco at element level, but overrides belong to one request only.
            builder.Services.AddScoped<IOverridablePublishedElementValueStore, OverridablePublishedElementValueStore>();
            builder.PropertyValueConverters().Remove<BlockListPropertyValueConverter>();
            builder.PropertyValueConverters().Remove<BlockGridPropertyValueConverter>();
        }
    }
}
