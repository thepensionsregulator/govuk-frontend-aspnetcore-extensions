using GovUk.Frontend.AspNetCore.Extensions.Models;
using Microsoft.AspNetCore.Http;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Services
{
    public class UmbracoPaginationFactory : IUmbracoPaginationFactory
    {
        private readonly IQueryCollection? _queryString;

        public UmbracoPaginationFactory(IHttpContextAccessor httpContextAccessor)
        {
            _queryString = httpContextAccessor.HttpContext?.Request?.Query;
        }

        public PaginationModel CreateFromPaginationBlock(BlockListItem block)
        {
            var overridableBlock = block as OverridableBlockListItem ?? new OverridableBlockListItem(block);
            var pagination = new PaginationModel();

            pagination.QueryStringParameter = FromUmbracoSettingsOrDefault(overridableBlock, "queryStringParameter", pagination.QueryStringParameter);

            if (_queryString != null && _queryString.ContainsKey(pagination.QueryStringParameter) && int.TryParse(_queryString[pagination.QueryStringParameter], out var pageNumber))
            {
                pagination.PageNumber = pageNumber;
            }
            if (pagination.PageNumber <= 0) { pagination.PageNumber = 1; }

            var defaultPageSize = pagination.PageSize;
            pagination.PageSize = FromUmbracoSettingsOrDefault(overridableBlock, "pageSize", defaultPageSize);
            if (pagination.PageSize <= 0) { pagination.PageSize = defaultPageSize; }

            pagination.TotalItems = FromUmbracoSettingsOrDefault(overridableBlock, "totalItems", 0);

            pagination.CssClasses = FromUmbracoSettingsOrDefault(overridableBlock, PropertyAliases.CssClasses, string.Empty);
            pagination.LandmarkLabel = FromUmbracoSettingsOrDefault(overridableBlock, "landmarkLabel", pagination.LandmarkLabel);
            pagination.PreviousPageLabel = FromUmbracoSettingsOrDefault(overridableBlock, "previousPageLabel", pagination.PreviousPageLabel);
            pagination.NextPageLabel = FromUmbracoSettingsOrDefault(overridableBlock, "nextPageLabel", pagination.NextPageLabel);
            pagination.PageVisuallyHiddenText = FromUmbracoSettingsOrDefault(overridableBlock, "pageLabel", pagination.PageVisuallyHiddenText);

            var defaultLargeNumberOfPagesThreshold = pagination.LargeNumberOfPagesThreshold;
            pagination.LargeNumberOfPagesThreshold = FromUmbracoSettingsOrDefault(overridableBlock, "largeNumberOfPagesThreshold", defaultLargeNumberOfPagesThreshold);
            if (pagination.LargeNumberOfPagesThreshold <= 0) { pagination.LargeNumberOfPagesThreshold = defaultLargeNumberOfPagesThreshold; }

            return pagination;
        }

        private static int FromUmbracoSettingsOrDefault(OverridableBlockListItem block, string propertyName, int defaultValue)
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

        private static string FromUmbracoSettingsOrDefault(OverridableBlockListItem block, string propertyName, string defaultValue)
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
