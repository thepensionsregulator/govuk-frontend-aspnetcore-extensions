using GovUk.Frontend.Umbraco;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smidge;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Frontend.Umbraco
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTprFrontendUmbraco(this IApplicationBuilder app, IOptions<MvcOptions> mvcOptions, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback)
        {
            app.UseGovUkFrontendUmbraco(mvcOptions, umbracoContextAccessor, publishedValueFallback);

            app.UseSmidge(bundles =>
            {
                bundles.CreateCss("tpr-frontend-css", "/_content/ThePensionsRegulator.Frontend.Umbraco/tpr/tpr.css");

                bundles.CreateJs("tpr-frontend-js", "~/govuk-frontend-4.6.0.min.js",
                    "/_content/ThePensionsRegulator.GovUk.Frontend/govuk/govuk-js-init.js",
                    "/_content/ThePensionsRegulator.Frontend/tpr/tpr-back-to-top.js");
            });

            return app;
        }
    }
}
