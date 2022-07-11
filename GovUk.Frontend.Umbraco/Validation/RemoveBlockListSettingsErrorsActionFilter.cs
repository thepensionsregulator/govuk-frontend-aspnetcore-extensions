using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net.Http;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    /// <summary>
    /// From Umbraco 10 where a view model has a ModelsBuilder model as a property, and the document type it represents has an Umbraco block list property with a 'Settings' element type,
    /// ModelState contains additional errors saying the Settings and SettingsUdi properties are required. This removes those errors from ModelState before they are a problem.
    /// </summary>
    public class RemoveBlockListSettingsErrorsActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Method != HttpMethod.Post.Method) { return; }

            foreach (var argument in context.ActionArguments.Values.Where(x => x != null))
            {
                foreach (var property in argument!.GetType().GetPublicProperties())
                {
                    if (property.PropertyType.IsAssignableTo(typeof(PublishedContentModel)))
                    {
                        foreach (var key in context.ModelState.Keys.Where(x => x.StartsWith(property.Name + ".")))
                        {
                            context.ModelState.Remove(key);
                        }
                    }
                }

            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
