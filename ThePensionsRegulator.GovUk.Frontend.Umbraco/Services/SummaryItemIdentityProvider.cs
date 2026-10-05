using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using Umbraco.Extensions;
using static Umbraco.Cms.Core.Constants.HttpContext;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class SummaryItemIdentityProvider : ISummaryItemIdentityProvider
    {
        public string? GetIdentity(SummaryListItem item)
        {
            if (string.IsNullOrWhiteSpace(item.TrackingId))
            {
                return null;
            }

            return GetIdentity(item.TrackingId) ;
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
