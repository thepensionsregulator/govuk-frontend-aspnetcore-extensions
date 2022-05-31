using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace GovUk.Frontend.Umbraco.Validation
{
    [PluginController("GOVUK")]
    public class ModelPropertyController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public IEnumerable<string> ForDocumentType(string alias)
        {
            var controllers = Assembly.GetEntryAssembly()?.GetTypes().Where(x => x.IsSubclassOf(typeof(RenderController)));
            var controllerType = controllers?.FirstOrDefault(x => x.Name.ToUpperInvariant() == $"{alias.ToUpperInvariant()}CONTROLLER");
            if (controllerType != null)
            {
                var actionMethod = controllerType.GetMethod("Index");
                var modelType = (actionMethod?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
                if (modelType != null)
                {
                    return modelType.GetProperties().Where(x => !x.PropertyType.IsSubclassOf(typeof(PublishedContentModel))).Select(x => x.Name);
                }
            }

            return Array.Empty<string>();
        }
    }
}
