using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    [VersionedApiBackOfficeRoute("model-property")]
    [ApiExplorerSettings(GroupName = "GOV.UK API")]
    public class ModelPropertyController : ManagementApiControllerBase
    {
        [HttpGet("{alias}")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        public IEnumerable<string> ForDocumentType(string alias)
        {
            var filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
           
            List<Type> controllers = new();

            var files = Directory.GetFiles(filepath, "*.dll").ToList();

            files.RemoveAll(x => x.Contains(@"\System.") || x.Contains(@"\Microsoft.")); // Don't load dlls where there won't be any custom code. This list is not exhaustive
            
            foreach(var file in files) 
            { 
                Assembly assembly = Assembly.LoadFrom(file);
                List<Type> controllersToAdd = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(RenderController))).ToList();
                controllers.AddRange(controllersToAdd);
            }
            var controllerType = controllers?.FirstOrDefault(x => x.Name.ToUpperInvariant() == $"{alias.ToUpperInvariant()}CONTROLLER");
            if (controllerType != null)
            {
                var actionMethods = controllerType.GetMethods().Where(x => x.Name == "Index");
                foreach (var method in actionMethods)
                {
                    var modelType = (method?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
                    if (modelType != null)
                    {
                        return modelType.GetProperties().Where(x =>
                            !x.PropertyType.IsSubclassOf(typeof(PublishedContentModel)) &&
                            !x.PropertyType.IsAssignableTo(typeof(OverridableBlockListModel))
                            ).Select(x => x.Name);
                    }
                }
            }

            return Array.Empty<string>();
        }
    }
}
