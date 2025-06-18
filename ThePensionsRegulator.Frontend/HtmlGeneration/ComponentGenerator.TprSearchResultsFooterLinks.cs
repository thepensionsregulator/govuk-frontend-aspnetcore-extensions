using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string DefaultShowMoreQuestionsContent = "Show more questions";
        public virtual TagBuilder GenerateTprSearchResultsFooterLinks(TprSearchFooterLinks footerLinks)
        {
            var outer = new TagBuilder("footer");

            var list = new TagBuilder("ul");
            list.AddCssClass("govuk-list");
            list.AddCssClass("tpr-search-results__links");

            outer.InnerHtml.AppendHtml(list);

            var showMoreQuestionsAttributes = new AttributeDictionary
            {
                {"id", "show-more-questions" }
            };

            var builder = new HtmlContentBuilder();
            builder.Append(DefaultShowMoreQuestionsContent);

            var li = CreateLink(showMoreQuestionsAttributes, builder);

            list.InnerHtml.AppendHtml(li);

            foreach(var link in footerLinks.Links)
            {
                var anchorListItem = CreateLink(link.Attributes, link.Content);
                list.InnerHtml.AppendHtml(anchorListItem);
            }

            return outer;
        }


        private TagBuilder CreateLink(AttributeDictionary dictionary, IHtmlContent content)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");
            a.MergeAttributes(dictionary);
            a.MergeCssClass("govuk-link");
            a.InnerHtml.AppendHtml(content);
            li.InnerHtml.AppendHtml(a);

            return li;
        }
    }
}
