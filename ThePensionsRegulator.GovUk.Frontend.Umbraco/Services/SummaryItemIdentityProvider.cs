using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class SummaryItemIdentityProvider : ISummaryItemIdentityProvider
    {
        public string GetIdentity(SummaryListItem item)
        {
            if (!string.IsNullOrWhiteSpace(item.TrackingId))
            {
                return $"id:{item.TrackingId}";
            }

            var value = item.Value?.ToHtmlString() ?? string.Empty;

            var identity = string.Join("\u001F", item.Key.Normalize(), value);

            return $"id:{identity.GenerateHash()}"  ;
        }
    }

    public interface ISummaryItemIdentityProvider
    {
        string GetIdentity(SummaryListItem item);
    }
}
