using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupPrimaryAndSecondaryDataProvidedViewModel : AddressLookupBaseViewModel
    {
        public AddressLookupPrimaryAndSecondaryDataProvided? Page { get; set; }
    }
}
