﻿using GovUk.Frontend.Umbraco.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class SummaryCardViewModel
    {
        public SummaryCard? Page { get; set; }

        public OverridableBlockListModel? Blocks { get; set; }
    }
}