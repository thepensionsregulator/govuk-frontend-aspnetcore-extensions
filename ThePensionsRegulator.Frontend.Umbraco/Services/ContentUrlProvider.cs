using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface IContentUrlProvider
    {
        string GetUrl(IPublishedContent content);
    }
    internal class ContentUrlProvider : IContentUrlProvider
    {
        public string GetUrl(IPublishedContent content)
        {
            return content.Url();
        }
    }
}
