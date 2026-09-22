using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Services
{
    public class SessionSummaryListNewItemTrackerTest
    {
        [Fact]
        public async Task Newly_added_item_is_new_until_session_ends()
        {
            // Arrange
            var session = new TestSessionContext();

            var httpContext = new DefaultHttpContext();
            httpContext.Features.Set<ISessionFeature>(
                new TestSessionFeature
                {
                    Session = session
                });
            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = httpContext
            };

            var identityProvider = new SummaryItemIdentityProvider();

            var tracker = new SessionSummaryListNewItemTracker(httpContextAccessor, identityProvider);

            const string summaryListId = "contacts";

            var initalItems = new[]
            {
                CreateItem("1", "John"),
                CreateItem("2", "Alice"),
            };

            var initlResults = await tracker.TrackNewItemsAsync(summaryListId, initalItems);

            Assert.Empty(initlResults.NewItemIndexs);

            var itemsWithNewRow = new[]

                {
                CreateItem("1", "John"),
                CreateItem("2", "Alice"),
                CreateItem("3", "Charlies")

            };

            var newItems = await tracker.TrackNewItemsAsync(summaryListId, itemsWithNewRow);

            Assert.False(newItems.IsNew(0));
            Assert.False(newItems.IsNew(1));
            Assert.True(newItems.IsNew(2));

            var refreshedResult = await tracker.TrackNewItemsAsync(summaryListId, itemsWithNewRow);

            Assert.False(refreshedResult.IsNew(0));
            Assert.False(refreshedResult.IsNew(1));
            Assert.False(refreshedResult.IsNew(2));
        }

        public static SummaryListItem CreateItem(string id, string name)
        {
            return new SummaryListItem("Name", new HtmlEncodedString(name)) { TrackingId = id };

        }
    }

    internal sealed class TestSessionContext : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();
        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();
        public IEnumerable<string> Keys => _sessionStorage.Keys;
        public void Clear() => _sessionStorage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _sessionStorage.Remove(key);
        public void Set(string key, byte[] value) => _sessionStorage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
    }

    internal sealed class TestSessionFeature : ISessionFeature
    {
        public ISession Session { get; set; } = new TestSessionContext();
    }
}
