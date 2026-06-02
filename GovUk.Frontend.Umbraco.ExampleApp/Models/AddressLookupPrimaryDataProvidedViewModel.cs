using System.ComponentModel.DataAnnotations;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class AddressLookupPrimaryDataProvidedViewModel : AddressLookupBaseViewModel
    {
        public AddressLookupPrimaryDataProvided? Page { get; set; }
    }
}
