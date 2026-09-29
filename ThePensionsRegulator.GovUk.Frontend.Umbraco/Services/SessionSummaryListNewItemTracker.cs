using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class SessionSummaryListNewItemTracker : ISummaryListNewItemTracker
    {
        private const string SessionKeyPrefix = "GOVUK.SummaryList.NewItems";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISummaryItemIdentityProvider _summaryItemIdentityProvider;

        public SessionSummaryListNewItemTracker(IHttpContextAccessor httpContextAccessor, ISummaryItemIdentityProvider summaryItemIdentityProvider)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _summaryItemIdentityProvider = summaryItemIdentityProvider ?? throw new ArgumentNullException(nameof(summaryItemIdentityProvider));
        }
        public async Task<SummaryListTrackingResult> TrackNewItemsAsync(IReadOnlyList<SummaryListItem> items, CancellationToken cancellationToken = default)
        {
            if(items.Count == 0) return SummaryListTrackingResult.Empty;

            var session = GetSession();

            await session.LoadAsync(cancellationToken);

            var pendingItems = GetPendingItems(session);
            if(pendingItems is null) return SummaryListTrackingResult.Empty;

            var identites = GetIdentities(items);

            var newItemIndexes = identites.Select((identity, index) => new { Identity = identity, Index = index })
                                           .Where(x => pendingItems.Contains(x.Identity))
                                           .Select(x => x.Index)
                                           .ToHashSet();

            if (newItemIndexes.Count == 0) return SummaryListTrackingResult.Empty;

            var viewedIdentities = newItemIndexes.Select(index => identites[index]).ToHashSet(StringComparer.OrdinalIgnoreCase);
            pendingItems.ExceptWith(viewedIdentities);

            SavePendingItems(session, pendingItems);

            return new SummaryListTrackingResult(newItemIndexes);
        }

        private IReadOnlyList<string> GetIdentities(IReadOnlyList<SummaryListItem> items)
        {        
            return items.Select(_summaryItemIdentityProvider.GetIdentity).ToList();
        }

        public async Task MarkAsNew(string trackingId, CancellationToken cancellationToken = default)
        {
            var session = GetSession();

            if(session is null) { return; }

            await session.LoadAsync(cancellationToken);

            var newItems = GetPendingItems(session);

            newItems.Add(_summaryItemIdentityProvider.GetIdentity(trackingId));

            SavePendingItems(session, newItems);

        }

        private void SavePendingItems(ISession session, HashSet<string> items)
        {
            if(items.Count == 0)
            {
                session.Remove(SessionKeyPrefix);
                return;
            }

            var value = JsonSerializer.Serialize(items);
            session.SetString(SessionKeyPrefix, value);
        }

        private HashSet<string> GetPendingItems(ISession session)
        {
            var value = session.GetString(SessionKeyPrefix);
            if(string.IsNullOrWhiteSpace(value))
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }
           
            var items = JsonSerializer.Deserialize<HashSet<string>>(value);

            return items is null ? new HashSet<string>(StringComparer.OrdinalIgnoreCase) : new HashSet<string>(items, StringComparer.OrdinalIgnoreCase);
        }

        private ISession GetSession()
        {
            return _httpContextAccessor.HttpContext?.Session ?? throw new InvalidOperationException("Session is not available.");
        }
    }

    public interface ISummaryListNewItemTracker
    { 
        Task MarkAsNew(string trackingId, CancellationToken cancellationToken = default);
        Task<SummaryListTrackingResult> TrackNewItemsAsync(IReadOnlyList<SummaryListItem> items, CancellationToken cancellationToken = default);
    }
}
