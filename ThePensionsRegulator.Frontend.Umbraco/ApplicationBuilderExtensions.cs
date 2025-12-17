using GovUk.Frontend.Umbraco;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Frontend.Umbraco
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTprFrontendUmbraco(
            this IApplicationBuilder app,
            IOptions<MvcOptions> mvcOptions,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback)
        {
            app.UseGovUkFrontendUmbraco(mvcOptions, umbracoContextAccessor, publishedValueFallback);

            return app;
        }
    }
}
