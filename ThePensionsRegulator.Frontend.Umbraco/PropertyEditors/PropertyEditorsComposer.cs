using GovUk.Frontend.Umbraco.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors
{
    /// <summary>
    /// Remove previously-registered property value converters so that replacements in this project are used instead.
    /// </summary>
    public class PropertyEditorsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.PropertyValueConverters().Remove<GovUkRichTextEditorPropertyValueConverter>();
        }
    }
}
