using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ModelPropertyPicker
{
    [VersionedApiBackOfficeRoute("model-property")]
    [ApiExplorerSettings(GroupName = "GOV.UK API")]
    public class ModelPropertyController(IEnumerable<IModelPropertyProvider> _modelPropertyProviders) : ManagementApiControllerBase
    {
        [HttpGet("{alias}")]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
        public IEnumerable<string> ForDocumentType(string alias)
        {
            var propertyNames = new List<string>();
            foreach (var provider in _modelPropertyProviders)
            {
                propertyNames.AddRange(provider.GetPropertyNames(alias));
            }
            return propertyNames;
        }
    }
}
