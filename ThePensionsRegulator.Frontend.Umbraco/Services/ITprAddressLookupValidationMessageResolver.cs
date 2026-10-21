using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.Umbraco.Services;

public interface ITprAddressLookupValidationMessageResolver
{
    TprAddressLookupContent? Resolve(string modelPropertyName);
}