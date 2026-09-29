using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using Umbraco.Extensions;
using static Umbraco.Cms.Core.Constants.HttpContext;

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

        public string GetIdentity(string trackingId)
        {
            return $"id:{trackingId.Trim()}"; 
        }
    }

    public interface ISummaryItemIdentityProvider
    {
        string GetIdentity(SummaryListItem item);
        string GetIdentity(string trackingId);
    }
}
