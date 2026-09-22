using Microsoft.Extensions.DependencyInjection;
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
            // Request-scoped because block values are long-term cached by Umbraco at published element level, but overrides must persist only for one whole request.
            builder.Services.AddScoped<IOverridablePublishedElementValueStore, OverridablePublishedElementValueStore>();
            // Singleton because it's captured by OverridableBlockxxxModel which is captured by a singleton property value converter.
            builder.Services.AddSingleton<IOverridablePublishedElementFactory, DefaultOverridablePublishedElementFactory>();
            builder.PropertyValueConverters().Remove<BlockListPropertyValueConverter>();
            builder.PropertyValueConverters().Remove<BlockGridPropertyValueConverter>();
        }
    }
}
