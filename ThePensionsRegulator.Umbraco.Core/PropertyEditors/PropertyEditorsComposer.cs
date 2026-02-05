using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace ThePensionsRegulator.Umbraco.Core.PropertyEditors
{
    /// <summary>
    /// Remove previously-registered property value converters so that replacements in this project are used instead.
    /// </summary>
    public class PropertyEditorsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.PropertyValueConverters().Remove<RteBlockRenderingValueConverter>();
            builder.PropertyValueConverters().Remove<MultiUrlPickerValueConverter>();
        }
    }
}
