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

        public virtual TagBuilder GenerateTprNotificationBanner()
        {
            var tagBuilder = new TagBuilder(TprNotificationBannerElement);
            tagBuilder.InnerHtml.Append("my custom text for a TprNotificationBanner");
            return tagBuilder;
        }
    }
}
