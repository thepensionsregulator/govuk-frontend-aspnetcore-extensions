using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors
{
    /// <summary>
    /// A rich text property editor identical to the default Umbraco rich text property editor.
    /// </summary>
    /// <remarks>This exists as a hook to apply different <see cref="IPropertyValueFormatter"/> implementations to selected properties.</remarks>
    [DataEditor(
    PropertyEditorAliases.TprHeaderFooterRichText,
    "TPR Rich text editor (Header and footer)",
    "rte",
    ValueType = ValueTypes.Text,
    HideLabel = false,
    Group = Constants.PropertyEditors.Groups.RichContent,
    Icon = "icon-browser-window",
    ValueEditorIsReusable = true)]
    public class TprHeaderFooterRichTextPropertyEditor : RichTextPropertyEditor
    {
        public TprHeaderFooterRichTextPropertyEditor(
            IDataValueEditorFactory dataValueEditorFactory,
            IEditorConfigurationParser editorConfigurationParser,
            IIOHelper ioHelper,
            IRichTextPropertyIndexValueFactory richTextPropertyIndexValueFactory) :
            base(dataValueEditorFactory,
                editorConfigurationParser,
                ioHelper,
                richTextPropertyIndexValueFactory)
        {
        }
    }
}
