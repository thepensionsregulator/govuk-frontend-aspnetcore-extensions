using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGovUkFrontendUmbracoExtensions(this IApplicationBuilder app, IOptions<MvcOptions> mvcOptions, IUmbracoContextAccessor umbracoContextAccessor)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            mvcOptions.Value.ModelMetadataDetailsProviders.Add(new UmbracoBlockListValidationMetadataProvider(umbracoContextAccessor));

            return app;
        }
    }
}