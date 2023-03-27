using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the context 2 area within the TPR context bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprContextBarTagHelper.TagName)]
    public class TprContextBarContext2TagHelper : TprContextBarContextTagHelperBase
    {
        internal const string TagName = "tpr-context-bar-context-2";

        public TprContextBarContext2TagHelper() : base(2) { }
    }
}