using GovUk.Frontend.AspNetCore.Extensions.Models;
using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models.Blocks;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

namespace GovUk.Frontend.Umbraco.Services
{
    public class UmbracoPaginationFactory : IUmbracoPaginationFactory
    {
        private readonly IQueryCollection? _queryString;

        public UmbracoPaginationFactory(IHttpContextAccessor httpContextAccessor)
        {
            _queryString = httpContextAccessor.HttpContext?.Request?.Query;
        }

        public PaginationModel CreateFromPaginationBlock(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
        {
            var pagination = new PaginationModel();

            pagination.QueryStringParameter = FromUmbracoSettingsOrDefault(block, "queryStringParameter", pagination.QueryStringParameter);

            if (_queryString != null && _queryString.ContainsKey(pagination.QueryStringParameter) && int.TryParse(_queryString[pagination.QueryStringParameter], out var pageNumber))
            {
                pagination.PageNumber = pageNumber;
            }
            if (pagination.PageNumber <= 0) { pagination.PageNumber = 1; }

            var defaultPageSize = pagination.PageSize;
            pagination.PageSize = FromUmbracoSettingsOrDefault(block, "pageSize", defaultPageSize);
            if (pagination.PageSize <= 0) { pagination.PageSize = defaultPageSize; }

            pagination.TotalItems = FromUmbracoSettingsOrDefault(block, "totalItems", 0);

            pagination.CssClasses = FromUmbracoSettingsOrDefault(block, PropertyAliases.CssClasses, string.Empty);
            pagination.LandmarkLabel = FromUmbracoSettingsOrDefault(block, "landmarkLabel", pagination.LandmarkLabel);
            pagination.PreviousPageLabel = FromUmbracoSettingsOrDefault(block, "previousPageLabel", pagination.PreviousPageLabel);
            pagination.NextPageLabel = FromUmbracoSettingsOrDefault(block, "nextPageLabel", pagination.NextPageLabel);
            pagination.PageVisuallyHiddenText = FromUmbracoSettingsOrDefault(block, "pageLabel", pagination.PageVisuallyHiddenText);

            var defaultLargeNumberOfPagesThreshold = pagination.LargeNumberOfPagesThreshold;
            pagination.LargeNumberOfPagesThreshold = FromUmbracoSettingsOrDefault(block, "largeNumberOfPagesThreshold", defaultLargeNumberOfPagesThreshold);
            if (pagination.LargeNumberOfPagesThreshold <= 0) { pagination.LargeNumberOfPagesThreshold = defaultLargeNumberOfPagesThreshold; }

            return pagination;
        }

        private static int FromUmbracoSettingsOrDefault(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block, string propertyName, int defaultValue)
        {
            var value = block.Settings.Value<int?>(propertyName);
            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                return defaultValue;
            }
        }

        private static string FromUmbracoSettingsOrDefault(IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block, string propertyName, string defaultValue)
        {
            var value = block.Settings.Value<string>(propertyName);
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
