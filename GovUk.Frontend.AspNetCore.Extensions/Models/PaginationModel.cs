using Microsoft.AspNetCore.Http;
using System;

namespace GovUk.Frontend.AspNetCore.Extensions.Models
{
    /// <summary>
    /// Configuration settings for an instance of the GOV.UK Design System Pagination component.
    /// </summary>
    public class PaginationModel
    {
        /// <summary>
        /// The current page.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of items per page. If <c>null</c> then pagination is not active.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// The total number of items to be paginated.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// The total number of pages based on <see cref="TotalItems"/> and <see cref="PageSize"/>
        /// </summary>
        public int TotalPages()
        {
            if (!PageSize.HasValue || PageSize <= 0 || TotalItems <= 0)
            {
                return 1;
            }
            return (int)Math.Ceiling((decimal)TotalItems / PageSize.Value);
        }

        /// <summary>
        /// The label for the navigation landmark that wraps the pagination.
        /// </summary>
        public string LandmarkLabel = "results";

        /// <summary>
        /// The link label which adds context to the 'Previous' link when there is a small number of pages.
        /// </summary>
        /// <remarks>{{page}} is replaced with the number of the previous page and {{total}} is replaced with the total number of pages</remarks>
        public string PreviousPageLabel = "{{page}} of {{total}}";

        /// <summary>
        /// The link label which adds context to the 'Next' link when there is a small number of pages.
        /// </summary>
        /// <remarks>{{page}} is replaced with the number of the next page and {{total}} is replaced with the total number of pages</remarks>
        public string NextPageLabel = "{{page}} of {{total}}";

        /// <summary>
        /// The aria-label which gives more context to links which point to individual pages by their page number alone.
        /// </summary>
        /// <remarks>{{page}} is replaced with the number of the page</remarks>
        public string PageVisuallyHiddenText = "Page {{page}}";

        /// <summary>
        /// The name of the query string parameter used to indicate the current page.
        /// </summary>
        public string QueryStringParameter { get; set; } = "page";

        /// <summary>
        /// The GOV.UK Design System recommends a different styles of pagination for small numbers of pages and large numbers of pages. This threshold controls when the style changes.
        /// </summary>
        public int LargeNumberOfPagesThreshold { get; set; } = 10;

        /// <summary>
        /// The path that page links should target, if different from the path of the current request.
        /// </summary>
        public PathString? Path { get; set; }

        /// <summary>
        /// The querystring that page links should target, if different from the querystring of the current request.
        /// </summary>
        public QueryString? QueryString { get; set; }
    }
}
