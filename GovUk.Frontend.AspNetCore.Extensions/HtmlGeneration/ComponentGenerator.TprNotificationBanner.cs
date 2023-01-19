using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprNotificationBannerElement = "div";
        internal const string BannerCssClass = "tpr-notification-banner";
        internal const string ContentCssClass = "tpr-notificaiton-banner__content";

        public virtual TagBuilder GenerateTprNotificationBanner(IHtmlContent content)
        {
            var contentDiv = new TagBuilder(TprNotificationBannerElement);
            contentDiv.InnerHtml.AppendHtml(content);
            contentDiv.AddCssClass(ContentCssClass);

            var tagBuilder = new TagBuilder(TprNotificationBannerElement);
            tagBuilder.AddCssClass(BannerCssClass);
            tagBuilder.InnerHtml.AppendHtml(contentDiv);

            return tagBuilder;
        }
    }
}
