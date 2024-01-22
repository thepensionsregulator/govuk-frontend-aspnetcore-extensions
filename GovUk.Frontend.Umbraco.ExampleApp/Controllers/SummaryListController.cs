using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Controllers
{
    public class SummaryListController : RenderController
    {
        private readonly IPublishedSnapshotAccessor accessor;

        public SummaryListController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IPublishedSnapshotAccessor accessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            this.accessor = accessor;
        }

        [ModelType(typeof(SummaryList))]
        public override IActionResult Index()
        {
            var viewModel = new SummaryList(CurrentPage, null);

            var summaryList = viewModel.Blocks.FindBlockByContentTypeAlias(GovukSummaryList.ModelTypeAlias);

            var summaryListItem = new SummaryListItem("key", new HtmlEncodedString("key"));
            summaryListItem.Actions.Add(new SummaryListAction(new Link { Url = "https://www.google.com" }, "my link text"));

            List<SummaryListItem> summaryListItems = new();
            summaryListItems.Add(summaryListItem);
            summaryListItems.Add(summaryListItem);
            summaryListItems.Add(summaryListItem);
            summaryListItems.Add(summaryListItem);
            summaryListItems.Add(summaryListItem);

            summaryList.Content.OverrideSummaryListItems(summaryListItems, accessor);

            // Override content in the block list
            //viewModel.Blocks!.FindBlockByClass("full-name")?.Content.OverrideValue(nameof(GovukSummaryListItem.ItemValue), "Sarah Smith");

            return CurrentTemplate(viewModel);
        }
    }
}
