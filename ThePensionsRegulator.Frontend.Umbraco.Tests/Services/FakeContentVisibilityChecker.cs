using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    internal class FakeContentVisibilityChecker : IContentVisibilityChecker
    {
        private readonly bool _isVisibleResult;

        public FakeContentVisibilityChecker(bool isVisibleResult)
        {
            _isVisibleResult = isVisibleResult;
        }
        public bool IsVisible(IPublishedContent content)
        {
            return _isVisibleResult;
        }
    }
}
