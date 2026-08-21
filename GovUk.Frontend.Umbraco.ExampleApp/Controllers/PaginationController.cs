using GovUk.Frontend.Umbraco.ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Globalization;
using System.Web;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Validation;
using ThePensionsRegulator.GovUk.Frontend.Validation;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class PaginationController : RenderController
    {
        private readonly IUmbracoPaginationFactory _paginationFactory;
        private readonly IPublishedValueFallback _publishedValueFallback;
        public PaginationController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoPaginationFactory paginationFactory,
            IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _paginationFactory = paginationFactory ?? throw new ArgumentNullException(nameof(paginationFactory));
            _publishedValueFallback = publishedValueFallback ?? throw new ArgumentNullException(nameof(publishedValueFallback));
        }

        [ModelType(typeof(PaginationViewModel))]
        public override IActionResult Index()
        {
            var viewModel = new PaginationViewModel
            {
                Page = new Pagination(CurrentPage, _publishedValueFallback),
            };

            var block = viewModel.Page.Blocks?.FindBlockByContentTypeAlias(GovukPagination.ModelTypeAlias);
            if (block != null)
            {
                var pagination = _paginationFactory.CreateFromPaginationBlock(block);

                if (Request.Query.ContainsKey(nameof(viewModel.Items)) && int.TryParse(Request.Query[nameof(viewModel.Items)], out var items))
                {
                    if (items >= 0)
                    {
                        pagination.TotalItems = items;
                    }
                }
                else
                {
                    pagination.TotalItems = 50;
                }

                // Redirect users to the first page if they enter a URL of a page that no longer exists.
                if (pagination.TotalPages() < pagination.PageNumber)
                {
                    var path = Request.Path;
                    var query = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    query.Remove(pagination.QueryStringParameter);

                    return Redirect(path + (query.Count > 0 ? "?" + query.ToString() : string.Empty));
                }

                Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> filter;
                if (pagination.TotalItems > pagination.PageSize)
                {
                    if (pagination.TotalItems > (pagination.PageSize * pagination.LargeNumberOfPagesThreshold))
                    {
                        filter = block => block.Content.ContentType.Alias != GovukTypography.ModelTypeAlias ||
                            (!block.GridRowClassList().Contains("tpr-pagination-small") &&
                            !block.GridRowClassList().Contains("tpr-pagination-none")
                        );
                    }
                    else
                    {
                        filter = block => block.Content.ContentType.Alias != GovukTypography.ModelTypeAlias ||
                            (!block.GridRowClassList().Contains("tpr-pagination-none") &&
                            !block.GridRowClassList().Contains("tpr-pagination-large")
                        );
                    }
                }
                else
                {
                    filter = block => block.Content.ContentType.Alias != GovukTypography.ModelTypeAlias ||
                        (!block.GridRowClassList().Contains("tpr-pagination-small") &&
                        !block.GridRowClassList().Contains("tpr-pagination-large")
                    );
                }
                viewModel.Page.Blocks!.Filter = filter;
                viewModel.Page.Blocks.FindBlockByContentTypeAlias(GovukPagination.ModelTypeAlias)?
                    .Settings?.OverrideValue(nameof(GovukPaginationSettings.TotalItems), pagination.TotalItems);

                viewModel.Page.Grid!.Filter = filter;
                viewModel.Page.Grid.FindBlockByContentTypeAlias(GovukPagination.ModelTypeAlias)?
                    .Settings?.OverrideValue(nameof(GovukPaginationSettings.TotalItems), pagination.TotalItems);

                ModelState.SetInitialValue(nameof(viewModel.Items), pagination.TotalItems.ToString(CultureInfo.InvariantCulture));

                viewModel.PageTitle = viewModel.Page.PageHeadingOrName();
                if (pagination.TotalPages() > 1) { viewModel.PageTitle += $" (page {pagination.PageNumber} of {pagination.TotalPages()})"; }
            }

            return CurrentTemplate(viewModel);
        }
    }
}
