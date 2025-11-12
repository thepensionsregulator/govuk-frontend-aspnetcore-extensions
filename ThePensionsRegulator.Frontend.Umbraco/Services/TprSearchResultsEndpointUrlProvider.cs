using Microsoft.Extensions.Configuration;
using System;
using ThePensionsRegulator.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface ITprSearchResultsEndpointUrlProvider
    {
        TprSearchResultEndpoints GetSearchResultsEndpoints(Guid? searchBoostingCategory);
    }

    public class TprQueryBasedSearchResultsEndpointUrlProvider : ITprSearchResultsEndpointUrlProvider
    {
        private readonly IConfigurationSection _searchResultsSection;

        public TprQueryBasedSearchResultsEndpointUrlProvider(IConfiguration configuration)
        {
            _searchResultsSection = configuration.GetSection("SearchResultsApiEndpoints");
        }
        public TprSearchResultEndpoints GetSearchResultsEndpoints(Guid? searchBoostingCategory)
        {
            var endpoints = new TprSearchResultEndpoints
            {
                ContentByIdApiUrl = _searchResultsSection["ContentByIdUrl"] ?? string.Empty,
                ContentSearchApiUrl = _searchResultsSection["SearchContentUrl"] ?? string.Empty,
                PopularContentApiUrl = _searchResultsSection["PopularContentUrl"] ?? string.Empty
            };
            if (searchBoostingCategory.HasValue)
            {
                string searchBoostingCategoryParam = $"?{TprPropertyAliases.SearchBoostingCategory}={searchBoostingCategory.Value}";
                endpoints.ContentSearchApiUrl += searchBoostingCategoryParam;
                endpoints.PopularContentApiUrl += searchBoostingCategoryParam;
            }
            return endpoints;
        }
    }
}
