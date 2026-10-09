using System;
using ThePensionsRegulator.Frontend.Models;
using Umbraco.Cms.Web.Common.PublishedModels;
namespace GovUk.Frontend.Umbraco.ExampleApp.Models;

public class AddressLookupViewModel
{
    public AddressLookup Page { get; set; }
    public TprAddress Address { get; set; }
}
