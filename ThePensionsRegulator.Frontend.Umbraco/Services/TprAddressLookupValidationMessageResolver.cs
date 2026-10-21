using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services;

public sealed class TprAddressLookupValidationMessageResolver : ITprAddressLookupValidationMessageResolver
{
    private const string AddressLookupContentTypeAlias = "tprAddressLookup";

    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly ICultureDictionary _cultureDictionary;

    public TprAddressLookupValidationMessageResolver(
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        ICultureDictionary cultureDictionary)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
        _publishedValueFallback = publishedValueFallback;
        _cultureDictionary = cultureDictionary;
    }

    public TprAddressLookupContent? Resolve(string modelPropertyName)
    {
        if (string.IsNullOrWhiteSpace(modelPropertyName) ||
            !_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext) ||
            umbracoContext?.PublishedRequest?.PublishedContent is not { } publishedContent)
        {
            return null;
        }

        var matchingBlocks = publishedContent
            .FindOverridableBlockModels(_publishedValueFallback)
            .FindBlocks(block =>
                    string.Equals(block.Content.ContentType.Alias, AddressLookupContentTypeAlias, StringComparison.Ordinal) &&
                    string.Equals(
                        block.Settings?.Value<string>(_publishedValueFallback, PropertyAliases.ModelProperty),
                        modelPropertyName,
                        StringComparison.OrdinalIgnoreCase),
                _publishedValueFallback)
            .Take(2)
            .ToArray();

        if (matchingBlocks.Length != 1)
        {
            return null;
        }

        var block = matchingBlocks[0];
        return TprAddressLookupContentResolver.Resolve(block.Content, block.Settings, _cultureDictionary);
    }
}