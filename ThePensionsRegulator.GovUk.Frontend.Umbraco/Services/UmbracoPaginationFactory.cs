using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.GovUk.Frontend.Models;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class UmbracoPaginationFactory : IUmbracoPaginationFactory
    {
        private readonly IQueryCollection? _queryString;
        private readonly IPublishedValueFallback _publishedValueFallback;

        public UmbracoPaginationFactory(IHttpContextAccessor httpContextAccessor, IPublishedValueFallback publishedValueFallback)
        {
            _queryString = httpContextAccessor.HttpContext?.Request?.Query;
            _publishedValueFallback = publishedValueFallback;
        }

        public PaginationModel CreateFromPaginationBlock(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
        {
            var pagination = new PaginationModel();

            pagination.QueryStringParameter = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "queryStringParameter", pagination.QueryStringParameter);

            if (_queryString != null && _queryString.ContainsKey(pagination.QueryStringParameter) && int.TryParse(_queryString[pagination.QueryStringParameter], out var pageNumber))
            {
                pagination.PageNumber = pageNumber;
            }
            if (pagination.PageNumber <= 0) { pagination.PageNumber = 1; }

            var defaultPageSize = pagination.PageSize;
            pagination.PageSize = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "pageSize", defaultPageSize);
            if (pagination.PageSize <= 0) { pagination.PageSize = defaultPageSize; }

            pagination.TotalItems = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "totalItems", 0);

            pagination.CssClasses = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, PropertyAliases.CssClasses, string.Empty);
            pagination.LandmarkLabel = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "landmarkLabel", pagination.LandmarkLabel);
            pagination.PreviousPageLabel = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "previousPageLabel", pagination.PreviousPageLabel);
            pagination.NextPageLabel = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "nextPageLabel", pagination.NextPageLabel);
            pagination.PageVisuallyHiddenText = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "pageLabel", pagination.PageVisuallyHiddenText);

            var defaultLargeNumberOfPagesThreshold = pagination.LargeNumberOfPagesThreshold;
            pagination.LargeNumberOfPagesThreshold = FromUmbracoSettingsOrDefault(block, _publishedValueFallback, "largeNumberOfPagesThreshold", defaultLargeNumberOfPagesThreshold);
            if (pagination.LargeNumberOfPagesThreshold <= 0) { pagination.LargeNumberOfPagesThreshold = defaultLargeNumberOfPagesThreshold; }

            return pagination;
        }

        private static int FromUmbracoSettingsOrDefault(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block, IPublishedValueFallback publishedValueFallback, string propertyName, int defaultValue)
        {
            var value = block.Settings?.Value<int?>(publishedValueFallback, propertyName);
            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                return defaultValue;
            }
        }

        private static string FromUmbracoSettingsOrDefault(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block, IPublishedValueFallback publishedValueFallback, string propertyName, string defaultValue)
        {
            var value = block.Settings?.Value<string>(publishedValueFallback, propertyName);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
