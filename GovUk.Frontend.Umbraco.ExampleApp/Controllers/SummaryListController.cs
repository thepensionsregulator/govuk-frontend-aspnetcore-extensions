using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

        public SummaryListController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedSnapshotAccessor publishedSnapshotAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedSnapshotAccessor = publishedSnapshotAccessor ?? throw new System.ArgumentNullException(nameof(publishedSnapshotAccessor));
        }

        [ModelType(typeof(SummaryList))]
        public override IActionResult Index()
        {
            var viewModel = new SummaryList(CurrentPage, null);

            // Override content in a summary list
            var summaryListToOverride = viewModel.Blocks!.FindBlockByClass("override-this");
            if (summaryListToOverride is not null)
            {
                var summaryListItems = new List<SummaryListItem>();
                for (var i = 1; i <= 5; i++)
                {
                    var summaryListItem = new SummaryListItem($"Data source item {i}", new HtmlEncodedString($"Data source item value {i}"));
                    summaryListItem.Actions.Add(new SummaryListAction(new Link { Url = "https://www.example.org" }, $"Action {i}"));
                    summaryListItems.Add(summaryListItem);
                }
                summaryListToOverride.Content.OverrideSummaryListItems(summaryListItems, _publishedSnapshotAccessor);
            }

            return CurrentTemplate(viewModel);
        }
    }
}
