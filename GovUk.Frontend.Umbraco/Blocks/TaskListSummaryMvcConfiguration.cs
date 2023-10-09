using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GovUk.Frontend.Umbraco.Blocks
{
    internal class TaskListSummaryMvcConfiguration : IConfigureOptions<MvcOptions>
    {
        public void Configure(MvcOptions options)
        {
            options.Filters.Add<TaskListSummaryActionFilter>();
        }
    }
}