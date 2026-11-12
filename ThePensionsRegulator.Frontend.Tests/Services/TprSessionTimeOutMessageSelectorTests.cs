using System.Collections.Generic;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;
using Xunit;

namespace ThePensionsRegulator.Frontend.Tests.Services
{
    public class TprSessionTimeOutMessageSelectorTests
    {
        private static readonly List<TprSessionTimeOutModel> Messages = new()
        {
            new TprSessionTimeOutModel { MessageKey = "warning", Title = "Warning" },
            new TprSessionTimeOutModel { MessageKey = "expired", Title = "Expired" }
        };

        [Fact]
        public void ReturnsMatchingKeyIgnoringCase()
        {
            var selected = TprSessionTimeOutMessageSelector.SelectByKey(Messages, "EXPIRED");
            Assert.Equal("Expired", selected?.Title);
        }

        [Fact]
        public void ReturnsFirstWhenKeyMissing()
        {
            var selected = TprSessionTimeOutMessageSelector.SelectByKey(Messages, null);
            Assert.Equal("Warning", selected?.Title);
        }

        [Fact]
        public void ReturnsNullWhenExplicitKeyUnknown()
        {
            var selected = TprSessionTimeOutMessageSelector.SelectByKey(Messages, "unknown");
            Assert.Null(selected);
        }

        [Fact]
        public void ReturnsNullWhenCollectionEmpty()
        {
            var selected = TprSessionTimeOutMessageSelector.SelectByKey(
                new List<TprSessionTimeOutModel>(), "warning");
            Assert.Null(selected);
        }
    }
}
