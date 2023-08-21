using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Templates;
using Umbraco.Cms.Infrastructure.Templates;

namespace GovUk.Frontend.Umbraco.PropertyEditors
{
    /// <summary>
    /// A rich text property editor identical to the default Umbraco rich text property editor.
    /// </summary>
    /// <remarks>This exists as a hook to apply different <see cref="IPropertyValueFormatter"/> implementations to selected properties.</remarks>
    [DataEditor(
    PropertyEditorAliases.GovUkInlineInverseRichText,
    "GOV.UK Rich text editor (Inline and inverse)",
    "rte",
    ValueType = ValueTypes.Text,
    HideLabel = false,
    Group = Constants.PropertyEditors.Groups.RichContent,
    Icon = "icon-browser-window",
    ValueEditorIsReusable = true)]
    public class GovUkInlineInverseRichTextPropertyEditor : RichTextPropertyEditor
    {
        public GovUkInlineInverseRichTextPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IBackOfficeSecurityAccessor backOfficeSecurityAccessor, HtmlImageSourceParser imageSourceParser, HtmlLocalLinkParser localLinkParser, RichTextEditorPastedImages pastedImages, IIOHelper ioHelper, IImageUrlGenerator imageUrlGenerator) : base(dataValueEditorFactory, backOfficeSecurityAccessor, imageSourceParser, localLinkParser, pastedImages, ioHelper, imageUrlGenerator)
        {
        }

        public GovUkInlineInverseRichTextPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IBackOfficeSecurityAccessor backOfficeSecurityAccessor, HtmlImageSourceParser imageSourceParser, HtmlLocalLinkParser localLinkParser, RichTextEditorPastedImages pastedImages, IIOHelper ioHelper, IImageUrlGenerator imageUrlGenerator, IHtmlMacroParameterParser macroParameterParser) : base(dataValueEditorFactory, backOfficeSecurityAccessor, imageSourceParser, localLinkParser, pastedImages, ioHelper, imageUrlGenerator, macroParameterParser)
        {
        }

        public GovUkInlineInverseRichTextPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IBackOfficeSecurityAccessor backOfficeSecurityAccessor, HtmlImageSourceParser imageSourceParser, HtmlLocalLinkParser localLinkParser, RichTextEditorPastedImages pastedImages, IIOHelper ioHelper, IImageUrlGenerator imageUrlGenerator, IHtmlMacroParameterParser macroParameterParser, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory, backOfficeSecurityAccessor, imageSourceParser, localLinkParser, pastedImages, ioHelper, imageUrlGenerator, macroParameterParser, editorConfigurationParser)
        {
        }
    }
}
