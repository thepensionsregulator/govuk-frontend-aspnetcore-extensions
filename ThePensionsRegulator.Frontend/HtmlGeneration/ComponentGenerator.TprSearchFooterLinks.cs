using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string DefaultShowMoreQuestionsContent = "Show more questions";
        public virtual TagBuilder GenerateTprSearchFooterLinks(TprSearchFooterLinks footerLinks)
        {
            var outer = new TagBuilder("footer");

            var list = new TagBuilder("ul");
            list.AddCssClass("govuk-list");
            list.AddCssClass("tpr-search-results__links");

            outer.InnerHtml.AppendHtml(list);


            if (footerLinks.Links.Any())
            {
                foreach(var link in footerLinks.Links)
                {
                    var li = CreateLink(link.Attributes, link.Content);
                    list.InnerHtml.AppendHtml(li);
                }
            }
            else
            {
                var builder1 = new HtmlContentBuilder();
                builder1.Append("Show more questions");
                var dictionary1 = new AttributeDictionary
                {
                    { "href", "/#" }
                };

                var builder2 = new HtmlContentBuilder();
                builder2.Append("Visit automatic enrolment Q&As");
                var dictionary2 = new AttributeDictionary
                {
                    {"href", "/#" }
                };
                
                var link1 = CreateLink(dictionary1, builder1);
                var link2 = CreateLink(dictionary2, builder2);

                list.InnerHtml.AppendHtml(link1);
                list.InnerHtml.AppendHtml(link2);
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
