using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class SessionSummaryListNewItemTracker : ISummaryListNewItemTracker
    {
        private const string SessionKeyPrefix = "SummaryListNewItems_";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISummaryItemIdentityProvider _summaryItemIdentityProvider;

        public SessionSummaryListNewItemTracker(IHttpContextAccessor httpContextAccessor, ISummaryItemIdentityProvider summaryItemIdentityProvider)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _summaryItemIdentityProvider = summaryItemIdentityProvider ?? throw new ArgumentNullException(nameof(summaryItemIdentityProvider));
        }
        public async Task<SummaryListTrackingResult> TrackNewItemsAsync(string summaryListId, IReadOnlyList<SummaryListItem> items, CancellationToken cancellationToken = default)
        {

            var session = _httpContextAccessor.HttpContext?.Session;

            if (session is null)
            {
                return SummaryListTrackingResult.Empty;
            }

            await session.LoadAsync(cancellationToken);

            var currentlyTrackedItems = GetIdentites(items);

            var sessionKey = GetSessionKey(summaryListId);

            var previouslyTrackedItems = GetPreviousIdentites(session, sessionKey);

            if (previouslyTrackedItems is null)
            {
                SaveSnapshot(session, sessionKey, currentlyTrackedItems);
                return SummaryListTrackingResult.Empty;
            }

            var previousCounts = previouslyTrackedItems.GroupBy(x => x)
                                                       .ToDictionary(g => g.Key, g => g.Count());
            var newItemIndexes = new HashSet<int>();

            for (var i = 0; i < currentlyTrackedItems.Count; i++)
            {
                var identity = currentlyTrackedItems[i];

                if (!previousCounts.TryGetValue(identity, out var count) || count == 0)
                {
                    newItemIndexes.Add(i);
                }
                else
                {
                    previousCounts[identity]--;
                }
            }

            SaveSnapshot(session, sessionKey, currentlyTrackedItems);

            return new SummaryListTrackingResult(newItemIndexes);
        }

        private IReadOnlyList<string> GetIdentites(IReadOnlyList<SummaryListItem> items)
        {
            var occurances = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var identities = new List<string>(items.Count());

            foreach (var item in items)
            {
                var identity = _summaryItemIdentityProvider.GetIdentity(item);
                if (identity is not null)
                {
                    identities.Add(identity);
                }
            }

            return identities;
        }

        private IReadOnlyList<string>? GetPreviousIdentites(ISession session, string sessionKey)
        {
            var value = session.GetString(sessionKey);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return JsonSerializer.Deserialize<IReadOnlyList<string>>(value);
        }

        private static void SaveSnapshot(ISession session, string sessionKey, IReadOnlyList<string> identities)
        {
            var serialized = JsonSerializer.Serialize(identities);
            session.SetString(sessionKey, serialized);
        }

        private static string GetSessionKey(string summaryListId)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(summaryListId));
            return $"{SessionKeyPrefix}{Convert.ToHexString(hash)}";
        }

    }

    public interface ISummaryListNewItemTracker
    {
        Task<SummaryListTrackingResult> TrackNewItemsAsync(string summaryListId, IReadOnlyList<SummaryListItem> items, CancellationToken cancellationToken = default);
    }
}
