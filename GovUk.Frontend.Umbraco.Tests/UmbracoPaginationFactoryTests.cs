using GovUk.Frontend.AspNetCore.Extensions.Models;
using GovUk.Frontend.Umbraco.Services;
using GovUk.Frontend.Umbraco.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class UmbracoPaginationFactoryTests
    {
#nullable disable
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private string _queryStringParameter = "custom-parameter";
        private int _pageNumberInQueryString = 33;
#nullable enable

        [SetUp]
        public void Setup()
        {
            _httpContextAccessor = new();

            var httpContext = new Mock<HttpContext>();
            _httpContextAccessor.SetupGet(x => x.HttpContext).Returns(httpContext.Object);

            var request = new Mock<HttpRequest>();
            httpContext.SetupGet(x => x.Request).Returns(request.Object);

            var query = new QueryCollection(new Dictionary<string, StringValues> { { _queryStringParameter, new StringValues(_pageNumberInQueryString.ToString()) } });
            request.SetupGet(x => x.Query).Returns(query);
        }

        [Test]
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


            Assert.AreEqual(defaults.PageNumber, pagination.PageNumber);
            Assert.AreEqual(defaults.PageSize, pagination.PageSize);
            Assert.AreEqual(defaults.TotalItems, pagination.TotalItems);
            Assert.AreEqual(string.IsNullOrEmpty(defaults.CssClasses), string.IsNullOrEmpty(pagination.CssClasses));
            Assert.AreEqual(defaults.LandmarkLabel, pagination.LandmarkLabel);
            Assert.AreEqual(defaults.PreviousPageLabel, pagination.PreviousPageLabel);
            Assert.AreEqual(defaults.NextPageLabel, pagination.NextPageLabel);
            Assert.AreEqual(defaults.PageVisuallyHiddenText, pagination.PageVisuallyHiddenText);
            Assert.AreEqual(defaults.QueryStringParameter, pagination.QueryStringParameter);
            Assert.AreEqual(defaults.LargeNumberOfPagesThreshold, pagination.LargeNumberOfPagesThreshold);
        }

        [Test]
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


            //Assert.AreEqual(defaults.PageNumber, pagination.PageNumber);
            Assert.AreEqual(pageSize, pagination.PageSize);
            Assert.AreEqual(totalItems, pagination.TotalItems);
            Assert.AreEqual(cssClasses, pagination.CssClasses);
            Assert.AreEqual(landmarkLabel, pagination.LandmarkLabel);
            Assert.AreEqual(previousPageLabel, pagination.PreviousPageLabel);
            Assert.AreEqual(nextPageLabel, pagination.NextPageLabel);
            Assert.AreEqual(pageVisuallyHiddenText, pagination.PageVisuallyHiddenText);
            Assert.AreEqual(_queryStringParameter, pagination.QueryStringParameter);
            Assert.AreEqual(largeNumberOfPagesThreshold, pagination.LargeNumberOfPagesThreshold);
        }

        [Test]
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


            Assert.AreEqual(_pageNumberInQueryString, pagination.PageNumber);
        }
    }
}
