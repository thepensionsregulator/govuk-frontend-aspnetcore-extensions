using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents a context within the TPR context bar component.
    /// </summary>
    public abstract class TprContextBarContextTagHelperBase : TagHelper
    {
        private readonly int _contextBarContextId;

        /// <summary>
        /// Gets or sets whether to allow HTML content.
        /// </summary>
        [HtmlAttributeName("allow-html")]
        public bool AllowHtml { get; set; }

        protected TprContextBarContextTagHelperBase(int contextBarContextId)
        {
            _contextBarContextId = contextBarContextId;
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = context.GetContextItem<TprContextBarContext>();

            var childContent = await output.GetChildContentAsync();

            barContext.SetContext(_contextBarContextId, output.Attributes.ToAttributeDictionary(), childContent.Snapshot(), AllowHtml);

            output.SuppressOutput();
        }
    }
}