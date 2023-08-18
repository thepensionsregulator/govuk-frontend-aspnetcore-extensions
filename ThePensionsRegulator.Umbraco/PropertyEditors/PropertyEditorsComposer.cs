using ThePensionsRegulator.Umbraco.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace ThePensionsRegulator.Umbraco.PropertyEditors
{
    /// <summary>
    /// Remove the default property value converter for rich text editors so that <see cref="RichTextEditorPropertyValueConverter"/> is used instead.
    /// </summary>
    public class PropertyEditorsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.PropertyValueConverters().Remove<RteMacroRenderingValueConverter>();
        }
    }
}
