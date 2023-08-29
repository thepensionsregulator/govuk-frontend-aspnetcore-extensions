using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Represents the context 1 area within the TPR context bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprContextBarTagHelper.TagName)]
    public class TprContextBarContext1TagHelper : TprContextBarContextTagHelperBase
    {
        internal const string TagName = "tpr-context-bar-context-1";

        public TprContextBarContext1TagHelper() : base(1) { }
    }
}