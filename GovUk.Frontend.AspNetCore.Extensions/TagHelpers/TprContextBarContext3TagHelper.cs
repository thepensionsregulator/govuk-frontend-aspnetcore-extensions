using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the context 3 area within the TPR context bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprContextBarTagHelper.TagName)]
    public class TprContextBarContext3TagHelper : TprContextBarContextTagHelperBase
    {
        internal const string TagName = "tpr-context-bar-context-3";

        public TprContextBarContext3TagHelper() : base(3) { }
    }
}