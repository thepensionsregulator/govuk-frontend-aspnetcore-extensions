using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GovUk.Frontend.Umbraco;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Smidge;
using ThePensionsRegulator.Frontend.Umbraco.Validation;
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

            mvcOptions.Value.ModelMetadataDetailsProviders.Add(new AddressLookupValidationMetadataProvider(
                umbracoContextAccessor,
                publishedValueFallback,
                new Dictionary<Type, string>
                {
                    { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired },
                    { typeof(MaxLengthAttribute), PropertyAliases.ErrorMessageMaxLength },
                }));

            app.UseSmidge(bundles =>
            {
                bundles.CreateCss("tpr-frontend-css", "/_content/ThePensionsRegulator.Frontend.Umbraco/tpr/tpr.css");

                bundles.CreateJs("tpr-frontend-js", "~/govuk-frontend.min.js?v=5.13.0",
                    "/_content/ThePensionsRegulator.GovUk.Frontend/govuk/govuk-js-init.js",
                    "/_content/ThePensionsRegulator.Frontend/tpr/tpr-back-to-top.js",
                    "/_content/ThePensionsRegulator.Frontend/tpr/tpr-side-navigation.js",
                    "/_content/ThePensionsRegulator.Frontend/tpr/headermenu.js");
            });

            return app;
        }
    }
}
