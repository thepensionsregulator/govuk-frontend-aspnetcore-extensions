﻿using GovUk.Frontend.Umbraco.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class TaskListViewModel
    {
        public TaskList? Page { get; set; }

        public OverridableBlockListModel? Blocks { get; set; }
    }
}