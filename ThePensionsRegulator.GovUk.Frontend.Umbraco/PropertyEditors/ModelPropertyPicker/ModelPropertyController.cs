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
            foreach (var provider in _modelPropertyProviders)
            {
                var propertyNames = provider.GetPropertyNames(alias);
                if (propertyNames?.Any() == true)
                {
                    return propertyNames;
                }
            }
            return Array.Empty<string>();
        }
    }
}
