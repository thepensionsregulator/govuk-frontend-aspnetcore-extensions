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

            //app.UseSmidge(bundles =>
            //{
            //    bundles.CreateCss("tpr-frontend-css", "/tpr/tpr.css");

            //    bundles.CreateJs("tpr-frontend-js", "~/govuk-frontend.min.js?v=5.13.0",
            //        "/_content/ThePensionsRegulator.GovUk.Frontend/govuk/govuk-js-init.js",
            //        "/_content/ThePensionsRegulator.Frontend/tpr/tpr-back-to-top.js",
            //        "/_content/ThePensionsRegulator.Frontend/tpr/tpr-side-navigation.js",
            //        "/_content/ThePensionsRegulator.Frontend/tpr/headermenu.js");
            //});

            return app;
        }
    }
}
