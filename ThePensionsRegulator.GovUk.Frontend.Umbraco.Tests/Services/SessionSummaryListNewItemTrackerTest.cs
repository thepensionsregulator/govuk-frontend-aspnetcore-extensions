using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Services
{
    public class SessionSummaryListNewItemTrackerTest
    {
        private TestSessionContext _sessionContext;
        private CancellationToken _cancellationToken;
        private HttpContext _httpContext;
        private HttpContextAccessor _httpContextAccessor;
        private SessionSummaryListNewItemTracker _sut;
        private SummaryItemIdentityProvider _summaryItemIdentityProvider;
        private const string SessionKey = "GOVUK.SummaryList.NewItems";

        public SessionSummaryListNewItemTrackerTest() {

            _sessionContext = new TestSessionContext();
            _cancellationToken = TestContext.Current.CancellationToken;
            _httpContext = new DefaultHttpContext();
            _httpContext.Features.Set<ISessionFeature>(new TestSessionFeature { Session = _sessionContext });
            _httpContextAccessor = new HttpContextAccessor { HttpContext = _httpContext };
            _summaryItemIdentityProvider = new SummaryItemIdentityProvider();
            _sut = new SessionSummaryListNewItemTracker(_httpContextAccessor, _summaryItemIdentityProvider);
        }

        [Fact]
        public async Task Newly_added_item_is_new_until_session_ends()
        {

            var initalItems = new[]
            {
                CreateItem("1", "John"),
                CreateItem("2", "Alice"),
            };

            await _sut.MarkAsNew("3", _cancellationToken);
            await _sut.MarkAsNew("4", _cancellationToken);
            var initlResults = await _sut.TrackNewItemsAsync(initalItems, _cancellationToken);

            Assert.Empty(initlResults.NewItemIndexs);

            var listWithNewRows = new[]

                {
                CreateItem("1", "John"),
                CreateItem("2", "Alice"),
                CreateItem("3", "Charlies"),
                CreateItem("4", "Beht")

            };

            var newItems = await _sut.TrackNewItemsAsync(listWithNewRows, _cancellationToken);

            Assert.False(newItems.IsNew(0));
            Assert.False(newItems.IsNew(1));
            Assert.True(newItems.IsNew(2));
            Assert.True(newItems.IsNew(3));

            var refreshedResult = await _sut.TrackNewItemsAsync(listWithNewRows, _cancellationToken);

            Assert.False(refreshedResult.IsNew(0));
            Assert.False(refreshedResult.IsNew(1));
            Assert.False(refreshedResult.IsNew(2));
            Assert.False(refreshedResult.IsNew(3));
        }

        [Fact]
        public async Task TrackNewItemsAsync_Should_RemoveSessionData()
        {
            var items = new[]
            {
                CreateItem("1", "John"),
                CreateItem("2", "Alice")
            };
            var initialValue = _sessionContext.GetString(SessionKey);
            Assert.Null(initialValue);

            await _sut.MarkAsNew("2", _cancellationToken);
            var updatedValue = _sessionContext.GetString(SessionKey);
            Assert.DoesNotContain("Alice", updatedValue);

            await _sut.TrackNewItemsAsync(items, _cancellationToken);
            Assert.NotNull(updatedValue);

            await _sut.TrackNewItemsAsync(items, _cancellationToken);
            var finalValue = _sessionContext.GetString(SessionKey);
            Assert.Null(finalValue);
        }

        [Fact]
        public async Task MarkAsNew_Should_OnlyStoreId_InSession() {
            var items = new[]
            {
                CreateItem("1", "John"),
                CreateItem("2", "Alice")
            };

            await _sut.MarkAsNew("2", _cancellationToken);

            var storedValue = _sessionContext.GetString(SessionKey);
            Assert.DoesNotContain("Alice", storedValue);
        }

        [Fact]
        public async Task MarkAsNew_Should_AddTrackingId_ToSession()
        {
            // Arrange
            var sessionKey = "GOVUK.SummaryList.NewItems";
            var trackingId = "test-tracking-id";

            // Act
             await _sut.MarkAsNew(trackingId, _cancellationToken);

            // Assert
            var storedValue = _sessionContext.GetString(sessionKey); 

            Assert.NotNull(storedValue);

            var storedItems = JsonSerializer.Deserialize<HashSet<string>>(storedValue);

            Assert.Contains($"id:{trackingId}", storedItems);
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
