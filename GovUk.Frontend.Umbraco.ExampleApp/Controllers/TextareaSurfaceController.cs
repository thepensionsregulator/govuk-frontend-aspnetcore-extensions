using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class TextareaSurfaceController : SurfaceController
    {
        public TextareaSurfaceController(IUmbracoDatabaseFactory umbracoDatabaseFactory,
            IUmbracoContextAccessor umbracoContextAccessor,
            ServiceContext context,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider
            )
            : base(umbracoContextAccessor, umbracoDatabaseFactory, context, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateUmbracoFormRouteString]
        [ModelType(typeof(TextareaViewModel))]
        public IActionResult Index(TextareaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.Page?.NextPage != null)
                {
                    Response.StatusCode = 303;
                    Response.GetTypedHeaders().Location = new Uri(viewModel.Page.NextPage.Url(), UriKind.RelativeOrAbsolute);
                }
            }

            return View("Textarea", viewModel);
        }

    }
}
