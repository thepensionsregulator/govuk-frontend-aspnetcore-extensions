using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TprContextBarContext
    {
        private (AttributeDictionary Attributes, IHtmlContent Content, bool AllowHtml)[] _content = new (AttributeDictionary, IHtmlContent, bool)[3];

        public AttributeDictionary Context1Attributes => _content[0].Attributes;
        public IHtmlContent? Context1Content => _content[0].Content;
        public bool Context1AllowHtml => _content[0].AllowHtml;
        public AttributeDictionary Context2Attributes => _content[1].Attributes;
        public IHtmlContent? Context2Content => _content[1].Content;
        public bool Context2AllowHtml => _content[1].AllowHtml;
        public AttributeDictionary Context3Attributes => _content[2].Attributes;
        public IHtmlContent? Context3Content => _content[2].Content;
        public bool Context3AllowHtml => _content[2].AllowHtml;

        public void SetContext(int contextBarContextId, AttributeDictionary attributes, IHtmlContent content, bool allowHtml)
        {
            Guard.ArgumentValid(nameof(contextBarContextId), $"{nameof(contextBarContextId)} must be between 1 and 3", contextBarContextId >= 1 && contextBarContextId <= 3);
            Guard.ArgumentNotNull(nameof(content), content);

            if (_content[contextBarContextId - 1].Content != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TprContextBarTagHelper.TagName + "-context-" + contextBarContextId,
                    TprContextBarTagHelper.TagName);
            }

            _content[contextBarContextId - 1] = (attributes, content, allowHtml);
        }
    }
}