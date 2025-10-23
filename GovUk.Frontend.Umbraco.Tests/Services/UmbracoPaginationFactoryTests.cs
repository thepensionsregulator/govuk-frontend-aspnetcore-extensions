using GovUk.Frontend.AspNetCore.Extensions.Models;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Testing;

namespace GovUk.Frontend.Umbraco.Tests.Services
{
    public class UmbracoPaginationFactoryTests
    {
#nullable disable
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private string _queryStringParameter = "custom-parameter";
        private int _pageNumberInQueryString = 33;
#nullable enable

        public UmbracoPaginationFactoryTests()
        {
            _httpContextAccessor = new();

            var httpContext = new Mock<HttpContext>();
            _httpContextAccessor.SetupGet(x => x.HttpContext).Returns(httpContext.Object);

            var request = new Mock<HttpRequest>();
            httpContext.SetupGet(x => x.Request).Returns(request.Object);

            var query = new QueryCollection(new Dictionary<string, StringValues> { { _queryStringParameter, new StringValues(_pageNumberInQueryString.ToString()) } });
            request.SetupGet(x => x.Query).Returns(query);
        }

        [Fact]
        public void Uses_PaginationModel_defaults_if_settings_empty()
        {
            var factory = new UmbracoPaginationFactory(_httpContextAccessor.Object);
            var defaults = new PaginationModel();

            var pagination = factory.CreateFromPaginationBlock(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("govukPagination").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("govukPaginationSettings").Object
                    )
                );


            Assert.Equal(defaults.PageNumber, pagination.PageNumber);
            Assert.Equal(defaults.PageSize, pagination.PageSize);
            Assert.Equal(defaults.TotalItems, pagination.TotalItems);
            Assert.Equal(string.IsNullOrEmpty(defaults.CssClasses), string.IsNullOrEmpty(pagination.CssClasses));
            Assert.Equal(defaults.LandmarkLabel, pagination.LandmarkLabel);
            Assert.Equal(defaults.PreviousPageLabel, pagination.PreviousPageLabel);
            Assert.Equal(defaults.NextPageLabel, pagination.NextPageLabel);
            Assert.Equal(defaults.PageVisuallyHiddenText, pagination.PageVisuallyHiddenText);
            Assert.Equal(defaults.QueryStringParameter, pagination.QueryStringParameter);
            Assert.Equal(defaults.LargeNumberOfPagesThreshold, pagination.LargeNumberOfPagesThreshold);
        }

        [Fact]
        public void Uses_properties_from_settings()
        {
            var factory = new UmbracoPaginationFactory(_httpContextAccessor.Object);
            var pageSize = 100;
            var totalItems = 500;
            var cssClasses = "example-class";
            var landmarkLabel = "example-landmark";
            var previousPageLabel = "previous-page";
            var nextPageLabel = "next-page";
            var pageVisuallyHiddenText = "visually-hidden-text";
            var largeNumberOfPagesThreshold = 50;

            var pagination = factory.CreateFromPaginationBlock(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("govukPagination").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("govukPaginationSettings")
                    .SetupUmbracoIntegerPropertyValue("pageSize", pageSize)
                    .SetupUmbracoIntegerPropertyValue("totalItems", totalItems)
                    .SetupUmbracoTextboxPropertyValue("cssClasses", cssClasses)
                    .SetupUmbracoTextboxPropertyValue("landmarkLabel", landmarkLabel)
                    .SetupUmbracoTextboxPropertyValue("previousPageLabel", previousPageLabel)
                    .SetupUmbracoTextboxPropertyValue("nextPageLabel", nextPageLabel)
                    .SetupUmbracoTextboxPropertyValue("pageLabel", pageVisuallyHiddenText)
                    .SetupUmbracoTextboxPropertyValue("queryStringParameter", _queryStringParameter)
                    .SetupUmbracoIntegerPropertyValue("largeNumberOfPagesThreshold", largeNumberOfPagesThreshold)
                    .Object
                    )
                );


            Assert.Equal(pageSize, pagination.PageSize);
            Assert.Equal(totalItems, pagination.TotalItems);
            Assert.Equal(cssClasses, pagination.CssClasses);
            Assert.Equal(landmarkLabel, pagination.LandmarkLabel);
            Assert.Equal(previousPageLabel, pagination.PreviousPageLabel);
            Assert.Equal(nextPageLabel, pagination.NextPageLabel);
            Assert.Equal(pageVisuallyHiddenText, pagination.PageVisuallyHiddenText);
            Assert.Equal(_queryStringParameter, pagination.QueryStringParameter);
            Assert.Equal(largeNumberOfPagesThreshold, pagination.LargeNumberOfPagesThreshold);
        }

        [Fact]
        public void Page_number_from_querystring_respects_setting()
        {
            var factory = new UmbracoPaginationFactory(_httpContextAccessor.Object);

            var pagination = factory.CreateFromPaginationBlock(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("govukPagination").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("govukPaginationSettings")
                    .SetupUmbracoTextboxPropertyValue("queryStringParameter", _queryStringParameter)
                    .Object
                    )
                );


            Assert.Equal(_pageNumberInQueryString, pagination.PageNumber);
        }
    }
}
