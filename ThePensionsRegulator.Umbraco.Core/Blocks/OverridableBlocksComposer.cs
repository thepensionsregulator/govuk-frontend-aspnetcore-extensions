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
            builder.Services.AddScoped<IOverridablePublishedElementFactory, DefaultOverridablePublishedElementFactory>();
            // Singleton accessor so singleton services (eg property value converters) can resolve the request-scoped factory.
            builder.Services.AddSingleton<IOverridablePublishedElementFactoryAccessor, DefaultOverridablePublishedElementFactoryAccessor>();
            builder.PropertyValueConverters().Remove<BlockListPropertyValueConverter>();
            builder.PropertyValueConverters().Remove<BlockGridPropertyValueConverter>();
        }
    }
}
